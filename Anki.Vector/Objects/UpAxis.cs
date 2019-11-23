// <copyright file="UpAxis.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Objects
{
    /// <summary>
    /// The up access of the object
    /// </summary>
    public enum UpAxis
    {
        /// <summary>Invalid axis</summary>
        InvalidAxis = 0,
        /// <summary>Negative X axis is up</summary>
        XNegative = 1,
        /// <summary>Positive X axis is up</summary>
        XPositive = 2,
        /// <summary>Negative Y axis is up</summary>
        YNegative = 3,
        /// <summary>Positive X axis is up</summary>
        YPositive = 4,
        /// <summary>Negative Z axis up</summary>
        ZNegative = 5,
        /// <summary>Positive Z axis up</summary>
        ZPositive = 6
    }
}
