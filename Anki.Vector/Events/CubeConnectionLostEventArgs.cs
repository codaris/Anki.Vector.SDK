// <copyright file="CubeConnectionLostEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Cube connection lost event args
    /// </summary>
    /// <seealso cref="RobotEventArgs" />
    public class CubeConnectionLostEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CubeConnectionLostEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal CubeConnectionLostEventArgs(ExternalInterface.Event e) : base(e)
        {
        }
    }
}
