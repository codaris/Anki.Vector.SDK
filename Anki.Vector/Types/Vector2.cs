// <copyright file="Vector2.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Represents a 2D Vector (type/units aren't specified)
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// Gets the x component
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Gets the y component
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
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
            return obj is Vector2 ? this.Equals((Vector2)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Vector2 other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Vector2 lhs, Vector2 rhs)
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
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Implements the addition operator.
        /// </summary>
        /// <param name="lhs">The left value.</param>
        /// <param name="rhs">The right value.</param>
        /// <returns>Sum of 2 instances</returns>
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        /// <summary>
        /// Implements the subtraction operator.
        /// </summary>
        /// <param name="lhs">The left value.</param>
        /// <param name="rhs">The right value.</param>
        /// <returns>Difference of 2 instances</returns>
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        /// <summary>
        /// Implements the multiplication operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to multiply by</param>
        /// <returns>The Vector multiplied by the value</returns>
        public static Vector2 operator *(Vector2 vector, float value)
        {
            return new Vector2(vector.X * value, vector.Y * value);
        }

        /// <summary>
        /// Implements the multiplication operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to multiply by</param>
        /// <returns>The Vector multiplied by the value</returns>
        public static Vector2 operator *(Vector2 vector, int value)
        {
            return new Vector2(vector.X * value, vector.Y * value);
        }

        /// <summary>
        /// Implements the division operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to divide by</param>
        /// <returns>The Vector divided by the value</returns>
        public static Vector2 operator /(Vector2 vector, float value)
        {
            return new Vector2(vector.X / value, vector.Y / value);
        }

        /// <summary>
        /// Implements the division operator.
        /// </summary>
        /// <param name="vector">The Vector.</param>
        /// <param name="value">The value to divide by</param>
        /// <returns>The Vector divided by the value</returns>
        public static Vector2 operator /(Vector2 vector, int value)
        {
            return new Vector2(vector.X / value, vector.Y / value);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"({X:n2}, {Y:n2})";
        }
    }
}
