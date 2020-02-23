// <copyright file="AudioReceivedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Audio received event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class AudioReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="response">The audio feed response.</param>
        internal AudioReceivedEventArgs(ExternalInterface.AudioFeedResponse response)
        {
            this.GroupId = response.GroupId;
            this.AudioData = response.SignalPower.ToByteArray();
            this.NoiseFloorPower = response.NoiseFloorPower;
            this.DirectionStrengths = response.DirectionStrengths.ToByteArray();
            this.SourceConfidence = response.SourceConfidence;
            this.SourceDirection = response.SourceDirection;
            this.RobotTimestamp = response.RobotTimeStamp;
        }

        /// <summary>
        /// Gets the index of audio feed response
        /// </summary>
        public uint GroupId { get; }

        /// <summary>
        /// Gets the audio data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Audio data")]
        public byte[] AudioData { get; }

        /// <summary>
        /// Gets the power value, convert to db with log_10(value)
        /// </summary>
        public uint NoiseFloorPower { get; }

        /// <summary>
        /// Gets the histogram data of which directions this audio chunk came from
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Direction data")]
        public byte[] DirectionStrengths { get; }

        /// <summary>
        /// Gets the accuracy of the calculated SourceDirection
        /// </summary>
        public uint SourceConfidence { get; }

        /// <summary>
        /// Gets the source direction; 0-11 with 12 representing "invalid"
        /// </summary>
        public uint SourceDirection { get; }

        /// <summary>
        /// Gets the robot timestamp at the transmission of this audio sample group
        /// </summary>
        public uint RobotTimestamp { get; }
    }
}
