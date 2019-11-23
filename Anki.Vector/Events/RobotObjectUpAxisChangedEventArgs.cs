// <copyright file="RobotObjectUpAxisChangedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object up axis changed event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotObjectUpAxisChangedEventArgs : RobotObjectEventArgs
    {
        /// <summary>
        /// Gets the object identifier.
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Gets the up axis.
        /// </summary>
        public UpAxis UpAxis { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObjectUpAxisChangedEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObjectUpAxisChangedEventArgs(ExternalInterface.Event e) : base(e)
        {
            var objectUpAxisChanged = e.ObjectEvent.ObjectUpAxisChanged;
            ObjectId = (int)objectUpAxisChanged.ObjectId;
            RobotTimestamp = objectUpAxisChanged.Timestamp;
            UpAxis = (UpAxis)(int)objectUpAxisChanged.UpAxis;
        }
    }
}
