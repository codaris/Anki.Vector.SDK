// <copyright file="RobotErasedEnrolledFaceEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Robot erased enrolled face event args
    /// </summary>
    /// <seealso cref="Anki.Vector.Events.RobotEventArgs" />
    [Serializable]
    public class RobotErasedEnrolledFaceEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the face identifier.
        /// </summary>
        public int FaceId { get; }

        /// <summary>
        /// Gets the face name.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "Invalid")]
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotErasedEnrolledFaceEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        internal RobotErasedEnrolledFaceEventArgs(ExternalInterface.Event e) : base(e)
        {
            FaceId = e.RobotErasedEnrolledFace.FaceId;
            Name = e.RobotErasedEnrolledFace.Name;
        }
    }
}
