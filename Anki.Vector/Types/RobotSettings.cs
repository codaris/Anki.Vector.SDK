// <copyright file="RobotSettings.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
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
        /// The supported timezones
        /// </summary>
        public static readonly IReadOnlyList<string> Timezones = new string[]
        {
            "Pacific/Honolulu",
            "America/Juneau",
            "America/Los_Angeles",
            "America/Phoenix",
            "America/Denver",
            "America/Lima",
            "America/Chicago",
            "America/Bogota",
            "America/New_York",
            "America/Halifax",
            "America/Argentina/Buenos_Aires",
            "America/Santiago",
            "America/Sao_Paulo",
            "America/St_Johns",
            "GMT",
            "Europe/Lisbon",
            "Europe/London",
            "Africa/Lagos",
            "Africa/Harare",
            "Europe/Paris",
            "Europe/Athens",
            "Europe/Istanbul",
            "Africa/Addis_Ababa",
            "Europe/Moscow",
            "Asia/Dubai",
            "Asia/Tehran",
            "Asia/Karachi",
            "Asia/Kolkata",
            "Asia/Dhaka",
            "Asia/Bangkok",
            "Asia/Jakarta",
            "Asia/Hong_Kong",
            "Asia/Singapore",
            "Asia/Manila",
            "Asia/Seoul",
            "Asia/Tokyo",
            "Australia/Perth",
            "Australia/Darwin",
            "Australia/Adelaide",
            "Australia/Brisbane",
            "Australia/Sydney",
            "Pacific/Auckland"
        };

        /// <summary>
        /// The supported locales
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> Locales = new Dictionary<string, string>()
        {
            ["en-US"] = "US English",
            ["en-GB"] = "UK English",
            ["en-AU"] = "AU English"
        };

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
