// <copyright file="VisionModesAutoDisabledEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Vision modes auto disabled event args
    /// </summary>
    /// <seealso cref="RobotEventArgs" />
    public class VisionModesAutoDisabledEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisionModesAutoDisabledEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal VisionModesAutoDisabledEventArgs(ExternalInterface.Event e) : base(e)
        {
        }
    }
}
