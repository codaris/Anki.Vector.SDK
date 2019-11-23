// <copyright file="RobotObjectStoppedMovingEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object stopped moving event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotObjectStoppedMovingEventArgs : RobotObjectEventArgs
    {
        /// <summary>
        /// Gets the object identifier.
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        /// Gets the robot timestamp of the event.
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObjectStoppedMovingEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObjectStoppedMovingEventArgs(ExternalInterface.Event e) : base(e)
        {
            var objectStoppedMoving = e.ObjectEvent.ObjectStoppedMoving;
            ObjectId = (int)objectStoppedMoving.ObjectId;
            RobotTimestamp = objectStoppedMoving.Timestamp;
        }
    }
}
