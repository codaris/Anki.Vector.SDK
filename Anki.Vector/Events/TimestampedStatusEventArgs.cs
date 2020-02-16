// <copyright file="TimestampedStatusEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Status type
    /// </summary>
    public enum StatusType
    {
        /// <summary>No event</summary>
        None = 0,
        /// <summary>AI Feature Status</summary>
        FeatureStatus = 1,
        /// <summary>Face scan started event</summary>
        MeetVictorFaceScanStarted = 2,
        /// <summary>Face scan completed event</summary>
        MeetVictorFaceScanComplete = 3,
        /// <summary>Face enrollment completed event</summary>
        FaceEnrollmentCompleted = 4
    }

    /// <summary>
    /// The timestamped status event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public abstract class TimestampedStatusEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the type of the status.
        /// </summary>
        public StatusType StatusType { get; }

        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimestampedStatusEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal TimestampedStatusEventArgs(ExternalInterface.Event e) : base(e)
        {
            StatusType = (StatusType)e.TimeStampedStatus.Status.StatusTypeCase;
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(e.TimeStampedStatus.TimestampUtc);
        }
    }
}
