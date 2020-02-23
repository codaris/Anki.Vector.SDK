// <copyright file="Acceleration.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    using System;
    using Anki.Vector.ExternalInterface;

    /// <summary>
    /// Represents acceleration values
    /// </summary>
    public struct Acceleration : IEquatable<Acceleration>
    {
        /// <summary>
        /// Gets the x acceleration in mm/s^2
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Gets the y acceleration in mm/s^2
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Gets the z acceleration in mm/s^2
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Acceleration"/> struct.
        /// </summary>
        /// <param name="x">The x acceleration in mm/s^2.</param>
        /// <param name="y">The y acceleration in mm/s^2.</param>
        /// <param name="z">The z acceleration in mm/s^2.</param>
        public Acceleration(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Acceleration"/> struct.
        /// </summary>
        /// <param name="accelData">The acceleration data.</param>
        internal Acceleration(AccelData accelData)
        {
            X = accelData.X;
            Y = accelData.Y;
            Z = accelData.Z;
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
            return obj is Acceleration ? this.Equals((Acceleration)obj) : false;
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
        public bool Equals(Acceleration other)
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
        public static bool operator ==(Acceleration lhs, Acceleration rhs)
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
        public static bool operator !=(Acceleration lhs, Acceleration rhs)
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
