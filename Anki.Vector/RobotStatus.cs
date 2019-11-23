// <copyright file="RobotStatus.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector
{
    /// <summary>
    /// Robot Status class and exposed properties for Vector’s various states.
    /// </summary>
    public struct RobotStatus : IEquatable<RobotStatus>
    {
#pragma warning disable SA1310
#pragma warning disable SA1600
        private const uint ROBOT_STATUS_IS_MOVING = 0x1;
        private const uint ROBOT_STATUS_IS_CARRYING_BLOCK = 0x2;
        private const uint ROBOT_STATUS_IS_PICKING_OR_PLACING = 0x4;
        private const uint ROBOT_STATUS_IS_PICKED_UP = 0x8;
        private const uint ROBOT_STATUS_IS_BUTTON_PRESSED = 0x10;
        private const uint ROBOT_STATUS_IS_FALLING = 0x20;
        private const uint ROBOT_STATUS_IS_ANIMATING = 0x40;
        private const uint ROBOT_STATUS_IS_PATHING = 0x80;
        private const uint ROBOT_STATUS_LIFT_IN_POS = 0x100;
        private const uint ROBOT_STATUS_HEAD_IN_POS = 0x200;
        private const uint ROBOT_STATUS_CALM_POWER_MODE = 0x400;
        private const uint ROBOT_STATUS_IS_ON_CHARGER = 0x1000;
        private const uint ROBOT_STATUS_IS_CHARGING = 0x2000;
        private const uint ROBOT_STATUS_CLIFF_DETECTED = 0x4000;
        private const uint ROBOT_STATUS_ARE_WHEELS_MOVING = 0x8000;
        private const uint ROBOT_STATUS_IS_BEING_HELD = 0x10000;
        private const uint ROBOT_STATUS_IS_MOTION_DETECTED = 0x20000;
#pragma warning restore SA1310
#pragma warning restore SA1600

        /// <summary>
        /// The robot status value
        /// </summary>
        private readonly uint status;

        /// <summary>
        /// True if Vector is currently moving any of his motors (head, arm or wheels/treads).
        /// </summary>
        public bool AreMotorsMoving => (status & ROBOT_STATUS_IS_MOVING) != 0;

        /// <summary>
        /// True if Vector is currently carrying a block.
        /// </summary>
        public bool IsCarryingBlock => (status & ROBOT_STATUS_IS_CARRYING_BLOCK) != 0;

        /// <summary>
        /// True if Vector has seen a marker and is actively heading toward it (for example his charger or cube).
        /// </summary>
        public bool IsDockingToMarker => (status & ROBOT_STATUS_IS_PICKING_OR_PLACING) != 0;

        /// <summary>
        /// True if Vector is currently picked up (in the air).
        /// </summary>
        public bool IsPickedUp => (status & ROBOT_STATUS_IS_PICKED_UP) != 0;

        /// <summary>
        /// True if Vector’s button is pressed.
        /// </summary>
        public bool IsButtonPressed => (status & ROBOT_STATUS_IS_BUTTON_PRESSED) != 0;

        /// <summary>
        /// True if Vector is currently falling.
        /// </summary>
        public bool IsFalling => (status & ROBOT_STATUS_IS_FALLING) != 0;

        /// <summary>
        /// True if Vector is currently playing an animation.
        /// </summary>
        public bool IsAnimating => (status & ROBOT_STATUS_IS_ANIMATING) != 0;

        /// <summary>
        /// True if Vector is currently traversing a path.
        /// </summary>
        public bool IsPathing => (status & ROBOT_STATUS_IS_PATHING) != 0;

        /// <summary>
        /// True if Vector’s arm is in the desired position (False if still trying to move it there).
        /// </summary>
        public bool IsLiftInPos => (status & ROBOT_STATUS_LIFT_IN_POS) != 0;

        /// <summary>
        /// True if Vector’s head is in the desired position (False if still trying to move there).
        /// </summary>
        public bool IsHeadInPos => (status & ROBOT_STATUS_HEAD_IN_POS) != 0;

        /// <summary>
        /// True if Vector is in calm power mode. Calm power mode is generally when Vector is sleeping or charging.
        /// </summary>
        public bool IsInCalmPowerMode => (status & ROBOT_STATUS_CALM_POWER_MODE) != 0;

        /// <summary>
        /// True if Vector is currently on the charger.
        /// </summary>
        public bool IsOnCharger => (status & ROBOT_STATUS_IS_ON_CHARGER) != 0;

        /// <summary>
        /// True if Vector is currently charging.
        /// </summary>
        public bool IsCharging => (status & ROBOT_STATUS_IS_CHARGING) != 0;

        /// <summary>
        /// True if Vector detected a cliff using any of his four cliff sensors.
        /// </summary>
        public bool IsCliffDetected => (status & ROBOT_STATUS_CLIFF_DETECTED) != 0;

        /// <summary>
        /// True if Vector’s wheels/treads are currently moving.
        /// </summary>
        public bool AreWheelsMoving => (status & ROBOT_STATUS_ARE_WHEELS_MOVING) != 0;

        /// <summary>
        /// True if Vector is being held.
        /// </summary>
        public bool IsBeingHeld => (status & ROBOT_STATUS_IS_BEING_HELD) != 0;

        /// <summary>
        /// True if Vector is in motion. This includes any of his motors (head, arm, wheels/tracks) and if he is being lifted, carried, or falling.
        /// </summary>
        public bool IsRobotMoving => (status & ROBOT_STATUS_IS_MOTION_DETECTED) != 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotStatus"/> struct.
        /// </summary>
        /// <param name="robotStatus">The robot status.</param>
        internal RobotStatus(uint robotStatus)
        {
            status = robotStatus;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is RobotStatus ? this.Equals((RobotStatus)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return status.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(RobotStatus other)
        {
            return status == other.status;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(RobotStatus lhs, RobotStatus rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(RobotStatus lhs, RobotStatus rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return status.ToString("X");
        }
    }
}
