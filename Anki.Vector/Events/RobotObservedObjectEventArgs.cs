// <copyright file="RobotObservedObjectEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Robot observed object event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotObservedObjectEventArgs : RobotObjectEventArgs
    {
        /// <summary>
        /// Gets the position in image coordinates
        /// </summary>
        public ImageRect ImageRect { get; }

        /// <summary>
        /// Gets a value indicating whether the object is active.
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Gets the object identifier.
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public ObjectType ObjectType { get; }

        /// <summary>
        /// Gets the index of the custom object.
        /// </summary>
        public CustomObjectType CustomObjectType { get; }

        /// <summary>
        /// Gets the pose of the object
        /// </summary>
        public Pose Pose { get; }

        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Gets the absolute top face orientation in radians
        /// </summary>
        public float TopFaceOrientationRad { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObservedObjectEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObservedObjectEventArgs(ExternalInterface.Event e) : base(e)
        {
            var robotObservedObject = e.ObjectEvent.RobotObservedObject;
            ImageRect = new ImageRect(robotObservedObject.ImgRect);
            IsActive = robotObservedObject.IsActive != 0;
            ObjectId = robotObservedObject.ObjectId;
            ObjectType = robotObservedObject.ObjectType.ToSdkObjectType();
            CustomObjectType = robotObservedObject.ObjectType.ToSdkCustomObjectType();
            Pose = new Pose(robotObservedObject.Pose);
            RobotTimestamp = robotObservedObject.Timestamp;
            TopFaceOrientationRad = robotObservedObject.TopFaceOrientationRad;
        }
    }
}
