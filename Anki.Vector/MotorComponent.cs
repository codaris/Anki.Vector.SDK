// <copyright file="MotorComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector
{
    using System;
    using System.Threading.Tasks;
    using Anki.Vector.Types;

    /// <summary>
    /// The speed of the motors for drive and move
    /// </summary>
    public enum MotorSpeed
    {
        /// <summary>The slow speed</summary>
        Slow,
        /// <summary>The medium (default) speed</summary>
        Medium,
        /// <summary>The fast speed</summary>
        Fast
    }

    /// <summary>
    /// The drive direction (forwards or backwards)
    /// </summary>
    public enum DriveDirection
    {
        /// <summary>No forward/backward movement</summary>
        None = 0,
        /// <summary>Move forwards movement</summary>
        Forwards = 1,
        /// <summary>Move backwards movement</summary>
        Backwards = -1
    }

    /// <summary>
    /// The drive turn direction (left, right)
    /// </summary>
    public enum TurnDirection
    {
        /// <summary>No turning</summary>
        None = 0,
        /// <summary>Turn right</summary>
        Right = 1,
        /// <summary>Turn left</summary>
        Left = -1,
    }

    /// <summary>
    /// The direction for moving head or lift (up, down)
    /// </summary>
    public enum MoveDirection
    {
        /// <summary>No up/down movement</summary>
        None = 0,
        /// <summary>Up movement</summary>
        Up = 1,
        /// <summary>Down movement</summary>
        Down = -1
    }

    /// <summary>
    /// Controls the low-level motor functions.
    /// </summary>
    public class MotorComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MotorComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal MotorComponent(Robot robot) : base(robot)
        {
        }

        /// <summary>
        /// Tell Vector to move his wheels / treads at a given speed.  The wheels will continue to move at that speed until commanded to drive at a new speed.
        /// To unlock the wheel track set all speeds to zero (0).
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="leftWheelSpeed">Speed of the left tread (in millimeters per second)</param>
        /// <param name="rightWheelSpeed">Speed of the right tread (in millimeters per second)</param>
        /// <param name="leftWheelAccel">Acceleration of left tread (in millimeters per second squared)</param>
        /// <param name="rightWheelAccel">Acceleration of right tread (in millimeters per second squared)</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> SetWheelMotors(float leftWheelSpeed, float rightWheelSpeed, float leftWheelAccel = 0, float rightWheelAccel = 0)
        {
            var response = await Robot.RunControlMethod(client => client.DriveWheelsAsync(new ExternalInterface.DriveWheelsRequest()
            {
                LeftWheelMmps = leftWheelSpeed,
                RightWheelMmps = rightWheelSpeed,
                LeftWheelMmps2 = leftWheelAccel,
                RightWheelMmps2 = rightWheelAccel
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Tell Vector’s head motor to move with a certain speed.  Positive speed for up, negative speed for down.
        /// To unlock the head track call with speed of zero (0).
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="speed">Motor speed for Vector’s head, measured in radians per second.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> SetHeadMotor(float speed)
        {
            var response = await Robot.RunControlMethod(client => client.MoveHeadAsync(new ExternalInterface.MoveHeadRequest()
            {
                SpeedRadPerSec = speed
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Tell Vector’s lift motor to move with a certain speed.  Positive speed for up, negative speed for down. 
        /// To unlock the lift track call with speed of zero (0).
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="speed">Motor speed for Vector’s lift, measured in radians per second.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> SetLiftMotor(float speed)
        {
            var response = await Robot.RunControlMethod(client => client.MoveLiftAsync(new ExternalInterface.MoveLiftRequest()
            {
                SpeedRadPerSec = speed
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Tell Vector to stop all motors.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> StopAllMotors()
        {
            var response = await Robot.RunControlMethod(client => client.StopAllMotorsAsync(new ExternalInterface.StopAllMotorsRequest())).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Drives Vector based on direction and speed
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="driveDirection">The drive direction.</param>
        /// <param name="turnDirection">The turn direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> Drive(DriveDirection driveDirection, TurnDirection turnDirection, MotorSpeed speed = MotorSpeed.Medium)
        {
            float driveSpeed = PickSpeed(speed, 50, 75, 150);
            float turnSpeed = PickSpeed(speed, 30, 50, 100);
            float driveValue = (float)driveDirection;
            float turnValue = (float)turnDirection;
            if (driveValue < 0) turnValue = -turnValue;
            float leftWheelSpeed = (driveValue * driveSpeed) + (turnValue * turnSpeed);
            float rightWheelSpeed = (driveValue * driveSpeed) - (turnValue * turnSpeed);
            return await SetWheelMotors(leftWheelSpeed, rightWheelSpeed, leftWheelSpeed * 4, rightWheelSpeed * 4).ConfigureAwait(false);
        }

        /// <summary>
        /// Moves the head based on direction and speed
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="moveDirection">The move direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> MoveHead(MoveDirection moveDirection, MotorSpeed speed = MotorSpeed.Medium)
        {
            float headSpeed = PickSpeed(speed, 0.5f, 1, 2);
            return await SetHeadMotor((float)moveDirection * headSpeed).ConfigureAwait(false);
        }

        /// <summary>
        /// Moves the lift based on direction and speed
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="moveDirection">The move direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<StatusCode> MoveLift(MoveDirection moveDirection, MotorSpeed speed = MotorSpeed.Medium)
        {
            float liftSpeed = PickSpeed(speed, 2, 4, 8);
            return await SetLiftMotor((float)moveDirection * liftSpeed).ConfigureAwait(false);
        }

        /// <summary>
        /// Picks the speed.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="speed">The speed.</param>
        /// <param name="slow">The slow.</param>
        /// <param name="medium">The medium.</param>
        /// <param name="fast">The fast.</param>
        /// <returns>The selected speed value</returns>
        private static float PickSpeed(MotorSpeed speed, float slow, float medium, float fast)
        {
            switch (speed)
            {
                case MotorSpeed.Slow: return slow;
                case MotorSpeed.Medium: return medium;
                case MotorSpeed.Fast: return fast;
                default: throw new NotSupportedException($"Speed {speed} is not supported");
            }
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override Task Teardown()
        {
            // No disconnection tasks
            return Task.CompletedTask;
        }
    }
}
