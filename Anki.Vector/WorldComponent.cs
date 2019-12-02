// <copyright file="WorldComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Anki.Vector.Events;
using Anki.Vector.Exceptions;
using Anki.Vector.Objects;
using Anki.Vector.Types;

namespace Anki.Vector
{
    /// <summary>
    /// Vector’s known view of his world.
    /// <para>This view includes objects and faces it knows about and can currently see with its camera.</para>
    /// </summary>
    public class WorldComponent : Component
    {
        /// <summary>
        /// The collection of objects in the world
        /// </summary>
        private readonly ObservableCollection<ObservableObject> objects = new ObservableCollection<ObservableObject>();

        /// <summary>
        /// The objects tracked by the world
        /// </summary>
        private readonly Dictionary<int, ObjectWithId> objectsById = new Dictionary<int, ObjectWithId>();

        /// <summary>
        /// The tracked faces
        /// </summary>
        private readonly Dictionary<int, Face> facesById = new Dictionary<int, Face>();

        /// <summary>
        /// The custom object archetypes
        /// </summary>
        private readonly Dictionary<CustomObjectType, CustomObjectArchetype> customObjectArchetypes = new Dictionary<CustomObjectType, CustomObjectArchetype>();

        /// <summary>
        /// A read only collection of objects in the world (lazily created)
        /// </summary>
        private ReadOnlyObservableCollection<ObservableObject> objectsReadOnly = null;

        /// <summary>
        /// The object visibility timeout in milliseconds
        /// </summary>
        public const int ObjectVisibilityTimeout = 2500;

        /// <summary>
        /// Occurs when object event occurs.
        /// </summary>
        public event EventHandler<ObjectEventArgs> ObjectEvent;

        /// <summary>
        /// Occurs when a new object is added to the world.  This will occur only once when a new object is first visually identified by the robot.  From then on
        /// the object will be available in the Objects collection.
        /// </summary>
        public event EventHandler<ObjectAddedEventArgs> ObjectAdded;

        /// <summary>
        /// Triggered whenever an object is first visually identified by a robot.
        /// <para>This differs from <see cref="ObjectObserved"/> in that it’s only triggered when an object initially becomes visible.  If it disappears for more than <see cref="ObjectVisibilityTimeout"/> milliseconds 
        /// and then is seen again, a <see cref="ObjectDisappeared"/> will be dispatched, followed by another <see cref="ObjectAppeared"/> event.</para>
        /// <para>For continuous tracking information about a visible object, see <see cref="ObjectObserved"/>.</para>
        /// </summary>
        public event EventHandler<ObjectAppearedEventArgs> ObjectAppeared;

        /// <summary>
        /// Triggered whenever an object that was previously being observed is no longer visible.
        /// </summary>
        public event EventHandler<ObjectDisappearedEventArgs> ObjectDisappeared;

        /// <summary>
        /// Triggered whenever an object is visually identified by the robot.
        /// <para>A stream of these events are produced while an object is visible to the robot.Each event has an updated image_box field.
        /// See <see cref="ObjectAppeared"/> if you only want to know when an object first becomes visible.</para>
        /// </summary>
        public event EventHandler<ObjectObservedEventArgs> ObjectObserved;

        /// <summary>
        /// Triggered when the robot connects to the light cube.
        /// </summary>
        public event EventHandler<ObjectConnectedEventArgs> ObjectConnected;

        /// <summary>
        /// Triggered when the robot disconnects from the light cube
        /// </summary>
        public event EventHandler<ObjectDisconnectedEventArgs> ObjectDisconnected;

        /// <summary>
        /// Triggered when an object is moved (the light cube)
        /// </summary>
        public event EventHandler<ObjectMovingEventArgs> ObjectMoving;

        /// <summary>
        /// Triggered when the object has finished moving (the light cube)
        /// </summary>
        public event EventHandler<ObjectFinishedMovingEventArgs> ObjectFinishedMoving;

        /// <summary>
        /// Triggered when the light cube is tapped
        /// </summary>
        public event EventHandler<ObjectTappedEventArgs> ObjectTapped;

        /// <summary>
        /// Triggered when the light cube's up axis changed.
        /// </summary>
        public event EventHandler<ObjectUpAxisChangedEventArgs> ObjectUpAxisChanged;

