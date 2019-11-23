// <copyright file="RobotSettings.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;
using Newtonsoft.Json;

namespace Anki.Vector.Types
{
    /// <summary>
    /// THe permanently stored Vector robot settings
    /// </summary>
    public class RobotSettings
    {
        /// <summary>
        /// Gets or sets the button wake word.  This is which service (Vector or Alexa) responds when his back button is pressed.
        /// </summary>
        [JsonProperty("button_wakeword")]
        public ButtonWakeWord ButtonWakeWord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether clock is display in 24 hour style.
        /// </summary>
        [JsonProperty("clock_24_hour")]
        public bool Clock24Hour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether temperature is displayed in Fahrenheit.
        /// </summary>
        [JsonProperty("temp_is_fahrenheit")]
        public bool TempIsFahrenheit { get; set; }

        /// <summary>
        /// Gets or sets the default physical location of Vector (city, state/prov, country).
        /// </summary>
        [JsonProperty("default_location")]
        public string DefaultLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether distance is displayed in metric.
        /// </summary>
        [JsonProperty("dist_is_metric")]
        public bool DistIsMetric { get; set; }

        /// <summary>
        /// Gets or sets the language locale of Vector (en_US, en_UK, en_AU).
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets the master volume.
        /// </summary>
        [JsonProperty("master_volume")]
        public RobotVolume MasterVolume { get; set; }

        /// <summary>
        /// Gets or sets the time zone as a standard timezone database name (e.g. "America/Los_Angeles")
        /// </summary>
        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets Vector's eye color
        /// </summary>
        [JsonProperty("eye_color")]
        public EyeColor EyeColor { get; set; }

        /// <summary>
        /// Creates <see cref="RobotSetting"/> from a named jdoc (validates the Jdoc).
        /// </summary>
        /// <param name="namedJdoc">The named jdoc.</param>
        /// <returns>Robot settings instance</returns>
        /// <exception cref="ArgumentNullException">namedJdoc</exception>
        /// <exception cref="ArgumentException">JDoc Type RobotSettings expected, received {namedJdoc.JdocType} instead. - namedJdoc</exception>
        internal static RobotSettings FromNamedJdoc(NamedJdoc namedJdoc)
        {
            if (namedJdoc == null) throw new ArgumentNullException(nameof(namedJdoc));
            if (namedJdoc.JdocType != JdocType.RobotSettings) throw new ArgumentException($"JDoc Type RobotSettings expected, received {namedJdoc.JdocType} instead.", nameof(namedJdoc));
            return FromJdoc(namedJdoc.Doc);
        }

        /// <summary>
        /// Creates <see cref="RobotSetting"/> from a  jdoc.
        /// </summary>
        /// <param name="jdoc">The jdoc.</param>
        /// <returns>Robot settings instance</returns>
        internal static RobotSettings FromJdoc(Jdoc jdoc)
        {
            return JsonConvert.DeserializeObject<RobotSettings>(jdoc.JsonDoc);
        }

        /// <summary>
        /// Converts to internal <see cref="RobotSettingsConfig"/> value.
        /// </summary>
        /// <returns>Robot settings configuration</returns>
        internal RobotSettingsConfig ToRobotSettingsConfig()
        {
            return new RobotSettingsConfig
            {
                ButtonWakeword = (ExternalInterface.ButtonWakeWord)ButtonWakeWord,
                Clock24Hour = Clock24Hour,
                TempIsFahrenheit = TempIsFahrenheit,
                DefaultLocation = DefaultLocation,
                DistIsMetric = DistIsMetric,
                Locale = Locale,
                MasterVolume = (ExternalInterface.Volume)MasterVolume,
                TimeZone = TimeZone,
                EyeColor = (ExternalInterface.EyeColor)EyeColor
            };
        }
    }
}
