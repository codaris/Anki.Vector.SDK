// <copyright file="LightCube.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Objects
{
    using System;
    using System.Threading.Tasks;
    using Anki.Vector.Events;
    using Anki.Vector.Types;

    /// <summary>
    /// Represents Vector’s Cube.
    /// <para>The LightCube object has four LEDs that Vector can actively manipulate and communicate with.</para>
    /// <para>As Vector drives around, he uses the position of objects that he recognizes, including his cube, to localize himself, 
    /// taking note of the <see cref="Pose"/> of the objects.</para>
    /// <para>You can subscribe to cube events including <see cref="WorldComponent.ObjectTapped"/>, <see cref="WorldComponent.ObjectAppeared"/>, 
    /// and <see cref="WorldComponent.ObjectDisappeared"/>.</para>
    /// <para>Vector supports 1 LightCube.</para>
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.ObservableObject" />
    public class LightCube : ObjectWithId
    {
        /// <summary>
        /// Gets the unique hardware id of the physical cube.
        /// </summary>
        public string FactoryId { get => _factoryId; private set => SetProperty(ref _factoryId, value); }
        private string _factoryId;

        /// <summary>
        /// Gets a value indicating whether the cube is currently connected to the robot.
        /// </summary>
        public bool IsConnected { get => _isConnected; private set => SetProperty(ref _isConnected, value); }
        private bool _isConnected;

        /// <summary>
        /// Gets a value indicating whether the cube’s accelerometer indicates that the cube is moving.
        /// </summary>
        public bool IsMoving { get => _isMoving; private set => SetProperty(ref _isMoving, value); }
        private bool _isMoving;

        /// <summary>
        /// Gets the time the object was last moved.
        /// </summary>
        public DateTime LastMovedTime { get => _lastMovedTime; private set => SetProperty(ref _lastMovedTime, value); }
        private DateTime _lastMovedTime;

        /// <summary>
        /// Gets the last moved robot timestamp.
        /// </summary>
        public uint LastMovedTimestamp { get => _lastMovedTimestamp; private set => SetProperty(ref _lastMovedTimestamp, value); }
        private uint _lastMovedTimestamp;

        /// <summary>
        /// Gets the time the object most recently started moving 
        /// </summary>
        public DateTime LastMovedStartTime { get => _lastMovedStartTime; private set => SetProperty(ref _lastMovedStartTime, value); }
        private DateTime _lastMovedStartTime;

        /// <summary>
        /// Gets the time the object most recently started moving in robot time.
        /// </summary>
        public uint LastMovedStartTimestamp { get => _lastMovedStartTimestamp; private set => SetProperty(ref _lastMovedStartTimestamp, value); }
        private uint _lastMovedStartTimestamp;

        /// <summary>
        /// Gets the object’s up_axis value from the last time it changed.
        /// </summary>
        public UpAxis UpAxis { get => _upAxis; private set => SetProperty(ref _upAxis, value); }
        private UpAxis _upAxis;

        /// <summary>
        /// Gets the time the object’s orientation last changed
        /// </summary>
        public DateTime LastUpAxisTime { get => _lastUpAxisTime; private set => SetProperty(ref _lastUpAxisTime, value); }
        private DateTime _lastUpAxisTime;

        /// <summary>
        /// Gets the robot timestamp when the object’s orientation last changed
        /// </summary>
        public uint LastUpAxisTimestamp { get => _lastUpAxisTimestamp; private set => SetProperty(ref _lastUpAxisTimestamp, value); }
        private uint _lastUpAxisTimestamp;

        /// <summary>
        /// Gets the time the object was last tapped 
        /// </summary>
        public DateTime LastTappedTime { get => _lastTappedTime; private set => SetProperty(ref _lastTappedTime, value); }
        private DateTime _lastTappedTime;

        /// <summary>
        /// Gets the robot timestamp when the object was last tapped
        /// </summary>
        public uint LastTappedTimestamp { get => _lastTappedTimestamp; private set => SetProperty(ref _lastTappedTimestamp, value); }
        private uint _lastTappedTimestamp;

        /// <summary>
        /// Gets the angular distance from the current reported up axis.
        /// </summary>
        public float TopFaceOrientationRad { get => _topFaceOrientationRad; private set => SetProperty(ref _topFaceOrientationRad, value); }
        private float _topFaceOrientationRad;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightCube" /> class.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="robot">The robot.</param>
        internal LightCube(int objectId, Robot robot) : base(objectId, robot)
        {
        }

        /// <summary>
        /// Set the light for all corners.
        /// </summary>
        /// <param name="light">The settings for all the lights.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the function.</returns>
        public Task<StatusCode> SetLightCorners(Light light)
        {
            return SetLightCorners(light, light, light, light);
        }

        /// <summary>
        /// Set the light for each corner.
        /// </summary>
        /// <param name="light1">The settings for the first light.</param>
        /// <param name="light2">The settings for the second light.</param>
        /// <param name="light3">The settings for the third light.</param>
        /// <param name="light4">The settings for the fourth light.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the function.</returns>
        public Task<StatusCode> SetLightCorners(Light light1, Light light2, Light light3, Light light4)
        {
            return SetLightCorners(light1, light2, light3, light4, ColorProfile.WhiteBalancedCubeProfile);
        }

        /// <summary>
        /// Set the light for each corner.
        /// </summary>
        /// <param name="light1">The settings for the first light.</param>
        /// <param name="light2">The settings for the second light.</param>
        /// <param name="light3">The settings for the third light.</param>
        /// <param name="light4">The settings for the fourth light.</param>
        /// <param name="colorProfile">The color profile for the cube lights.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the function.</returns>
        public async Task<StatusCode> SetLightCorners(Light light1, Light light2, Light light3, Light light4, ColorProfile colorProfile)
        {
            var request = new ExternalInterface.SetCubeLightsRequest
            {
                ObjectId = (uint)ObjectId,
                RelativeToX = 0f,
                RelativeToY = 0f,
                Rotate = false,
                MakeRelative = ExternalInterface.SetCubeLightsRequest.Types.MakeRelativeMode.Off
            };

            light1.AddToRequest(request, colorProfile);
            light2.AddToRequest(request, colorProfile);
            light3.AddToRequest(request, colorProfile);
            light4.AddToRequest(request, colorProfile);

            var response = await Robot.RunMethod(client => client.SetCubeLightsAsync(request));
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Set all lights off on the cube
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the function.</returns>
        public Task<StatusCode> SetLightsOff()
        {
            return SetLightCorners(new Light());
        }

        /// <summary>
        /// Process the <see cref="E:ObjectObserved" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RobotObservedObjectEventArgs"/> instance containing the event data.</param>
        internal override void OnObjectObserved(RobotObservedObjectEventArgs e)
        {
            base.OnObjectObserved(e);
            TopFaceOrientationRad = e.TopFaceOrientationRad;
        }

        /// <summary>
        /// Process the <see cref="E:ObjectConnectionState" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RobotObjectConnectionStateEventArgs"/> instance containing the event data.</param>
        internal void OnObjectConnectionState(RobotObjectConnectionStateEventArgs e)
        {
            FactoryId = e.FactoryId;
            IsConnected = e.Connected;
        }
        
        /// <summary>
        /// Process the <see cref="E:ObjectMoved" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RobotObjectMovedEventArgs"/> instance containing the event data.</param>
        internal void OnObjectMoved(RobotObjectMovedEventArgs e)
        {
            var now = DateTime.Now;
            bool startedMoving = !IsMoving;
            LastEventTime = now;
            IsMoving = true;
            LastMovedTime = now;
            LastMovedTimestamp = e.RobotTimestamp;
            if (startedMoving)
            {
                LastMovedStartTime = now;
                LastMovedStartTimestamp = e.RobotTimestamp;
            }
        }

        /// <summary>
        /// Process the <see cref="E:ObjectStoppedMoving" /> event.
        /// </summary>
        /// <returns>The move duration</returns>
        internal TimeSpan OnObjectStoppedMoving()
        {
            var now = DateTime.Now;
            LastEventTime = now;
            if (!IsMoving) return TimeSpan.Zero;
            IsMoving = false;
            return now - LastMovedStartTime;
        }

        /// <summary>
        /// Processes the <see cref="E:ObjectUpAxisChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RobotObjectUpAxisChangedEventArgs"/> instance containing the event data.</param>
        internal void OnObjectUpAxisChanged(RobotObjectUpAxisChangedEventArgs e)
        {
            var now = DateTime.Now;
            LastEventTime = now;
            UpAxis = e.UpAxis;
            LastUpAxisTime = now;
            LastUpAxisTimestamp = e.RobotTimestamp;
        }

        /// <summary>
        /// Processes the <see cref="E:ObjectTapped" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RobotObjectTappedEventArgs"/> instance containing the event data.</param>
        internal void OnObjectTapped(RobotObjectTappedEventArgs e)
        {
            var now = DateTime.Now;
            LastEventTime = now;
            LastTappedTime = now;
            LastTappedTimestamp = e.RobotTimestamp;
        }

        /// <summary>
        /// Gets the name of the object type.
        /// </summary>
        public override string ObjectTypeName => "LightCube";
    }
}

/*
 * TODO
 * 
 * Figure out if other lightcube light request parameters do anything
 * 
 */