        /// <summary>
        /// Triggered when a known face has appeared
        /// </summary>
        public event EventHandler<KnownFaceAppearedEventArgs> KnownFaceAppeared;

        /// <summary>
        /// Gets the all objects currently tracked in the world
        /// </summary>
        public ReadOnlyObservableCollection<ObservableObject> Objects
        {
            get
            {
                if (objectsReadOnly == null) objectsReadOnly = new ReadOnlyObservableCollection<ObservableObject>(objects);
                return objectsReadOnly;
            }
        }

        /// <summary>
        /// Gets the light cube if it has been seen by the robot, otherwise this value is null.
        /// </summary>
        public LightCube LightCube { get => _lightCube; private set => SetProperty(ref _lightCube, value); }
        private LightCube _lightCube;

        /// <summary>
        /// Gets the charger if it has been seen by the robot, otherwise this value is null.
        /// </summary>
        public Charger Charger { get => _charger; private set => SetProperty(ref _charger, value); }
        private Charger _charger;

        /// <summary>
        /// Gets a value indicating whether the is cube connected.
        /// </summary>
        public bool IsCubeConnected => LightCube?.IsConnected ?? false;

        /// <summary>
        /// Gets an object by object identifier.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>The observable object matching the ID or null</returns>
        public ObjectWithId GetObjectById(int objectId)
        {
            return objectsById[objectId];
        }

