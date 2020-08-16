// <copyright file="RobotObservedMotionEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Robot observed motion event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    [Serializable]
    public class RobotObservedMotionEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the area of the supporting region for the point, as a fraction of the bottom region
        /// </summary>
        public float BottomImgArea { get; }

        /// <summary>
        /// Gets pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int BottomImgX { get; }

        /// <summary>
        /// Gets pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int BottomImgY { get; }

        /// <summary>
        /// Gets the area of the supporting region for the point, as a fraction of the ground ROI.  If unable to map to the ground, area = 0
        /// </summary>
        public float GroundArea { get; }

        /// <summary>
        /// Gets the coordinates of the point on the ground, relative to robot, in mm
        /// </summary>
        public int GroundX { get; }

        /// <summary>
        /// Gets the coordinates of the point on the ground, relative to robot, in mm
        /// </summary>
        public int GroundY { get; }

        /// <summary>
        /// Gets the area of the supporting region for the point, as a fraction of the image
        /// </summary>
        public float ImgArea { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int ImgX { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int ImgY { get; }

        /// <summary>
        /// Gets the area of the supporting region for the point, as a fraction of the left region
        /// </summary>
        public float LeftImgArea { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int LeftImgX { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int LeftImgY { get; }

        /// <summary>
        /// Gets the area of the supporting region for the point, as a fraction of the right region
        /// </summary>
        public float RightImgArea { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int RightImgX { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int RightImgY { get; }

        /// <summary>
        /// Gets the area of the supporting region for the point, as a fraction of the top region
        /// </summary>
        public float TopImgArea { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int TopImgX { get; }

        /// <summary>
        /// Gets the pixel coordinate of the point in the image, relative to top-left corner.
        /// </summary>
        public int TopImgY { get; }

        /// <summary>
        /// Gets the timestamp of the corresponding image
        /// </summary>
        public uint Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObservedMotionEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        internal RobotObservedMotionEventArgs(ExternalInterface.Event e) : base(e)
        {
            BottomImgArea = e.RobotObservedMotion.BottomImgArea;
            BottomImgX = e.RobotObservedMotion.BottomImgX;
            BottomImgY = e.RobotObservedMotion.BottomImgY;
            GroundArea = e.RobotObservedMotion.GroundArea;
            GroundX = e.RobotObservedMotion.GroundX;
            GroundY = e.RobotObservedMotion.GroundY;
            ImgArea = e.RobotObservedMotion.ImgArea;
            ImgX = e.RobotObservedMotion.ImgX;
            ImgY = e.RobotObservedMotion.ImgY;
            LeftImgArea = e.RobotObservedMotion.LeftImgArea;
            LeftImgX = e.RobotObservedMotion.LeftImgX;
            LeftImgY = e.RobotObservedMotion.LeftImgY;
            RightImgArea = e.RobotObservedMotion.RightImgArea;
            RightImgX = e.RobotObservedMotion.RightImgX;
            RightImgY = e.RobotObservedMotion.RightImgY;
            TopImgArea = e.RobotObservedMotion.TopImgArea;
            TopImgX = e.RobotObservedMotion.TopImgX;
            TopImgY = e.RobotObservedMotion.TopImgY;
            Timestamp = e.RobotObservedMotion.Timestamp;
        }
    }
}
