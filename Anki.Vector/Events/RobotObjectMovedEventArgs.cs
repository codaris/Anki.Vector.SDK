// <copyright file="RobotObjectMovedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object moved event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotObjectMovedEventArgs : RobotObjectEventArgs
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
        /// Initializes a new instance of the <see cref="RobotObjectMovedEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObjectMovedEventArgs(ExternalInterface.Event e) : base(e)
        {
            var objectMoved = e.ObjectEvent.ObjectMoved;
            ObjectId = (int)objectMoved.ObjectId;
            RobotTimestamp = objectMoved.Timestamp;
        }
    }
}
