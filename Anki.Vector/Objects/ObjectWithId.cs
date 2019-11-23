// <copyright file="ObjectWithId.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.Events;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Object tracked with an ObjectId
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.ObservableObject" />
    public abstract class ObjectWithId : ObservableObject
    {
        /// <summary>
        /// Gets the internal ID assigned to the object.  This value can only be assigned once as it is static on the robot.
        /// </summary>
        public int ObjectId { get => _objectId; private set => SetProperty(ref _objectId, value); }
        private int _objectId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithId" /> class.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="robot">The robot.</param>
        internal ObjectWithId(int objectId, Robot robot) : base(robot)
        {
            ObjectId = objectId;
        }

        /// <summary>
        /// Called when the object is observed
        /// </summary>
        /// <param name="e">The <see cref="RobotObservedObjectEventArgs"/> instance containing the event data.</param>
        internal virtual void OnObjectObserved(RobotObservedObjectEventArgs e)
        {
            var now = DateTime.Now;
            LastEventTime = now;
            IsVisible = true;
            LastImageRect = e.ImageRect;
            LastObservedTime = now;
            LastObservedTimestamp = e.RobotTimestamp;
            Pose = e.Pose;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{ObjectTypeName} ({ObjectId})";
        }
    }
}
