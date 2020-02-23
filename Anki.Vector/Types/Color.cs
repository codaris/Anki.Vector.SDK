// <copyright file="Color.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Extensions for working with robot colors
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        /// Gets the red component value of this Color structure.
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// Gets the green component value of this Color structure.
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Gets the blue component value of this Color structure.
        /// </summary>
        public byte B { get; }

        /// <summary>
        /// Gets the red color.
        /// </summary>
        public static Color Red { get; } = new Color(255, 0, 0);

        /// <summary>
        /// Gets the blue color.
        /// </summary>
        public static Color Blue { get; } = new Color(0, 0, 255);

        /// <summary>
        /// Gets the green color.
        /// </summary>
        public static Color Green { get; } = new Color(0, 255, 0);

        /// <summary>
        /// Gets the cyan color.
        /// </summary>
        public static Color Cyan { get; } = new Color(0, 255, 255);

        /// <summary>
        /// Gets the magenta color.
        /// </summary>
        public static Color Magenta { get; } = new Color(255, 0, 255);

        /// <summary>
        /// Gets the yellow color.
        /// </summary>
        public static Color Yellow { get; } = new Color(255, 0, 255);

        /// <summary>
        /// Gets the white color.
        /// </summary>
        public static Color White { get; } = new Color(255, 255, 255);

        /// <summary>
        /// Gets the black/off color
        /// </summary>
        public static Color Black { get; } = new Color(0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
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
            return obj is Color ? this.Equals((Color)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Color other)
        {
            return (R == other.R) && (G == other.G) && (B == other.B);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Color lhs, Color rhs)
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
        public static bool operator !=(Color lhs, Color rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Create a color from RGB values
        /// </summary>
        /// <param name="red">The red component.</param>
        /// <param name="green">The green component.</param>
        /// <param name="blue">The blue component.</param>
        /// <returns>Color instance</returns>
        public static Color FromRgb(byte red, byte green, byte blue)
        {
            return new Color(red, green, blue);
        }

        /// <summary>
        /// Converts to robot color.
        /// </summary>
        /// <returns>The color in RGBA</returns>
        internal uint ToRobotColor()
        {
            return ((uint)R << 24) | ((uint)G << 16) | ((uint)B << 8) | 0xff;
        }

        /// <summary>
        /// Converts to 565 format.
        /// </summary>
        /// <returns>565 16bit value</returns>
        internal ushort To565()
        {
            int b = (B >> 3) & 0x1f;
            int g = ((G >> 2) & 0x3f) << 5;
            int r = ((R >> 3) & 0x1f) << 11;
            return (ushort)(r | g | b);
        }

        /// <summary>
        /// Converts to 565 bytepair.
        /// </summary>
        /// <returns>Pair of 565 bytes</returns>
        internal byte[] ToBytePair()
        {
            uint color = ToRobotColor();
            uint red5 = ((color >> 24) & 0xff) >> 3;
            uint green6 = ((color >> 16) & 0xff) >> 2;
            uint blue5 = ((color >> 8) & 0xff) >> 3;

            uint green3_hi = green6 >> 3;
            uint green3_low = green6 & 0x07;

            byte int_565_color_lowbyte = (byte)((green3_low << 5) | blue5);
            byte int_565_color_highbyte = (byte)((red5 << 3) | green3_hi);
            return new byte[] { int_565_color_highbyte, int_565_color_lowbyte };
        }
    }
}
