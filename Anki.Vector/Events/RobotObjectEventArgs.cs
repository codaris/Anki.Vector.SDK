// <copyright file="RobotObjectEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object event type
    /// </summary>
    public enum ObjectEventType
    {
        /// <summary>No event</summary>
        None = 0,
        /// <summary>Object available event</summary>
        ObjectAvailable = 1,
        /// <summary>Object connection state event</summary>
        ObjectConnectionState = 2,
        /// <summary>Object moved event</summary>
        ObjectMoved = 3,
        /// <summary>Object stopped moving event</summary>
        ObjectStoppedMoving = 4,
        /// <summary>Object up axis changed event</summary>
        ObjectUpAxisChanged = 5,
        /// <summary>Object tapped</summary>
        ObjectTapped = 6,
        /// <summary>Object observed</summary>
        RobotObservedObject = 7,
        /// <summary>Cube connection lost</summary>
        CubeConnectionLost = 8
    }

    /// <summary>
    /// Object event args
    /// </summary>
    /// <seealso cref="RobotEventArgs" />
    public abstract class RobotObjectEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the object event type.
        /// </summary>
        public ObjectEventType ObjectEventType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObjectEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObjectEventArgs(ExternalInterface.Event e) : base(e)
        {
            ObjectEventType = (ObjectEventType)e.ObjectEvent.ObjectEventTypeCase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObjectEventArgs"/> class.
        /// </summary>
        /// <param name="objectEventType">Type of the object event.</param>
        internal RobotObjectEventArgs(ObjectEventType objectEventType) : base(EventType.ObjectEvent)
        {
            ObjectEventType = objectEventType;
        }
    }
}
