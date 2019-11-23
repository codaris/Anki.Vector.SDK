// <copyright file="ImageReceivedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Image received event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ImageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the image identifier.
        /// </summary>
        public uint ImageId { get; }

        /// <summary>
        /// Gets the image data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Image data")]
        public byte[] ImageData { get; }

        /// <summary>
        /// Gets the frame robot timestamp.
        /// </summary>
        public uint FrameTimestamp { get; }

        /// <summary>
        /// Gets the image encoding.
        /// </summary>
        public ImageEncoding ImageEncoding { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal ImageReceivedEventArgs(ExternalInterface.CameraFeedResponse response)
        {
            ImageId = response.ImageId;
            ImageData = response.Data.ToByteArray();
            ImageEncoding = CameraComponent.MapImageEncoding(response.ImageEncoding);
            FrameTimestamp = response.FrameTimeStamp;
        }
    }
}
