// <copyright file="EventComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Anki.Vector.Events;
using Anki.Vector.ExternalInterface;
using Anki.Vector.GrpcUtil;

namespace Anki.Vector
{
    /// <summary>
    /// Event handlers to subscribe to robot events.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class EventComponent : Component
    {
        /// <summary>
        /// The event feed loop
        /// </summary>
        private readonly IAsyncEventLoop eventFeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal EventComponent(Robot robot) : base(robot)
        {
            this.eventFeed = new AsyncEventLoop<EventResponse>(
                (token) => robot.StartStream(client => client.EventStream(new EventRequest(), cancellationToken: token)),
                ProcessEvent,
                () =>
                {
                    OnPropertyChanged(nameof(IsProccessingEvents));
                    Robot.Disconnect().ConfigureAwait(false);
                },
                robot.PropagateException
            );
        }

        /// <summary>
        /// Gets a value indicating whether this instance is processing events.
        /// </summary>
        public bool IsProccessingEvents => eventFeed.IsActive;

        /// <summary>
        /// Starts the event processing
        /// </summary>
        /// <exception cref="InvalidOperationException">The camera feed has already been started.</exception>
        internal void Start()
        {
            eventFeed.Start().ConfigureAwait(false);
            OnPropertyChanged(nameof(IsProccessingEvents));
        }

        /// <summary>
        /// Ends the event processing
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        internal async Task End()
        {
            await eventFeed.End().ConfigureAwait(false);
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        internal override Task Teardown()
        {
            return End();
        }

        /// <summary>
        /// Occurs for every robot event
        /// </summary>
        public event EventHandler<RobotEventArgs> RobotEvent;

        /// <summary>
        /// Occurs when connection response received.
        /// </summary>
        public event EventHandler<ConnectionResponseEventArgs> ConnectionResponse;

        /// <summary>
        /// Occurs when cube batter status received.
        /// </summary>
        public event EventHandler<CubeBatteryEventArgs> CubeBattery;

        /// <summary>
        /// Robot event triggered when Vector hears "Hey Vector"
        /// </summary>
        public event EventHandler<WakeWordBeginEventArgs> WakeWordBegin;

        /// <summary>
        /// Robot event triggered after Vector finishes listening to "Hey Vector"
        /// </summary>
        public event EventHandler<WakeWordEndEventArgs> WakeWordEnd;

        /// <summary>
        /// Keep alive event from robot
        /// </summary>
        public event EventHandler<KeepAliveEventArgs> KeepAlive;

        /// <summary>
        /// Occurs when mirror mode is disabled.
        /// </summary>
        public event EventHandler<MirrorModeDisabledEventArgs> MirrorModeDisabled;

        /// <summary>
        /// Occurs when the cube connection is lost.
        /// </summary>
        public event EventHandler<CubeConnectionLostEventArgs> CubeConnectionLost;

        /// <summary>
        /// After the ConnectCube process is started, all available light cubes in range will broadcast an availability message through the Robot.
        /// </summary>
        public event EventHandler<RobotObjectAvailableEventArgs> ObjectAvailable;

        /// <summary>
        /// Connection state change of the cube
        /// </summary>
        public event EventHandler<RobotObjectConnectionStateEventArgs> ObjectConnectionState;

        /// <summary>
        /// Robot event triggered when an object starts moving.
        /// </summary>
        public event EventHandler<RobotObjectMovedEventArgs> ObjectMoved;

        /// <summary>
        /// Robot event triggered when an object stops moving.
        /// </summary>
        public event EventHandler<RobotObjectStoppedMovingEventArgs> ObjectStoppedMoving;

        /// <summary>
        /// Robot event triggered when an object is tapped.
        /// </summary>
        public event EventHandler<RobotObjectTappedEventArgs> ObjectTapped;

        /// <summary>
        /// Robot event triggered when an object’s orientation changed.
        /// </summary>
        public event EventHandler<RobotObjectUpAxisChangedEventArgs> ObjectUpAxisChanged;

        /// <summary>
        /// Robot event triggered when an object is observed by the robot.
        /// </summary>
        public event EventHandler<RobotObservedObjectEventArgs> ObservedObject;

        /// <summary>
        /// Robot event when photo is taken
        /// </summary>
        public event EventHandler<PhotoTakenEventArgs> PhotoTaken;

        /// <summary>
        /// Robot event for when observed face changes
        /// </summary>
        public event EventHandler<RobotChangedObservedFaceIdEventArgs> ChangedObservedFaceId;

        /// <summary>
        /// Robot event for when a face is observed by the robot.
        /// </summary>
        public event EventHandler<RobotObservedFaceEventArgs> ObservedFace;

        /// <summary>
        /// Robot event containing changes to the robot’s state.
        /// </summary>
        public event EventHandler<RobotStateEventArgs> RobotState;

        /// <summary>
        /// Robot event containing stimulation information.
        /// </summary>
        public event EventHandler<StimulationInfoEventArgs> StimulationInfo;

        /// <summary>
        /// Robot event when face enrollment completed.
        /// </summary>
        public event EventHandler<FaceEnrollmentCompletedEventArgs> FaceEnrollmentCompleted;

        /// <summary>
        /// Robot event when face scan completed.
        /// </summary>
        public event EventHandler<FaceScanCompleteEventArgs> FaceScanComplete;

        /// <summary>
        /// Robot event when face scan started.
        /// </summary>
        public event EventHandler<FaceScanStartedEventArgs> FaceScanStarted;

        /// <summary>
        /// Robot event vision modes are automatically disabled.
        /// </summary>
        public event EventHandler<VisionModesAutoDisabledEventArgs> VisionModesAutoDisabled;

        /// <summary>
        /// Root event triggered after processing voice commands
        /// </summary>
        public event EventHandler<UserIntentEventArgs> UserIntent;

        /// <summary>
        /// Called when cube is explicitly connected.
        /// </summary>
        /// <param name="response">The response.</param>
        internal void OnCubeConnected(ExternalInterface.ConnectCubeResponse response)
        {
            RaiseRobotEvents(ObjectConnectionState, new RobotObjectConnectionStateEventArgs(response));
        }

        /// <summary>
        /// Processes the event.  This is called directly from event feed loop
        /// </summary>
        /// <param name="eventResponse">The event response.</param>
        private void ProcessEvent(EventResponse eventResponse)
        {
            var e = eventResponse.Event;
            switch (e.EventTypeCase)
            {
                case Event.EventTypeOneofCase.ConnectionResponse:
                    RaiseRobotEvents(ConnectionResponse, new ConnectionResponseEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.CubeBattery:
                    RaiseRobotEvents(CubeBattery, new CubeBatteryEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.WakeWord:
                    ProcessWakeWord(e);
                    break;
                case Event.EventTypeOneofCase.KeepAlive:
                    RaiseRobotEvents(KeepAlive, new KeepAliveEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.MirrorModeDisabled:
                    RaiseRobotEvents(MirrorModeDisabled, new MirrorModeDisabledEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.ObjectEvent:
                    ProcessObjectEvent(e);
                    break;
                case Event.EventTypeOneofCase.PhotoTaken:
                    RaiseRobotEvents(PhotoTaken, new PhotoTakenEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.RobotChangedObservedFaceId:
                    RaiseRobotEvents(ChangedObservedFaceId, new RobotChangedObservedFaceIdEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.RobotObservedFace:
                    RaiseRobotEvents(ObservedFace, new RobotObservedFaceEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.RobotState:
                    RaiseRobotEvents(RobotState, new RobotStateEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.StimulationInfo:
                    RaiseRobotEvents(StimulationInfo, new StimulationInfoEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.TimeStampedStatus:
                    ProcessTimestampedStatus(e);
                    break;
                case Event.EventTypeOneofCase.VisionModesAutoDisabled:
                    RaiseRobotEvents(VisionModesAutoDisabled, new VisionModesAutoDisabledEventArgs(e));
                    break;
                case Event.EventTypeOneofCase.UserIntent:
                    RaiseRobotEvents(UserIntent, new UserIntentEventArgs(e));
                    break;
            }
        }

        /// <summary>
        /// Processes the wake word event 
        /// </summary>
        /// <param name="e">The wake word data.</param>
        private void ProcessWakeWord(Event e)
        {
            switch (e.WakeWord.WakeWordTypeCase)
            {
                case WakeWord.WakeWordTypeOneofCase.WakeWordBegin:
                    RaiseRobotEvents(WakeWordBegin, new WakeWordBeginEventArgs(e));
                    break;
                case WakeWord.WakeWordTypeOneofCase.WakeWordEnd:
                    RaiseRobotEvents(WakeWordEnd, new WakeWordEndEventArgs(e));
                    break;
            }
        }

        /// <summary>
        /// Processes the object event.
        /// </summary>
        /// <param name="e">The object event.</param>
        private void ProcessObjectEvent(Event e)
        {
            switch (e.ObjectEvent.ObjectEventTypeCase)
            {
                case ObjectEvent.ObjectEventTypeOneofCase.CubeConnectionLost:
                    RaiseRobotEvents(CubeConnectionLost, new CubeConnectionLostEventArgs(e));
                    break;
                case ObjectEvent.ObjectEventTypeOneofCase.ObjectAvailable:
                    RaiseRobotEvents(ObjectAvailable, new RobotObjectAvailableEventArgs(e));
                    break;
                case ObjectEvent.ObjectEventTypeOneofCase.ObjectConnectionState:
                    RaiseRobotEvents(ObjectConnectionState, new RobotObjectConnectionStateEventArgs(e));
                    break;
                case ObjectEvent.ObjectEventTypeOneofCase.ObjectMoved:
                    RaiseRobotEvents(ObjectMoved, new RobotObjectMovedEventArgs(e));
                    break;
                case ObjectEvent.ObjectEventTypeOneofCase.ObjectStoppedMoving:
                    RaiseRobotEvents(ObjectStoppedMoving, new RobotObjectStoppedMovingEventArgs(e));
                    break;
                case ObjectEvent.ObjectEventTypeOneofCase.ObjectTapped:
                    RaiseRobotEvents(ObjectTapped, new RobotObjectTappedEventArgs(e));
                    break;
                case ObjectEvent.ObjectEventTypeOneofCase.ObjectUpAxisChanged:
                    RaiseRobotEvents(ObjectUpAxisChanged, new RobotObjectUpAxisChangedEventArgs(e));
                    break;
                case ObjectEvent.ObjectEventTypeOneofCase.RobotObservedObject:
                    RaiseRobotEvents(ObservedObject, new RobotObservedObjectEventArgs(e));
                    break;
            }
        }

        /// <summary>
        /// Processes the timestamped status events.
        /// </summary>
        /// <param name="e">The event details.</param>
        private void ProcessTimestampedStatus(Event e)
        {
            switch (e.TimeStampedStatus.Status.StatusTypeCase)
            {
                case Status.StatusTypeOneofCase.FaceEnrollmentCompleted:
                    RaiseRobotEvents(FaceEnrollmentCompleted, new FaceEnrollmentCompletedEventArgs(e));
                    break;
                case Status.StatusTypeOneofCase.MeetVictorFaceScanComplete:
                    RaiseRobotEvents(FaceScanComplete, new FaceScanCompleteEventArgs(e));
                    break;
                case Status.StatusTypeOneofCase.MeetVictorFaceScanStarted:
                    RaiseRobotEvents(FaceScanStarted, new FaceScanStartedEventArgs(e));
                    break;
            }
        }

        /// <summary>
        /// Raises the <see cref="RobotEvent"/> event and then the specified event handler
        /// </summary>
        /// <typeparam name="T">Event args type</typeparam>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void RaiseRobotEvents<T>(EventHandler<T> eventHandler, T eventArgs) where T : RobotEventArgs
        {
            RobotEvent?.Raise(this, eventArgs);
            eventHandler?.Raise(this, eventArgs);
        }
    }
}
