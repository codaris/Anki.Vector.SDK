// <copyright file="ObjectFinishedMovingEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object stopped moving event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectFinishedMovingEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Gets the duration of the move.
        /// </summary>
        public TimeSpan MoveDuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFinishedMovingEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="RobotObjectStoppedMovingEventArgs" /> instance containing the event data.</param>
        /// <param name="moveDuration">The move duration.</param>
        internal ObjectFinishedMovingEventArgs(ObservableObject obj, RobotObjectStoppedMovingEventArgs e, TimeSpan moveDuration) : base(obj)
        {
            RobotTimestamp = e.RobotTimestamp;
            MoveDuration = moveDuration;
        }
    }
}
