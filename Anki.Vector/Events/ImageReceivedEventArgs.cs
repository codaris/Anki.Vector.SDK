// <copyright file="ImageReceivedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
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
        /// Gets the image.
        /// </summary>
        public Image Image { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal ImageReceivedEventArgs(ExternalInterface.CameraFeedResponse response)
        {
            Image = new Image(response);
        }
    }
}
