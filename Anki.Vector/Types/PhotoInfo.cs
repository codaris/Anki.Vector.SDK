// <copyright file="PhotoInfo.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Store photo information
    /// </summary>
    public class PhotoInfo
    {
        /// <summary>
        /// Gets the photo identifier.
        /// </summary>
        public uint PhotoId { get; }

        /// <summary>
        /// Gets the photo taken on date and time.
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// Gets a value indicating whether photo was copied to application.
        /// </summary>
        public bool PhotoCopiedToApp { get; }

        /// <summary>
        /// Gets a value indicating whether thumb was copied to application.
        /// </summary>
        public bool ThumbCopiedToApp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoInfo"/> class.
        /// </summary>
        /// <param name="photoInfo">The photo information.</param>
        internal PhotoInfo(ExternalInterface.PhotoInfo photoInfo)
        {
            PhotoId = photoInfo.PhotoId;
            PhotoCopiedToApp = photoInfo.PhotoCopiedToApp;
            ThumbCopiedToApp = photoInfo.ThumbCopiedToApp;
            DateTime = DateTimeOffset.FromUnixTimeSeconds(photoInfo.TimestampUtc).DateTime.ToLocalTime();
        }
    }
}
