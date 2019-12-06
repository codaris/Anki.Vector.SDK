// <copyright file="Quaternion.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Represents the rotation of an object in the world.
    /// </summary>
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <summary>
        /// The q0 (w) value of the quaternion.
        /// </summary>
        public float Q0 { get; }

        /// <summary>
        /// The q1 (i) value of the quaternion.
        /// </summary>
        public float Q1 { get; }

        /// <summary>
        /// The q2 (j) value of the quaternion.
        /// </summary>
        public float Q2 { get; }

        /// <summary>
        /// The q3 (k) value of the quaternion.
        /// </summary>
        public float Q3 { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="q0">The The q0 (w) value of the quaternion.</param>
        /// <param name="q1">The q1 (i) value of the quaternion..</param>
        /// <param name="q2">The q2 (j) value of the quaternion..</param>
        /// <param name="q3">The q3 (k) value of the quaternion..</param>
        public Quaternion(float q0, float q1, float q2, float q3)
        {
            Q0 = q0;
            Q1 = q1;
            Q2 = q2;
            Q3 = q3;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.  This function converts an angle in the z axis (Euler angle z component) to a quaternion.
        /// </summary>
        /// <param name="radiansZ">angle in the z axis in radians.</param>
        public Quaternion(float radiansZ)
        {
            // Define the quaternion to be converted from a Euler angle (x,y,z) of 0,0,angle_z
            // These equations have their original equations above, and simplified implemented
            // q0 = cos(x/2)*cos(y/2)*cos(z/2) + sin(x/2)*sin(y/2)*sin(z/2)
            Q0 = (float)Math.Cos(radiansZ / 2);
            // q1 = sin(x/2)*cos(y/2)*cos(z/2) - cos(x/2)*sin(y/2)*sin(z/2)
            Q1 = 0;
            // q2 = cos(x/2)*sin(y/2)*cos(z/2) + sin(x/2)*cos(y/2)*sin(z/2)
            Q2 = 0;
            // q3 = cos(x/2)*cos(y/2)*sin(z/2) - sin(x/2)*sin(y/2)*cos(z/2)
            Q3 = (float)Math.Sin(radiansZ / 2);
        }

        /// <summary>
        /// An Angle instance representing the z Euler component of the object's rotation in radians
        /// </summary>
        public float AngleZ => (float)Math.Atan2(2 * (Q1 * Q2 + Q0 * Q3), 1 - 2 * (Math.Pow(Q2, 2) + Math.Pow(Q3, 2)));

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Quaternion ? this.Equals((Quaternion)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Q0.GetHashCode() ^ Q1.GetHashCode() ^ Q2.GetHashCode() ^ Q3.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Quaternion other)
        {
            return (Q0 == other.Q0) && (Q1 == other.Q1) && (Q2 == other.Q2) && (Q3 == other.Q3);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Quaternion lhs, Quaternion rhs)
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
        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
            return !(lhs == rhs);
        }


        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Use user's localization")]
        public override string ToString()
        {
            return AngleZ.ToString("n2");
        }
    }
}
