// <copyright file="Pose.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Represents where an object is in the world.
    /// <para>Whenever Vector is delocalized (i.e.whenever Vector no longer knows where he is - e.g.when he's picked up), Vector creates a new pose starting at
    /// (0,0,0) with no rotation, with origin_id incremented to show that these poses cannot be compared with earlier ones. As Vector drives around, his 
    /// pose (and the pose of other objects he observes - e.g.faces, his LightCube, charger, etc.) is relative to this initial position and orientation.</para>
    /// <para>The coordinate space is relative to Vector, where Vector's origin is the  point on the ground between Vector's two front wheels. The X axis is 
    /// Vector's forward direction, the Y axis is to Vector's left, and the Z axis is up.</para>
    /// <para>Only poses of the same origin can safely be compared or operated on.</para>
    /// </summary>
    public class Pose : IEquatable<Pose>
    {
        /// <summary>
        /// Gets the position component of this pose.
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Gets the rotation component of this pose.
        /// </summary>
        public Quaternion Rotation { get; }

        /// <summary>
        /// Gets the origin identifier.
        /// </summary>
        public uint? OriginId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pose"/> class.
        /// </summary>
        public Pose()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pose"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="originId">The origin identifier.</param>
        public Pose(Position position, Quaternion rotation, uint? originId)
        {
            Position = position;
            Rotation = rotation;
            this.OriginId = originId;
        }

        /// <summary>
        /// Returns true if this Pose is valid.
        /// </summary>
        public bool IsValid => OriginId.HasValue;

        /// <summary>
        /// Checks whether these two poses are comparable.  Poses are comparable if they have the same origin.
        /// </summary>
        /// <param name="other">The other pose to compare against.</param>
        /// <returns>
        ///   <c>true</c> if the other pose is comparable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsComparable(Pose other)
        {
            if (other is null) return false;
            if (!this.OriginId.HasValue) return false;
            if (!other.OriginId.HasValue) return false;
            return this.OriginId.Value == other.OriginId.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pose"/> class.
        /// </summary>
        /// <param name="poseStruct">The pose structure.</param>
        internal Pose(PoseStruct poseStruct)
        {
            Position = new Position(poseStruct.X, poseStruct.Y, poseStruct.Z);
            Rotation = new Quaternion(poseStruct.Q0, poseStruct.Q1, poseStruct.Q2, poseStruct.Q3);
            OriginId = poseStruct.OriginId;
        }

        /// <summary>
        /// Creates a new pose such that newPose's origin is now at the location of this pose.
        /// </summary>
        /// <param name="newPose">The pose which origin is being changed.</param>
        /// <returns>A new pose relative to this pose</returns>
        public Pose RelativeToThis(Pose newPose)
        {
            if (newPose == null) throw new ArgumentNullException(nameof(newPose));
            var cosAngle = Math.Cos(Rotation.AngleZ);
            var sinAngle = Math.Sin(Rotation.AngleZ);
            var x = Position.X + (cosAngle * newPose.Position.X) - (sinAngle * newPose.Position.Y);
            var y = Position.Y + (sinAngle * newPose.Position.X) + (cosAngle * newPose.Position.Y);
            var z = Position.Z + newPose.Position.Z;
            var angle = Rotation.AngleZ + newPose.Rotation.AngleZ;
            return new Pose(new Position((float)x, (float)y, (float)z), new Quaternion(angle), newPose.OriginId);
        }

        /// <summary>
        /// Creates a new pose such that the origin ID of newPose is the same as the origin of this.
        /// </summary>
        /// <param name="newPose">The pose which originId is being changed.</param>
        /// <returns>A new pose in the same space as this</returns>
        internal Pose LocalizeToThis(Pose newPose)
        {
            return new Pose(newPose.Position, newPose.Rotation, OriginId);
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
            return obj is Pose ? this.Equals((Pose)obj) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Rotation.GetHashCode() ^ OriginId.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Pose other)
        {
            if (other is null) return false;
            return (Position == other.Position) && (Rotation == other.Rotation) && (OriginId == other.OriginId);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Pose lhs, Pose rhs)
        {
            if (lhs is null) return false;
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
        public static bool operator !=(Pose lhs, Pose rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Converts to <see cref="PoseStruct" />.
        /// </summary>
        /// <returns>
        /// The Robot <see cref="PoseStruct" /> for this instance
        /// </returns>
        internal PoseStruct ToPoseStruct()
        {
            var result = new PoseStruct
            {
                X = Position.X,
                Y = Position.Y,
                Z = Position.Z,
                Q0 = Rotation.Q0,
                Q1 = Rotation.Q1,
                Q2 = Rotation.Q2,
                Q3 = Rotation.Q3,
                OriginId = OriginId.Value
            };
            return result;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"({Position}, {Rotation})";
        }
    }
}
