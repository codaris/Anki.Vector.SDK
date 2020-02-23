// <copyright file="ImageEncoding.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// The image encoding
    /// </summary>
    public enum ImageEncoding
    {
        NoneImageEncoding = 0,
        RawGray = 1,
        RawRgb = 2,
        Yuyv = 3,
        Yuv420Sp = 4,
        Bayer = 5,
        JpegGray = 6,
        JpegColor = 7,
        JpegColorHalfWidth = 8,
        JpegMinimizedGray = 9,
        JpegMinimizedColor = 10
    }
}
