// <copyright file="Angle.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Angle extensions
    /// </summary>
    public static class Angle
    {
        /// <summary>
        /// Convert degrees into radians
        /// </summary>
        /// <param name="degrees">The number of degrees.</param>
        /// <returns>Radians</returns>
        public static float Degrees(this float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        /// <summary>
        /// Convert degrees into radians
        /// </summary>
        /// <param name="degrees">The number of degrees.</param>
        /// <returns>Radians</returns>
        public static float Degrees(this int degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }
    }
}
