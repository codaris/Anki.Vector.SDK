// <copyright file="KeepAliveEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Keep alive event args
    /// </summary>
    /// <seealso cref="RobotEventArgs" />
    public class KeepAliveEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal KeepAliveEventArgs(ExternalInterface.Event e) : base(e)
        {
        }
    }
}
