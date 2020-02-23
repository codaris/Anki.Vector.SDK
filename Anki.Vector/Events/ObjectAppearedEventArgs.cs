// <copyright file="ObjectAppearedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object appeared event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectAppearedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Gets the position in image coordinates
        /// </summary>
        public ImageRect ImageRect { get; }

        /// <summary>
        /// Gets the pose of the object
        /// </summary>
        public Pose Pose { get; }

        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAppearedEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="RobotObservedObjectEventArgs" /> instance containing the event data.</param>
        internal ObjectAppearedEventArgs(ObservableObject obj, RobotObservedObjectEventArgs e) : base(obj)
        {
            ImageRect = e.ImageRect;
            Pose = e.Pose;
            RobotTimestamp = e.RobotTimestamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAppearedEventArgs"/> class.
        /// </summary>
        /// <param name="face">The face.</param>
        /// <param name="e">The <see cref="RobotObservedFaceEventArgs"/> instance containing the event data.</param>
        internal ObjectAppearedEventArgs(Face face, RobotObservedFaceEventArgs e) : base(face)
        {
            ImageRect = e.ImageRect;
            Pose = e.Pose;
            RobotTimestamp = e.RobotTimestamp;
        }
    }
}
