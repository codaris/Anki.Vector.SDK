// <copyright file="RobotObjectTappedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object tapped event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotObjectTappedEventArgs : RobotObjectEventArgs
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
        /// Initializes a new instance of the <see cref="RobotObjectTappedEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObjectTappedEventArgs(ExternalInterface.Event e) : base(e)
        {
            var objectTapped = e.ObjectEvent.ObjectTapped;
            ObjectId = (int)objectTapped.ObjectId;
            RobotTimestamp = objectTapped.Timestamp;
        }
    }
}
