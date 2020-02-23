// <copyright file="MirrorModeDisabledEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Mirror mode disabled event args
    /// </summary>
    /// <seealso cref="RobotEventArgs" />
    public class MirrorModeDisabledEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MirrorModeDisabledEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal MirrorModeDisabledEventArgs(ExternalInterface.Event e) : base(e)
        {
        }
    }
}
