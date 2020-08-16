// <copyright file="Image.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Represents a single image captured from the robot.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets the image identifier.
        /// </summary>
        public uint ImageId { get; }

        /// <summary>
        /// Gets the image data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Image data")]
        public byte[] Data { get; }

        /// <summary>
        /// Gets the frame robot timestamp.
        /// </summary>
        public uint FrameTimestamp { get; }

        /// <summary>
        /// Gets the image encoding.
        /// </summary>
        public ImageEncoding Encoding { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class from a camera feed response.
        /// </summary>
        /// <param name="response">The response.</param>
        internal Image(ExternalInterface.CameraFeedResponse response)
        {
            ImageId = response.ImageId;
            Data = response.Data.ToByteArray();
            Encoding = MapImageEncoding(response.ImageEncoding);
            FrameTimestamp = response.FrameTimeStamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from a single image response.
        /// </summary>
        /// <param name="response">The response.</param>
        internal Image(ExternalInterface.CaptureSingleImageResponse response)
        {
            ImageId = response.ImageId;
            Data = response.Data.ToByteArray();
            Encoding = MapImageEncoding(response.ImageEncoding);
            FrameTimestamp = response.FrameTimeStamp;
        }

        /// <summary>
        /// Maps the image encoding.
        /// </summary>
        /// <param name="imageEncoding">The robot image encoding.</param>
        /// <returns>The SDK image encoding</returns>
        internal static ImageEncoding MapImageEncoding(ImageChunk.Types.ImageEncoding imageEncoding)
        {
            switch (imageEncoding)
            {
                case ImageChunk.Types.ImageEncoding.NoneImageEncoding: return ImageEncoding.NoneImageEncoding;
                case ImageChunk.Types.ImageEncoding.RawGray: return ImageEncoding.RawGray;
                case ImageChunk.Types.ImageEncoding.RawRgb: return ImageEncoding.RawRgb;
                case ImageChunk.Types.ImageEncoding.Yuyv: return ImageEncoding.Yuyv;
                case ImageChunk.Types.ImageEncoding.Yuv420Sp: return ImageEncoding.Yuv420Sp;
                case ImageChunk.Types.ImageEncoding.Bayer: return ImageEncoding.Bayer;
                case ImageChunk.Types.ImageEncoding.JpegGray: return ImageEncoding.JpegGray;
                case ImageChunk.Types.ImageEncoding.JpegColor: return ImageEncoding.JpegColor;
                case ImageChunk.Types.ImageEncoding.JpegColorHalfWidth: return ImageEncoding.JpegColorHalfWidth;
                case ImageChunk.Types.ImageEncoding.JpegMinimizedGray: return ImageEncoding.JpegMinimizedGray;
                case ImageChunk.Types.ImageEncoding.JpegMinimizedColor: return ImageEncoding.JpegMinimizedColor;
                default:
                    throw new NotSupportedException($"ImageEncoding {imageEncoding} is not supported");
            }
        }
    }
}
