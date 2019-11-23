// <copyright file="RobotObservedFaceEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Anki.Vector.Objects;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Robot Observed Face Event Args
    /// </summary>
    /// <seealso cref="RobotEventArgs" />
    public class RobotObservedFaceEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the facial expression Vector has recognized on the face.
        /// </summary>
        public FacialExpression Expression { get; }

        /// <summary>
        /// Gets individual expression values histogram, sums to 100
        /// </summary>
        public IReadOnlyList<uint> ExpressionValues { get; }

        /// <summary>
        /// Gets the internal ID assigned to the face.
        /// </summary>
        public int FaceId { get; }

        /// <summary>
        /// Gets the image rectangle of the face.
        /// </summary>
        public ImageRect ImageRect { get; }

        /// <summary>
        /// Gets points representing the outline of the left eye.
        /// </summary>
        public IReadOnlyList<Point> LeftEye { get; }

        /// <summary>
        /// Gets points representing the outline of the mouth
        /// </summary>
        public IReadOnlyList<Point> Mouth { get; }

        /// <summary>
        /// Gets the name Vector has associated with the face.  Empty if face is not recognized or enrolled
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets points representing the outline of the nose.
        /// </summary>
        public IReadOnlyList<Point> Nose { get; }

        /// <summary>
        /// Gets the location of the face
        /// </summary>
        public Pose Pose { get; }

        /// <summary>
        /// Gets points representing the outline of the right eye.
        /// </summary>
        public IReadOnlyList<Point> RightEye { get; }

        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObservedFaceEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObservedFaceEventArgs(ExternalInterface.Event e) : base(e)
        {
            var robotObservedFace = e.RobotObservedFace;
            Expression = (FacialExpression)robotObservedFace.Expression;
            ExpressionValues = new ReadOnlyCollection<uint>(robotObservedFace.ExpressionValues);
            FaceId = robotObservedFace.FaceId;
            ImageRect = new ImageRect(robotObservedFace.ImgRect);
            LeftEye = robotObservedFace.LeftEye.Select(r => new Point(r)).ToList().AsReadOnly();
            Mouth = robotObservedFace.Mouth.Select(r => new Point(r)).ToList().AsReadOnly();
            Name = robotObservedFace.Name;
            Nose = robotObservedFace.Nose.Select(r => new Point(r)).ToList().AsReadOnly();
            Pose = new Pose(robotObservedFace.Pose);
            RightEye = robotObservedFace.RightEye.Select(r => new Point(r)).ToList().AsReadOnly();
            RobotTimestamp = robotObservedFace.Timestamp;
        }
    }
}
