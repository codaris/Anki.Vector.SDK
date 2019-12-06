// <copyright file="Vector3.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    using System;

    /// <summary>
    /// Represents a 3D Vector (type/units aren't specified)
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// The x component
        /// </summary>
        public float X { get; }

        /// <summary>
        /// The y component
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// The z component
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
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
            return obj is Vector3 ? this.Equals((Vector3)obj) : false;
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
        public bool Equals(Vector3 other)
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
        public static bool operator ==(Vector3 lhs, Vector3 rhs)
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
        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Gets the magnitude.
        /// </summary>
        public float Magnitude => (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

        /// <summary>
        /// Gets the normalized version of this instance with the same direction and unit magnitude.
        /// </summary>
        public Vector3 Normalized
        {
            get
            {
                float mag = Magnitude;
                if (mag == 0) return new Vector3(0, 0, 0);
                return new Vector3(X / mag, Y / mag, Z / mag);
            }
        }

        /// <summary>
        /// Return the dots product of this instance with another Vector3
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>The dot product</returns>
        public float Dot(Vector3 other)
        {
            return (X * other.X) + (Y * other.Y) + (Z * other.Z);
        }

        /// <summary>
        /// Return the cross product of this instance with another Vector3
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>The cross product</returns>
        public Vector3 Cross(Vector3 other)
        {
            return new Vector3((Y * other.Z) - (Z * other.Y), (Z * other.X) - (X * other.Z), (X * other.Y) - (Y * other.X));
        }

        /// <summary>
        /// Implements the addition operator.
        /// </summary>
        /// <param name="lhs">The left value.</param>
        /// <param name="rhs">The right value.</param>
        /// <returns>Sum of 2 instances</returns>
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        /// <summary>
        /// Implements the subtraction operator.
        /// </summary>
        /// <param name="lhs">The left value.</param>
        /// <param name="rhs">The right value.</param>
        /// <returns>Difference of 2 instances</returns>
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        /// <summary>
        /// Implements the multiplication operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to multiply by</param>
        /// <returns>The Vector multiplied by the value</returns>
        public static Vector3 operator *(Vector3 vector, float value)
        {
            return new Vector3(vector.X * value, vector.Y * value, vector.Z * value);
        }

        /// <summary>
        /// Implements the multiplication operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to multiply by</param>
        /// <returns>The Vector multiplied by the value</returns>
        public static Vector3 operator *(Vector3 vector, int value)
        {
            return new Vector3(vector.X * value, vector.Y * value, vector.Z * value);
        }

        /// <summary>
        /// Implements the division operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to divide by</param>
        /// <returns>The Vector divided by the value</returns>
        public static Vector3 operator /(Vector3 vector, float value)
        {
            return new Vector3(vector.X / value, vector.Y / value, vector.Z / value);
        }

        /// <summary>
        /// Implements the division operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to divide by</param>
        /// <returns>The Vector divided by the value</returns>
        public static Vector3 operator /(Vector3 vector, int value)
        {
            return new Vector3(vector.X / value, vector.Y / value, vector.Z / value);
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
