// <copyright file="ColorProfile.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// Applies transforms to make Vector’s lights and colors appear as intended, by limiting maximum channel intensity.
    /// </summary>
    public struct ColorProfile
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
    }
}
