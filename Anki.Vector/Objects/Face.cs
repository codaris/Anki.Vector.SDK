// <copyright file="Face.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Anki.Vector.Events;
using Anki.Vector.Types;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// A single face that Vector has detected.
    /// <para>May represent a face that has previously been enrolled, in which case name will hold the name that it was enrolled with.</para>
    /// <para>Each Face instance has a <see cref="FaceId"/> integer - This may change if Vector later gets an improved view and makes a different 
    /// prediction about which face he is looking at.</para>
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.ObservableObject" />
    public class Face : ObservableObject
    {
        /// <summary>
        /// Gets the facial expression Vector has recognized on the face.
        /// </summary>
        public FacialExpression Expression { get => _expression; private set => SetProperty(ref _expression, value); }
        private FacialExpression _expression;

        /// <summary>
        /// Gets individual expression values histogram, sums to 100
        /// </summary>
        public IReadOnlyList<uint> ExpressionValues { get => _expressionValues; private set => SetProperty(ref _expressionValues, value); }
        private IReadOnlyList<uint> _expressionValues;

        /// <summary>
        /// Gets the internal ID assigned to the face.
        /// </summary>
        public int FaceId { get => _faceId; private set => SetProperty(ref _faceId, value); }
        private int _faceId;

        /// <summary>
        /// Gets points representing the outline of the left eye.
        /// </summary>
        public IReadOnlyList<Point> LeftEye { get => _leftEye; private set => SetProperty(ref _leftEye, value); }
        private IReadOnlyList<Point> _leftEye;

        /// <summary>
        /// Gets points representing the outline of the mouth
        /// </summary>
        public IReadOnlyList<Point> Mouth { get => _mouth; private set => SetProperty(ref _mouth, value); }
        private IReadOnlyList<Point> _mouth;

        /// <summary>
        /// Gets the name Vector has associated with the face.  Empty if face is not recognized or enrolled
        /// </summary>
        public string Name { get => _name; private set => SetProperty(ref _name, value); }
        private string _name;

        /// <summary>
        /// Gets points representing the outline of the nose.
        /// </summary>
        public IReadOnlyList<Point> Nose { get => _nose; private set => SetProperty(ref _nose, value); }
        private IReadOnlyList<Point> _nose;

        /// <summary>
        /// Gets points representing the outline of the right eye.
        /// </summary>
        public IReadOnlyList<Point> RightEye { get => _rightEye; private set => SetProperty(ref _rightEye, value); }
        private IReadOnlyList<Point> _rightEye;

        /// <summary>
        /// Initializes a new instance of the <see cref="Face" /> class.
        /// </summary>
        /// <param name="faceId">The face identifier.</param>
        /// <param name="robot">The robot.</param>
        internal Face(int faceId, Robot robot) : base(robot)
        {
            FaceId = faceId;
        }

        /// <summary>
        /// Called when the face is observed
        /// </summary>
        /// <param name="e">The <see cref="RobotObservedObjectEventArgs"/> instance containing the event data.</param>
        internal virtual void OnFaceObserved(RobotObservedFaceEventArgs e)
        {
            var now = DateTime.Now;
            LastEventTime = now;
            IsVisible = true;
            LastImageRect = e.ImageRect;
            LastObservedTime = now;
            LastObservedTimestamp = e.RobotTimestamp;
            Pose = e.Pose;

            // Face specific properties
            Expression = e.Expression;
            ExpressionValues = e.ExpressionValues;
            LeftEye = e.LeftEye;
            Mouth = e.Mouth;
            Name = e.Name;
            Nose = e.Nose;
            RightEye = e.RightEye;
        }

        /// <summary>
        /// Call when the <see cref="E:ChangeObservedFaceId" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RobotChangedObservedFaceIdEventArgs"/> instance containing the event data.</param>
        internal virtual void OnChangeObservedFaceId(RobotChangedObservedFaceIdEventArgs e)
        {
            FaceId = e.NewId;
        }

        /// <summary>
        /// Gets the name of the object type.
        /// </summary>
        public override string ObjectTypeName => "Face";

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{ObjectTypeName} ({FaceId})";
        }
    }
}
