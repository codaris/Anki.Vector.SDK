// <copyright file="ColorProfile.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Applies transforms to make Vector’s lights and colors appear as intended, by limiting maximum channel intensity.
    /// </summary>
    public struct ColorProfile : IEquatable<ColorProfile>
    {
        /// <summary>
        /// Gets the multiplier used on the red channel.
        /// </summary>
        public double RedMultiplier { get; }

        /// <summary>
        /// Gets the multiplier used on the green channel.
        /// </summary>
        public double GreenMultiplier { get; }

        /// <summary>
        /// Gets the multiplier used on the blue channel.
        /// </summary>
        public double BlueMultiplier { get; }

        /// <summary>
        /// Gets the color profile balanced so that a max color value more closely resembles pure white.
        /// </summary>
        public static ColorProfile WhiteBalancedCubeProfile { get; } = new ColorProfile(1.0, 0.95, 0.7);

        /// <summary>
        /// Gets the color profile to get the maximum possible brightness out of each LED.
        /// </summary>
        public static ColorProfile MaxProfile { get; } = new ColorProfile(1.0, 1.0, 1.0);

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorProfile"/> struct.
        /// </summary>
        /// <param name="redMultiplier">The red multiplier.</param>
        /// <param name="greenMultiplier">The green multiplier.</param>
        /// <param name="blueMultiplier">The blue multiplier.</param>
        public ColorProfile(double redMultiplier, double greenMultiplier, double blueMultiplier)
        {
            RedMultiplier = redMultiplier;
            GreenMultiplier = greenMultiplier;
            BlueMultiplier = blueMultiplier;
        }

        /// <summary>
        /// Augments the color using the color profile
        /// </summary>
        /// <param name="originalColor">Color of the original.</param>
        /// <returns>The augmented color instance</returns>
        public Color AugmentColor(Color originalColor)
        {
            return Color.FromRgb((byte)(RedMultiplier * originalColor.R), (byte)(GreenMultiplier * originalColor.G), (byte)(BlueMultiplier * originalColor.B));
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
            return obj is ColorProfile ? this.Equals((ColorProfile)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return RedMultiplier.GetHashCode() ^ GreenMultiplier.GetHashCode() ^ BlueMultiplier.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ColorProfile other)
        {
            return (RedMultiplier == other.RedMultiplier) && (GreenMultiplier == other.GreenMultiplier) && (BlueMultiplier == other.BlueMultiplier);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ColorProfile lhs, ColorProfile rhs)
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
        public static bool operator !=(ColorProfile lhs, ColorProfile rhs)
        {
            return !(lhs == rhs);
        }
    }
}
