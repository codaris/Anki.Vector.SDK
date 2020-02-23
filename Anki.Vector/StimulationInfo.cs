// <copyright file="StimulationInfo.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Anki.Vector
{
    /// <summary>
    /// Vector stimulation info
    /// </summary>
    public class StimulationInfo
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
        /// Initializes a new instance of the <see cref="StimulationInfo"/> class.
        /// </summary>
        /// <param name="stimulationInfo">The stimulation information.</param>
        internal StimulationInfo(Anki.Vector.ExternalInterface.StimulationInfo stimulationInfo)
        {
            Acceleration = stimulationInfo.Accel;
            EmotionEvents = stimulationInfo.EmotionEvents.ToList().AsReadOnly();
            MaxValue = stimulationInfo.MaxValue;
            MinValue = stimulationInfo.MinValue;
            Value = stimulationInfo.Value;
            ValueBeforeEvent = stimulationInfo.ValueBeforeEvent;
            Velocity = stimulationInfo.Velocity;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string events = string.Join(", ", EmotionEvents);
            return $"{Value:n2} [{events}] A:{Acceleration:n2} V:{Velocity:n2}";
        }
    }
}
