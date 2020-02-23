// <copyright file="ProximitySensorData.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector
{
    /// <summary>
    /// A distance sample from the time-of-flight sensor with metadata describing reliability of the measurement
    /// <para>The proximity sensor is located near the bottom of Vector between the two front wheels, facing forward.The reported distance describes how far in front 
    /// of this sensor the robot feels an obstacle is. The sensor estimates based on time-of-flight information within a field of view which the engine resolves 
    /// to a certain quality value.</para>
    /// <para>Four additional flags are supplied by the engine to indicate whether this proximity data is considered valid for the robot’s internal 
    /// path finding.  Respecting these is optional, but will help your code respect the behavior of the robot’s innate object avoidance.</para>
    /// </summary>
    public struct ProximitySensorData : IEquatable<ProximitySensorData>
    {
        /// <summary>
        /// Gets the distance between the sensor and a detected object
        /// </summary>
        public uint DistanceMm { get; }

        /// <summary>
        /// Gets a value indicating whether Vector’s lift is blocking the time-of-flight sensor. While the lift will send clear proximity signals, it’s not useful for object detection.
        /// </summary>
        public bool IsLiftInFov { get; }

        /// <summary>
        /// Gets a value indicating whether the sensor detected an object in the valid operating range.
        /// </summary>
        public bool FoundObject { get; }

        /// <summary>
        /// Gets a value indicating whether the sensor has confirmed it has not detected anything up to its max range.
        /// </summary>
        public bool Unobstructed { get; }

        /// <summary>
        /// Gets the quality of the detected object.  The proximity sensor detects obstacles within a given field of view, this value represents the likelihood of the reported 
        /// distance being a solid surface.
        /// </summary>
        public float SignalQuality { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProximitySensorData"/> struct.
        /// </summary>
        /// <param name="proxData">The proximity data.</param>
        internal ProximitySensorData(ProxData proxData)
        {
            DistanceMm = proxData.DistanceMm;
            FoundObject = proxData.FoundObject;
            Unobstructed = proxData.Unobstructed;
            IsLiftInFov = proxData.IsLiftInFov;
            SignalQuality = proxData.SignalQuality;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is ProximitySensorData ? this.Equals((ProximitySensorData)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return DistanceMm.GetHashCode() ^ FoundObject.GetHashCode() ^ IsLiftInFov.GetHashCode() ^ Unobstructed.GetHashCode()
                ^ SignalQuality.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ProximitySensorData other)
        {
            return (DistanceMm == other.DistanceMm) && (FoundObject == other.FoundObject) && (IsLiftInFov == other.IsLiftInFov)
                 && (Unobstructed == other.Unobstructed) && (SignalQuality == other.SignalQuality);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ProximitySensorData lhs, ProximitySensorData rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ProximitySensorData lhs, ProximitySensorData rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (Unobstructed) return "Unobstructed";
            if (FoundObject) return $"{DistanceMm}mm Q:{SignalQuality:n2}";
            if (IsLiftInFov) return "Lift in FOV";
            return string.Empty;
        }
    }
}
