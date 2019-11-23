// <copyright file="RobotStateEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Gets robot state event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotStateEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the current accelerometer reading
        /// </summary>
        public Acceleration Acceleration { get; }

        /// <summary>
        /// Gets the ID of the object currently being carried (-1 if none)
        /// </summary>
        public int CarryingObjectId { get; }

        /// <summary>
        /// Not supported
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int CarryingObjectOnTopId { get; }

        /// <summary>
        /// Gets the current gyroscope reading (x, y, z) 
        /// </summary>
        public AngularVelocity Gyro { get; }

        /// <summary>
        /// Gets Vector's head angle (up/down) in radians
        /// </summary>
        public float HeadAngleRad { get; }

        /// <summary>
        /// Gets the ID of the object the head is tracking to (-1 if none)
        /// </summary>
        public int HeadTrackingObjectId { get; }

        /// <summary>
        /// Gets the robot's timestamp for the last image seen.
        /// </summary>
        public uint LastImageTimestamp { get; }

        /// <summary>
        /// Gets Vector’s left wheel speed in mm/sec
        /// </summary>
        public float LeftWheelSpeedMmps { get; }

        /// <summary>
        /// Gets the height of Vector’s lift from the ground.
        /// </summary>
        public float LiftHeightMm { get; }

        /// <summary>
        /// Gets the ID of the object that the robot is localized to (-1 if none)
        /// </summary>
        public int LocalizedToObjectId { get; }

        /// <summary>
        /// Gets the current pose (position and orientation) of Vector.
        /// </summary>
        public Pose Pose { get; }

        /// <summary>
        /// Gets Vector's pose angle (heading in X-Y plane) in radians
        /// </summary>
        public float PoseAngleRad { get; }

        /// <summary>
        /// Gets Vector’s pose pitch (angle up/down) in radians
        /// </summary>
        public float PosePitchRad { get; }

        /// <summary>
        /// Gets the proximity sensor data.
        /// </summary>
        public ProximitySensorData Proximity { get; }

        /// <summary>
        /// Gets Vector's right wheel speed in mm/sec
        /// </summary>
        public float RightWheelSpeedMmps { get; }

        /// <summary>
        /// Gets the various status properties of the robot.
        /// </summary>
        public RobotStatus Status { get; }

        /// <summary>
        /// Gets the object touch detection values.
        /// </summary>
        public TouchSensorData Touch { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotStateEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotStateEventArgs(ExternalInterface.Event e) : base(e)
        {
            var robotState = e.RobotState;
            Acceleration = new Acceleration(robotState.Accel);
            CarryingObjectId = robotState.CarryingObjectId;
            CarryingObjectOnTopId = robotState.CarryingObjectOnTopId;
            Gyro = new AngularVelocity(robotState.Gyro);
            HeadAngleRad = robotState.HeadAngleRad;
            HeadTrackingObjectId = robotState.HeadTrackingObjectId;
            LastImageTimestamp = robotState.LastImageTimeStamp;
            LeftWheelSpeedMmps = robotState.LeftWheelSpeedMmps;
            LiftHeightMm = robotState.LiftHeightMm;
            LocalizedToObjectId = robotState.LocalizedToObjectId;
            Pose = new Pose(robotState.Pose);
            PoseAngleRad = robotState.PoseAngleRad;
            PosePitchRad = robotState.PosePitchRad;
            Proximity = new ProximitySensorData(robotState.ProxData);
            RightWheelSpeedMmps = robotState.RightWheelSpeedMmps;
            Status = new RobotStatus(robotState.Status);
            Touch = new TouchSensorData(robotState.TouchData);
        }
    }
}
