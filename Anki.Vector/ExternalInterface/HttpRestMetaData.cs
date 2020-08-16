// <copyright file="HttpRestMetaData.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

// This file adds additional meta data to the Vector GRPC classes to that they can be serialized as JSON for REST calls.
// It is not auto-generated.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Anki.Vector.ExternalInterface
{
    /// <summary>
    /// Naming strategy that uses a static JsonMapping property on the class to map property names to JSON object keys
    /// </summary>
    /// <typeparam name="T">The same class as the JsonObject attribute is used on</typeparam>
    /// <seealso cref="Newtonsoft.Json.Serialization.DefaultNamingStrategy" />
    public class MappedNamingStrategy<T> : DefaultNamingStrategy
    {
        /// <summary>The property mapping dictionary loaded from the class</summary>
        private IDictionary<string, string> propertyMapping = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedNamingStrategy{T}"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Classes with MappedNamingStrategy must have static JsonMapping property</exception>
        public MappedNamingStrategy()
        {
            var property = typeof(T).GetProperty("JsonMapping", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (property == null) throw new InvalidOperationException("Classes with MappedNamingStrategy must have static JsonMapping property");
            propertyMapping = (IDictionary<string, string>)property.GetValue(null, null);
        }

        /// <summary>
        /// Resolves the specified property name.
        /// </summary>
        /// <param name="name">The property name to resolve.</param>
        /// <returns>The resolved property name.</returns>
        protected override string ResolvePropertyName(string name)
        {
            var resolved = propertyMapping.TryGetValue(name, out string resolvedName);
            return resolved ? resolvedName : base.ResolvePropertyName(name);
        }
    }

    /// <summary>
    /// Ensures these types can be serialized for HTTP API
    /// </summary>
    internal interface IHttpJsonData
    {
    }

    [JsonObject(NamingStrategyType = typeof(MappedNamingStrategy<Jdoc>))]
    public partial class Jdoc : IHttpJsonData
    {
        public static Dictionary<string, string> JsonMapping { get; } = new Dictionary<string, string>()
        {
            [nameof(DocVersion)] = "doc_version",
            [nameof(FmtVersion)] = "fmt_version",
            [nameof(ClientMetadata)] = "client_metadata",
            [nameof(JsonDoc)] = "json_doc"
        };
    }

    [JsonObject(NamingStrategyType = typeof(MappedNamingStrategy<UpdateSettingsRequest>))]
    public partial class UpdateSettingsRequest : IHttpJsonData
    {
        public static Dictionary<string, string> JsonMapping { get; } = new Dictionary<string, string>()
        {
            [nameof(Settings)] = "settings",
        };
    }

    [JsonObject(NamingStrategyType = typeof(MappedNamingStrategy<RobotSettingsConfig>))]
    public partial class RobotSettingsConfig : IHttpJsonData
    {
        public static Dictionary<string, string> JsonMapping { get; } = new Dictionary<string, string>()
        {
            [nameof(Clock24Hour)] = "clock_24_hour",
            [nameof(EyeColor)] = "eye_color",
            [nameof(DefaultLocation)] = "default_location",
            [nameof(DistIsMetric)] = "dist_is_metric",
            [nameof(Locale)] = "locale",
            [nameof(MasterVolume)] = "master_volume",
            [nameof(TempIsFahrenheit)] = "temp_is_fahrenheit",
            [nameof(TimeZone)] = "time_zone",
            [nameof(ButtonWakeword)] = "button_wakeword",
        };
    }

    [JsonObject(NamingStrategyType = typeof(MappedNamingStrategy<UpdateSettingsResponse>))]
    public partial class UpdateSettingsResponse : IHttpJsonData
    {
        public static Dictionary<string, string> JsonMapping { get; } = new Dictionary<string, string>()
        {
            [nameof(Status)] = "status",
            [nameof(Code)] = "code",
            [nameof(Doc)] = "doc"
        };
    }

    [JsonObject(NamingStrategyType = typeof(MappedNamingStrategy<UpdateSettingsResponse>))]
    public partial class ResponseStatus : IHttpJsonData
    {
        public static Dictionary<string, string> JsonMapping { get; } = new Dictionary<string, string>()
        {
            [nameof(Code)] = "code"
        };
    }
}