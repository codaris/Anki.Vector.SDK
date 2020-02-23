// <copyright file="RobotLifetimeStats.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;
using Newtonsoft.Json;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Vector lifetime statistics
    /// </summary>
    public class RobotLifetimeStats
    {
        /// <summary>
        /// Gets the number of seconds Vector has been alive.
        /// </summary>
        [JsonProperty("Alive.seconds")]
        public int AliveSeconds { get; private set; }

        /// <summary>
        /// Gets the number of times Vector reacted to the trigger word (Hey Vector).
        /// </summary>
        [JsonProperty("BStat.ReactedToTriggerWord")]
        public int ReactedToTriggerWord { get; private set; }

        /// <summary>
        /// Gets the number of utilities used.
        /// </summary>
        [JsonProperty("FeatureType.Utility")]
        public int UtilitiesUsed { get; private set; }

        /// <summary>
        /// Gets the distance driven.
        /// </summary>
        [JsonProperty("Odom.Body")]
        public int DistanceDriven { get; private set; }

        /// <summary>
        /// Gets the duration of the petting in seconds.
        /// </summary>
        [JsonProperty("Pet.ms")]
        public int PettingDuration { get; private set; }

        /// <summary>
        /// Gets the lifetime sensory score.
        /// </summary>
        [JsonProperty("Stim.CumlPosDelta")]
        public int LifetimeSensoryScore { get; private set; }

        /// <summary>
        /// Creates <see cref="RobotSetting"/> from a named jdoc (validates the Jdoc).
        /// </summary>
        /// <param name="namedJdoc">The named jdoc.</param>
        /// <returns>The robot lifetime stats instance</returns>
        /// <exception cref="ArgumentNullException">namedJdoc</exception>
        /// <exception cref="ArgumentException">JDoc Type RobotSettings expected, received {namedJdoc.JdocType} instead. - namedJdoc</exception>
        internal static RobotLifetimeStats FromNamedJdoc(NamedJdoc namedJdoc)
        {
            if (namedJdoc == null) throw new ArgumentNullException(nameof(namedJdoc));
            if (namedJdoc.JdocType != JdocType.RobotLifetimeStats) throw new ArgumentException($"JDoc Type RobotSettings expected, received {namedJdoc.JdocType} instead.", nameof(namedJdoc));
            return FromJdoc(namedJdoc.Doc);
        }

        /// <summary>
        /// Creates <see cref="RobotLifetimeStats"/> from a  jdoc.
        /// </summary>
        /// <param name="jdoc">The jdoc.</param>
        /// <returns>The robot lifetime stats instance</returns>
        internal static RobotLifetimeStats FromJdoc(Jdoc jdoc)
        {
            return JsonConvert.DeserializeObject<RobotLifetimeStats>(jdoc.JsonDoc);
        }
    }
}
