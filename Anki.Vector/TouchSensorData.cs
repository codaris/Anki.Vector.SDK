// <copyright file="TouchSensorData.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector
{
    /// <summary>
    /// A touch sample from the capacitive touch sensor, accompanied with the robot’s conclusion on whether this is considered a valid touch.
    /// </summary>
    public struct TouchSensorData : IEquatable<TouchSensorData>
    {
        /// <summary>
        /// Gets a value indicating whether the robot’s conclusion on whether the current value is considered a valid touch.
        /// </summary>
        public bool IsBeingTouched { get; }

        /// <summary>
        /// Gets the detected sensitivity from the touch sensor.
        /// <para>This will not map to a constant raw value, as it may be impacted by various environmental factors such as whether the robot is on its charger, 
        /// being held, humidity, etc.</para>
        /// </summary>
        public uint RawTouchValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchSensorData"/> struct.
        /// </summary>
        /// <param name="touchData">The touch data.</param>
        internal TouchSensorData(TouchData touchData)
        {
            IsBeingTouched = touchData.IsBeingTouched;
            RawTouchValue = touchData.RawTouchValue;
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
            return obj is TouchSensorData ? this.Equals((TouchSensorData)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return IsBeingTouched.GetHashCode() ^ RawTouchValue.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TouchSensorData other)
        {
            return (IsBeingTouched == other.IsBeingTouched) && (RawTouchValue == other.RawTouchValue);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(TouchSensorData lhs, TouchSensorData rhs)
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
        public static bool operator !=(TouchSensorData lhs, TouchSensorData rhs)
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
            return (IsBeingTouched ? "Yes" : "No") + $" ({RawTouchValue})";
        }
    }
}
