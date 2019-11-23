// <copyright file="ObjectMovingEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object moved event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectMovingEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMovingEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="RobotObjectMovedEventArgs" /> instance containing the event data.</param>
        internal ObjectMovingEventArgs(ObservableObject obj, RobotObjectMovedEventArgs e) : base(obj)
        {
            RobotTimestamp = e.RobotTimestamp;
        }
    }
}
