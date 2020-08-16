// <copyright file="CameraConfig.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// The fixed properties for Vector's camera.
    /// <para>
    /// A full 3x3 calibration matrix for doing 3D reasoning based on the camera images would look like:
    /// 
    ///    +--------------+--------------+---------------+
    ///    |focal_length.x|      0       |    center.x   |
    ///    +--------------+--------------+---------------+
    ///    |       0      |focal_length.y|    center.y   |
    ///    +--------------+--------------+---------------+
    ///    |       0      |       0      |        1      |
    ///    +--------------+--------------+---------------+
    ///    
    /// </para>
    /// <remarks></remarks>
    /// </summary>
    public class CameraConfig
    {
        /// <summary>
        /// Gets the focal center of the camera.
        /// <para>This is the position of the optical center of projection within the image. It will be close to the center of the image, but adjusted 
        /// based on the calibration of the lens. It is in floating point pixel values e.g. (155.11, 111.40).</para>
        /// </summary>
        public Vector2 Center { get; }

        /// <summary>
        /// Gets the focal length of the camera.
        /// <para>This is focal length combined with pixel skew (as the pixels aren't perfectly square), so there are subtly different values for x and y.  
        /// It is in floating point pixel values e.g. (288.87, 288.36)</para>
        /// </summary>
        public Vector2 FocalLength { get; }

        /// <summary>Gets the x (horizontal) field of view in radians.</summary>
        public float FovX { get; }

        /// <summary>Gets the y (vertical) field of view in radians.</summary>
        public float FovY { get; }

        /// <summary>Gets the minimum supported exposure time in milliseconds.</summary>
        public uint MinCameraExposureTimeMs { get; }

        /// <summary>Gets the maximum supported exposure time in milliseconds.</summary>
        public uint MaxCameraExposureTimeMs { get; }

        /// <summary>Gets the minimum supported camera gain.</summary>
        public float MinCameraGain { get; }

        /// <summary>Gets the maximum supported camera gain.</summary>
        public float MaxCameraGain { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraConfig"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal CameraConfig(CameraConfigResponse response)
        {
            Center = new Vector2(response.CenterX, response.CenterY);
            FocalLength = new Vector2(response.FocalLengthX, response.FocalLengthY);
            FovX = response.FovX;
            FovY = response.FovY;
            MinCameraExposureTimeMs = response.MinCameraExposureTimeMs;
            MaxCameraExposureTimeMs = response.MaxCameraExposureTimeMs;
            MinCameraGain = response.MinCameraGain;
            MaxCameraGain = response.MaxCameraGain;
        }
    }
}
