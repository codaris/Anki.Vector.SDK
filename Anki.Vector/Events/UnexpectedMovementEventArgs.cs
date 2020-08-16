// <copyright file="UnexpectedMovementEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Anki.Vector.Events
{
    /// <summary>
    /// The side of the unexpected movement
    /// </summary>
    public enum UnexpectedMovementSide
    {
        /// <summary>The side is unknown</summary>
        Unknown = 0,
        /// <summary>The front side</summary>
        Front = 1,
        /// <summary>The back side</summary>
        Back = 2,
        /// <summary>The left side</summary>
        Left = 3,
        /// <summary>The right side</summary>
        Right = 4,
    }

    /// <summary>
    /// Unexpected movement type
    /// </summary>
    public enum UnexpectedMovementType
    {
        /// <summary>Turned but stopped</summary>
        TurnedButStopped = 0,
        /// <summary>Turned in same direction</summary>
        TurnedInSameDirection = 1,
        /// <summary>Turned in opposite direction</summary>
        TurnedInOppositeDirection = 2,
        /// <summary>Rotating without motors</summary>
        RotatingWithoutMotors = 3,
    }

    /// <summary>
    /// Unexpected movement event args
    /// </summary>
    /// <seealso cref="Anki.Vector.Events.RobotEventArgs" />
    [Serializable]
    public class UnexpectedMovementEventArgs : RobotEventArgs
    {
        /// <summary>Gets the unexpected movement side.</summary>
        public UnexpectedMovementSide UnexpectedMovementSide { get; }

        /// <summary>Gets the type of the unexpected movement.</summary>
        public UnexpectedMovementType UnexpectedMovementType { get; }

        /// <summary>Gets the timestamp.</summary>
        public uint Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedMovementEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        internal UnexpectedMovementEventArgs(ExternalInterface.Event e) : base(e)
        {
            UnexpectedMovementSide = (UnexpectedMovementSide)e.UnexpectedMovement.MovementSide;
            UnexpectedMovementType = (UnexpectedMovementType)e.UnexpectedMovement.MovementType;
            Timestamp = e.UnexpectedMovement.Timestamp;              
        }
    }
}
