// <copyright file="BehaviorComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

//
// This file adds additional meta data to the Vector GRPC classes to that they can be serialized as JSON for REST calls.
// It is not auto-generated.
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

#pragma warning disable CA1812, CS1591

#if NETSTANDARD

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Implementation of MetadataTypeAttribute if not defined
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MetadataTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the metadata class.
        /// </summary>
        public Type MetadataClassType { get;  }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTypeAttribute"/> class.
        /// </summary>
        /// <param name="metadataClassType">Type of the metadata class.</param>
        /// <exception cref="ArgumentNullException">metadataClassType</exception>
        public MetadataTypeAttribute(Type metadataClassType)
        {
            if (metadataClassType == null) throw new ArgumentNullException(nameof(metadataClassType));
            MetadataClassType = metadataClassType;
        }
    }
}

#endif

namespace Anki.Vector.ExternalInterface
{
    /// <summary>
    /// Ensures these types can be serialized for HTTP API
    /// </summary>
    internal interface IHttpJsonData
    {
    }

    [MetadataType(typeof(JdocMetaData))]
    public partial class Jdoc : IHttpJsonData
    {
    }

    internal class JdocMetaData
    {
        [JsonProperty("doc_version")]
        public ulong DocVersion { get; set; }

        [JsonProperty("fmt_version")]
        public ulong FmtVersion { get; set; }

        [JsonProperty("client_metadata")]
        public string ClientMetadata { get; set; }

        [JsonProperty("json_doc")]
        public string JsonDoc { get; set; }
    }

    [MetadataType(typeof(UpdateSettingsRequestMetaData))]
    public partial class UpdateSettingsRequest : IHttpJsonData
    {
    }

    internal class UpdateSettingsRequestMetaData
    {
        [JsonProperty("settings")]
        public RobotSettingsConfig Settings { get; set; }
    }

    [MetadataType(typeof(RobotSettingsConfigMetaData))]
    public partial class RobotSettingsConfig : IHttpJsonData
    {
    }

    internal class RobotSettingsConfigMetaData
    {
        [JsonProperty("clock_24_hour")]
        public bool Clock24Hour { get; set; }

        [JsonProperty("eye_color")]
        public global::Anki.Vector.ExternalInterface.EyeColor EyeColor { get; set; }

        [JsonProperty("default_location")]
        public string DefaultLocation { get; set; }

        [JsonProperty("dist_is_metric")]
        public bool DistIsMetric { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("master_volume")]
        public global::Anki.Vector.ExternalInterface.Volume MasterVolume { get; set; }

        [JsonProperty("temp_is_fahrenheit")]
        public bool TempIsFahrenheit { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("button_wakeword")]
        public global::Anki.Vector.ExternalInterface.ButtonWakeWord ButtonWakeword { get; set; }
    }

    [MetadataType(typeof(UpdateSettingsResponseMetaData))]
    public partial class UpdateSettingsResponse : IHttpJsonData
    {
    }

    internal class UpdateSettingsResponseMetaData
    {
        [JsonProperty("status")]
        public global::Anki.Vector.ExternalInterface.ResponseStatus Status { get; set; }

        [JsonProperty("code")]
        public global::Anki.Vector.ExternalInterface.ResultCode Code { get; set; }

        [JsonProperty("doc")]
        public global::Anki.Vector.ExternalInterface.Jdoc Doc { get; set; }
    }

    [MetadataType(typeof(ResponseStatusMetaData))]
    public partial class ResponseStatus : IHttpJsonData
    {
    }

    internal class ResponseStatusMetaData
    {
        [JsonProperty("code")]
        public global::Anki.Vector.ExternalInterface.ResponseStatus.Types.StatusCode Code { get; set; }
    }

    [MetadataType(typeof(AppIntentRequestMetaData))]
    public partial class AppIntentRequest : IHttpJsonData
    {
    }

    internal class AppIntentRequestMetaData
    {
        [JsonProperty("intent")]
        public string Intent { get; set; }
        [JsonProperty("param")]
        public string Param { get; set; }
    }

    [MetadataType(typeof(AppIntentResponseMetaData))]
    public partial class AppIntentResponse : IHttpJsonData
    {
    }

    internal class AppIntentResponseMetaData
    {
        [JsonProperty("status")]
        public global::Anki.Vector.ExternalInterface.ResponseStatus Status { get; set; }
    }
}