        /// <summary>
        /// Gets the face by face identifier.
        /// </summary>
        /// <param name="faceId">The face identifier.</param>
        /// <returns>The face matching the ID</returns>
        public Face GetFaceById(int faceId)
        {
            return facesById[faceId];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal WorldComponent(Robot robot) : base(robot)
        {
            robot.Events.ObservedObject += Events_ObservedObject;
            robot.Events.ObjectMoved += Events_ObjectMoved;
            robot.Events.ObjectStoppedMoving += Events_ObjectStoppedMoving;
            robot.Events.ObjectTapped += Events_ObjectTapped;
            robot.Events.ObjectUpAxisChanged += Events_ObjectUpAxisChanged;
            robot.Events.ObjectConnectionState += Events_ObjectConnectionState;
            robot.Events.ObservedFace += Events_ObservedFace;
            robot.Events.ChangedObservedFaceId += Events_ChangedObservedFaceId;
        }

        /// <summary>
        /// Attempt to connect to a cube.
        /// <para>If a cube is currently connected, this will do nothing.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> ConnectCube()
        {
            var response = await Robot.RunMethod(client => client.ConnectCubeAsync(new ExternalInterface.ConnectCubeRequest()));
            Robot.Events.OnCubeConnected(response);
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Requests a disconnection from the currently connected cube.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> DisconnectCube()
        {
            var response = await Robot.RunMethod(client => client.DisconnectCubeAsync(new ExternalInterface.DisconnectCubeRequest()));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Flashes the cube lights.
        /// <para>Plays the default cube connection animation on the currently connected cube's lights.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> FlashCubeLights()
        {
            var response = await Robot.RunMethod(client => client.FlashCubeLightsAsync(new ExternalInterface.FlashCubeLightsRequest()));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Forgets the preferred cube.
        /// <para>'Forget' the robot's preferred cube. This will cause the robot to connect to the cube with the highest RSSI(signal strength) next time a connection is requested.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> ForgetPreferredCube()
        {
            var response = await Robot.RunMethod(client => client.ForgetPreferredCubeAsync(new ExternalInterface.ForgetPreferredCubeRequest()));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Returns the Factory Ids of all available cubes.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the factory id's of available cubes.</returns>
        public async Task<IEnumerable<string>> CubesAvailable()
        {
            var response = await Robot.RunMethod(client => client.CubesAvailableAsync(new ExternalInterface.CubesAvailableRequest()));
            return response.FactoryIds;
        }

        /// <summary>
        /// Defines the custom object with the provided object definition.
        /// <para>The robot will now detect the markers associated with this object and raise an <see cref="ObjectObserved"/> event when they are seen.  There are 
        /// 20 custom object types that can be defined.</para>
        /// </summary>
        /// <param name="customObjectType">The object type you are binding this custom object definition to.</param>
        /// <param name="archetype">The custom object archetype (definition).</param>
        /// <param name="isUnique">If True, the robot will assume there is only 1 of this object. (and therefore only 1 of each of any of these markers) in the world.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="ArgumentOutOfRangeException">customObjectType - Custom object type cannot be 'None'</exception>
        /// <exception cref="ArgumentException">Specified Custom object definition is not supported - definition</exception>
        public async Task<StatusCode> DefineCustomObject(CustomObjectType customObjectType, CustomObjectArchetype archetype, bool isUnique = true)
        {
            if (customObjectType == CustomObjectType.None)
            {
                throw new ArgumentOutOfRangeException(nameof(customObjectType), "Custom object type cannot be 'None'");
            }
            var request = new ExternalInterface.DefineCustomObjectRequest()
            {
                CustomType = customObjectType.ToRobotType(),
                IsUnique = isUnique
            };
            if (archetype is CustomBoxArchetype) request.CustomBox = ((CustomBoxArchetype)archetype).ToRobotCustomBoxDefinition();
            else if (archetype is CustomCubeArchetype) request.CustomCube = ((CustomCubeArchetype)archetype).ToRobotCustomCubeDefinition();
            else if (archetype is CustomWallArchetype) request.CustomWall = ((CustomWallArchetype)archetype).ToRobotCustomWallDefinition();
            else throw new ArgumentException("Specified Custom object definition is not supported", nameof(archetype));
            var response = await Robot.RunMethod(client => client.DefineCustomObjectAsync(request));
            if (response.Success)
            {
                if (customObjectArchetypes.ContainsKey(customObjectType)) customObjectArchetypes[customObjectType].Unbind();
                archetype.Bind(customObjectType);
                customObjectArchetypes[customObjectType] = archetype;
            }
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Defines a cuboid of custom size and places it in the world. It cannot be observed.
        /// </summary>
        /// <param name="pose">The pose of the object to create.</param>
        /// <param name="xSizeMm">The size of the object (in millimeters) in the x axis.</param>
        /// <param name="ySizeMm">The size of the object (in millimeters) in the y axis.</param>
        /// <param name="zSizeMm">The size of the object (in millimeters) in the z axis.</param>
        /// <param name="relativeToRobot">Whether or not the pose given assumes the robot's pose as its origin.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> CreateFixedCustomObject(Pose pose, float xSizeMm, float ySizeMm, float zSizeMm, bool relativeToRobot = false)
        {
            if (relativeToRobot) pose = Robot.Pose.RelativeToThis(pose);
            else pose = Robot.Pose.LocalizeToThis(pose);

            var response = await Robot.RunMethod(client => client.CreateFixedCustomObjectAsync(new ExternalInterface.CreateFixedCustomObjectRequest()
            {
                Pose = pose.ToPoseStruct(),
                XSizeMm = xSizeMm,
                YSizeMm = ySizeMm,
                ZSizeMm = zSizeMm
            }));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Causes the robot to forget all the custom defined object archetypes.
        /// <para>The custom objects need to be deleted from Vector before calling this method to ensure none of the archetypes are in use.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="InvalidOperationException">Cannot delete archetypes because custom objects are in use.  Delete all custom objects first.</exception>
        public async Task<StatusCode> DeleteCustomObjectArchetypes()
        {
            if (objects.Any(o => o is CustomObject)) throw new InvalidOperationException("Cannot delete archetypes because custom objects are in use.  Delete all custom objects first.");
            customObjectArchetypes.Clear();
            var response = await Robot.RunMethod(client => client.DeleteCustomObjectsAsync(new ExternalInterface.DeleteCustomObjectsRequest()
            {
                Mode = ExternalInterface.CustomObjectDeletionMode.DeletionMaskArchetypes
            }));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Causes the robot to forget about all the custom objects it currently knows about.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> DeleteCustomObjects()
        {
            foreach (var obj in objects.OfType<CustomObject>().ToArray())
            {
                objects.Remove(obj);
                objectsById.Remove(obj.ObjectId);
            }
            var response = await Robot.RunMethod(client => client.DeleteCustomObjectsAsync(new ExternalInterface.DeleteCustomObjectsRequest()
            {
                Mode = ExternalInterface.CustomObjectDeletionMode.DeletionMaskCustomMarkerObjects
            }));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Causes the robot to forget about all the fixed custom objects it currently knows about.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> DeleteFixedCustomObjects()
        {
            var response = await Robot.RunMethod(client => client.DeleteCustomObjectsAsync(new ExternalInterface.DeleteCustomObjectsRequest()
            {
                Mode = ExternalInterface.CustomObjectDeletionMode.DeletionMaskFixedCustomObjects
            }));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignores not connected exceptions")]
        internal override async Task Teardown()
        {
            try
            {
                await DeleteCustomObjects().ConfigureAwait(false);
                await DeleteCustomObjectArchetypes().ConfigureAwait(false);
                await DeleteFixedCustomObjects().ConfigureAwait(false);
            }
            catch (VectorNotConnectedException)
            {
                // Ignore
            }
            facesById.Clear();
            objectsById.Clear();
            objects.Clear();
        }

        /// <summary>
        /// Handles the ObservedFace event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotObservedFaceEventArgs"/> instance containing the event data.</param>
        private void Events_ObservedFace(object sender, RobotObservedFaceEventArgs e)
        {
            bool newObject = false;
            Face face = facesById.ContainsKey(e.FaceId) ? facesById[e.FaceId] : null;
            if (face == null)
            {
                face = new Face(e.FaceId, this.Robot);
                facesById.Add(face.FaceId, face);
                newObject = true;
            }
            bool appeared = !face.IsVisible;
            face.OnFaceObserved(e);
            face.DispatchDisappearEvent(OnObjectDisappeared, ObjectVisibilityTimeout).ConfigureAwait(false);
            if (newObject)
            {
                objects.Add(face);
                RaiseObjectEvents(ObjectAdded, new ObjectAddedEventArgs(face));
            }
            if (appeared)
            {
                RaiseObjectEvents(ObjectAppeared, new ObjectAppearedEventArgs(face, e));
                if (!string.IsNullOrWhiteSpace(face.Name)) RaiseObjectEvents(KnownFaceAppeared, new KnownFaceAppearedEventArgs(face));
            }
            RaiseObjectEvents(ObjectObserved, new ObjectObservedEventArgs(face, e));
        }

        /// <summary>
        /// Handles the RobotObservedObject event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotObservedObjectEventArgs" /> instance containing the event data.</param>
        private void Events_ObservedObject(object sender, RobotObservedObjectEventArgs e)
        {
            bool newObject = false;
            ObjectWithId obj = objectsById.ContainsKey(e.ObjectId) ? objectsById[e.ObjectId] : null;
            if (obj == null)
            {
                obj = ObjectFactory(e.ObjectType, e.CustomObjectType, e.ObjectId);
                objectsById.Add(obj.ObjectId, obj);
                if (obj is LightCube) LightCube = (LightCube)obj;
                if (obj is Charger) Charger = (Charger)obj;
                newObject = true;
            }
            bool appeared = !obj.IsVisible;
            obj.OnObjectObserved(e);
            obj.DispatchDisappearEvent(OnObjectDisappeared, ObjectVisibilityTimeout).ConfigureAwait(false);
            if (newObject)
            {
                objects.Add(obj);
                RaiseObjectEvents(ObjectAdded, new ObjectAddedEventArgs(obj));
            }
            if (appeared) RaiseObjectEvents(ObjectAppeared, new ObjectAppearedEventArgs(obj, e));
            RaiseObjectEvents(ObjectObserved, new ObjectObservedEventArgs(obj, e));
        }

        /// <summary>
        /// Called when object has disappeared.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnObjectDisappeared(ObservableObject obj)
        {
            RaiseObjectEvents(ObjectDisappeared, new ObjectDisappearedEventArgs(obj));
        }

        /// <summary>
        /// Object factory
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <param name="customObjectType">Type of the custom object, if applicable.</param>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>New observable object of the type related to <see cref="ObjectType" /></returns>
        private ObjectWithId ObjectFactory(ObjectType objectType, CustomObjectType customObjectType, int objectId)
        {
            switch (objectType)
            {
                case ObjectType.BlockLightcube1: return new LightCube(objectId, this.Robot);
                case ObjectType.ChargerBasic: return new Charger(objectId, this.Robot);
                case ObjectType.CustomObject: return new CustomObject(objectId, this.Robot, customObjectArchetypes[customObjectType]);
                default: throw new NotSupportedException($"Unknown object type {objectType}");
            }
        }

        /// <summary>
        /// Handles the ObjectConnectionState event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotObjectConnectionStateEventArgs"/> instance containing the event data.</param>
        private void Events_ObjectConnectionState(object sender, RobotObjectConnectionStateEventArgs e)
        {
            InitLightCubeEvent(e.ObjectId);
            LightCube.OnObjectConnectionState(e);
            OnPropertyChanged(nameof(IsCubeConnected));
            if (e.Connected) RaiseObjectEvents(ObjectConnected, new ObjectConnectedEventArgs(LightCube));
            else RaiseObjectEvents(ObjectDisconnected, new ObjectDisconnectedEventArgs(LightCube));
        }

        /// <summary>
        /// Handles the ObjectMoved event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotObjectMovedEventArgs"/> instance containing the event data.</param>
        private void Events_ObjectMoved(object sender, RobotObjectMovedEventArgs e)
        {
            InitLightCubeEvent(e.ObjectId);
            LightCube.OnObjectMoved(e);
            RaiseObjectEvents(ObjectMoving, new ObjectMovingEventArgs(LightCube, e));
        }

        /// <summary>
        /// Handles the ObjectStoppedMoving event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotObjectStoppedMovingEventArgs"/> instance containing the event data.</param>
        private void Events_ObjectStoppedMoving(object sender, RobotObjectStoppedMovingEventArgs e)
        {
            InitLightCubeEvent(e.ObjectId);
            var moveDuration = LightCube.OnObjectStoppedMoving();
            if (moveDuration == TimeSpan.Zero) return;
            RaiseObjectEvents(ObjectFinishedMoving, new ObjectFinishedMovingEventArgs(LightCube, e, moveDuration));
        }

        /// <summary>
        /// Handles the ObjectUpAxisChanged event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotObjectUpAxisChangedEventArgs"/> instance containing the event data.</param>
        private void Events_ObjectUpAxisChanged(object sender, RobotObjectUpAxisChangedEventArgs e)
        {
            InitLightCubeEvent(e.ObjectId);
            LightCube.OnObjectUpAxisChanged(e);
            RaiseObjectEvents(ObjectUpAxisChanged, new ObjectUpAxisChangedEventArgs(LightCube, e));
        }

        /// <summary>
        /// Handles the ObjectTapped event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotObjectTappedEventArgs"/> instance containing the event data.</param>
        private void Events_ObjectTapped(object sender, RobotObjectTappedEventArgs e)
        {
            InitLightCubeEvent(e.ObjectId);
            LightCube.OnObjectTapped(e);
            RaiseObjectEvents(ObjectTapped, new ObjectTappedEventArgs(LightCube, e));
        }

        /// <summary>
        /// Handles the ChangedObservedFaceId event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotChangedObservedFaceIdEventArgs"/> instance containing the event data.</param>
        private void Events_ChangedObservedFaceId(object sender, RobotChangedObservedFaceIdEventArgs e)
        {
            if (!facesById.ContainsKey(e.OldId)) return;
            if (facesById.ContainsKey(e.NewId)) return;
            Face face = facesById[e.OldId];
            facesById.Add(e.NewId, face);
            face.OnChangeObservedFaceId(e);
        }

        /// <summary>
        /// If a light cube event is triggered, this creates the light cube object and adds it to the world
        /// if it doesn't already exist.
        /// </summary>
        /// <param name="lightCubeObjectId">The light cube object identifier.</param>
        private void InitLightCubeEvent(int lightCubeObjectId)
        {
            if (LightCube != null) return;
            LightCube = new LightCube(lightCubeObjectId, this.Robot);
            objectsById.Add(lightCubeObjectId, LightCube);
            objects.Add(LightCube);
        }

        /// <summary>
        /// Raises the <see cref="ObjectEvent"/> event and then the specified event handler
        /// </summary>
        /// <typeparam name="T">Event args type</typeparam>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void RaiseObjectEvents<T>(EventHandler<T> eventHandler, T eventArgs) where T : ObjectEventArgs
        {
            ObjectEvent?.Raise(this, eventArgs);
            eventHandler?.Raise(this, eventArgs);
        }
    }
}
