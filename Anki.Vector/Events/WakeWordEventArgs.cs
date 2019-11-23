// <copyright file="WakeWordEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Wake word event types
    /// </summary>
    public enum WakeWordEventType
    {
        /// <summary>No event</summary>
        None = 0,
        /// <summary>Wake word started event</summary>
        WakeWordBegin = 1,
        /// <summary>Wake work ended event</summary>
        WakeWordEnd = 2
    }

    /// <summary>
    /// Wake word event args
    /// </summary>
    /// <seealso cref="RobotEventArgs" />
    public abstract class WakeWordEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the wake word event type.
        /// </summary>
        public WakeWordEventType WakeWordEventType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WakeWordEventArgs" /> class.
        /// </summary>
        /// <param name="e">The event info.</param>
        internal WakeWordEventArgs(ExternalInterface.Event e) : base(e)
        {
            WakeWordEventType = (WakeWordEventType)e.WakeWord.WakeWordTypeCase;
        }
    }
}
