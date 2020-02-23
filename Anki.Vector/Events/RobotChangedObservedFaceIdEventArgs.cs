// <copyright file="RobotChangedObservedFaceIdEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Robot changed observed face id event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotChangedObservedFaceIdEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the new face identifier.
        /// </summary>
        public int NewId { get; }

        /// <summary>
        /// Gets the old face identifier.
        /// </summary>
        public int OldId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotChangedObservedFaceIdEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotChangedObservedFaceIdEventArgs(ExternalInterface.Event e) : base(e)
        {
            NewId = e.RobotChangedObservedFaceId.NewId;
            OldId = e.RobotChangedObservedFaceId.OldId;
        }
    }
}
