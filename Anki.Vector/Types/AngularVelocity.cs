// <copyright file="AngularVelocity.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    using System;
    using Anki.Vector.ExternalInterface;

    /// <summary>
    /// Represents the angular velocity 
    /// </summary>
    public struct AngularVelocity : IEquatable<AngularVelocity>
    {
        /// <summary>
        /// The x velocity in rad/s
        /// </summary>
        public float X { get; }

        /// <summary>
        /// The y velocity in rad/s
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// The z velocity in rad/s
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AngularVelocity"/> struct.
        /// </summary>
        /// <param name="x">The x velocity in rad/s.</param>
        /// <param name="y">The y velocity in rad/s.</param>
        /// <param name="z">The z velocity in rad/s.</param>
        public AngularVelocity(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AngularVelocity"/> struct.
        /// </summary>
        /// <param name="gyroData">The gyro data.</param>
        internal AngularVelocity(GyroData gyroData)
        {
            X = gyroData.X;
            Y = gyroData.Y;
            Z = gyroData.Z;
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
            return obj is AngularVelocity ? this.Equals((AngularVelocity)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(AngularVelocity other)
        {
            return (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(AngularVelocity lhs, AngularVelocity rhs)
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
        public static bool operator !=(AngularVelocity lhs, AngularVelocity rhs)
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
            return $"({X:n2}, {Y:n2}, {Z:n2})";
        }
    }
}
