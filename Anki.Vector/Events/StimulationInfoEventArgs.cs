// <copyright file="StimulationInfoEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Vector stimulation info event args
    /// </summary>
    [Serializable]
    public class StimulationInfoEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the acceleration per second squared.
        /// </summary>
        public float Acceleration { get; }

        /// <summary>
        /// Gets the emotion events.
        /// </summary>
        public IReadOnlyList<string> EmotionEvents { get; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public float MaxValue { get; }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public float MinValue { get; }

        /// <summary>
        /// Gets the stimulation value.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Gets the stimulation value before event.  Matches value if there were no events.
        /// </summary>
        public float ValueBeforeEvent { get; }

        /// <summary>
        /// Gets the velocity per second
        /// </summary>
        public float Velocity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StimulationInfoEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal StimulationInfoEventArgs(ExternalInterface.Event e) : base(e)
        {
            var stimulationInfo = e.StimulationInfo;
            Acceleration = stimulationInfo.Accel;
            EmotionEvents = stimulationInfo.EmotionEvents.ToList().AsReadOnly();
            MaxValue = stimulationInfo.MaxValue;
            MinValue = stimulationInfo.MinValue;
            Value = stimulationInfo.Value;
            ValueBeforeEvent = stimulationInfo.ValueBeforeEvent;
            Velocity = stimulationInfo.Velocity;
        }
    }
}
