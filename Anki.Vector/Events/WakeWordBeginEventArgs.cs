// <copyright file="WakeWordBeginEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Wake word begin event args
    /// </summary>
    /// <seealso cref="WakeWordEventArgs" />
    public class WakeWordBeginEventArgs : WakeWordEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WakeWordBeginEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal WakeWordBeginEventArgs(ExternalInterface.Event e) : base(e)
        {
        }
    }
}
