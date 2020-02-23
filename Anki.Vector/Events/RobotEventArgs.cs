// <copyright file="RobotEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Event type
    /// </summary>
    public enum EventType
    {
        /// <summary>No event was triggered</summary>
        None = 0,
        /// <summary>Timestamped status event</summary>
        TimeStampedStatus = 1,
        /// <summary>Wake word event</summary>
        WakeWord = 3,
        /// <summary>Observed face event</summary>
        RobotObservedFace = 5,
        /// <summary>Detected faced changed</summary>
        RobotChangedObservedFaceId = 6,
        /// <summary>Object event</summary>
        ObjectEvent = 7,
        /// <summary>Stimulation info event</summary>
        StimulationInfo = 8,
        /// <summary>Photo taken event</summary>
        PhotoTaken = 9,
        /// <summary>Robot state event</summary>
        RobotState = 10,
        /// <summary>Cube battery event</summary>
        CubeBattery = 11,
        /// <summary>Keep alive event</summary>
        KeepAlive = 12,
        /// <summary>Connection response event</summary>
        ConnectionResponse = 13,
        /// <summary>Mirror mode disabled event</summary>
        MirrorModeDisabled = 16,
        /// <summary>Vision modes auto disabled event</summary>
        VisionModesAutoDisabled = 17
    }

    /// <summary>
    /// Robot event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public abstract class RobotEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the event type.
        /// </summary>
        public EventType EventType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        internal RobotEventArgs(Event e)
        {
            EventType = (EventType)e.EventTypeCase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotEventArgs"/> class.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        internal RobotEventArgs(EventType eventType)
        {
            EventType = eventType;
        }
    }
}
