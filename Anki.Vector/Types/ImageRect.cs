// <copyright file="ImageRect.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Image coordinates and size
    /// </summary>
    /// <seealso cref="System.IEquatable{ImageRect}" />
    public class ImageRect : IEquatable<ImageRect>
    {
        /// <summary>
        /// Gets the top left x value of where the object was last visible within Vector’s camera view.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Gets the top left y value of where the object was last visible within Vector’s camera view..
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Gets the width of the object from when it was last visible within Vector’s camera view..
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Gets the height of the object from when it was last visible within Vector’s camera view.
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRect"/> class.
        /// </summary>
        /// <param name="cladRect">The robot rectangle object.</param>
        internal ImageRect(CladRect cladRect)
        {
            X = cladRect.XTopLeft;
            Y = cladRect.YTopLeft;
            Width = cladRect.Width;
            Height = cladRect.Height;
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
            return obj is ImageRect ? this.Equals((ImageRect)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ImageRect other)
        {
            if (other is null) return false;
            return (X == other.X) && (Y == other.Y) && (Width == other.Width) && (Height == other.Height);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ImageRect lhs, ImageRect rhs)
        {
            if (lhs is null) return false;
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
        public static bool operator !=(ImageRect lhs, ImageRect rhs)
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
            return $"(({X}, {Y}), ({Width} x {Height}))";
        }
    }
}
