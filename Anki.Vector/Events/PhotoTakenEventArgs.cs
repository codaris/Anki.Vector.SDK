// <copyright file="PhotoTakenEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Photo taken event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class PhotoTakenEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the photo identifier of the photo that was taken
        /// </summary>
        public uint PhotoId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoTakenEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal PhotoTakenEventArgs(ExternalInterface.Event e) : base(e)
        {
            PhotoId = e.PhotoTaken.PhotoId;
        }
    }
}
