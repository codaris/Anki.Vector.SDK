// <copyright file="FaceEnrollmentCompletedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Face enrollment result
    /// </summary>
    public enum FaceEnrollmentResult
    {
        /// <summary>Successfully enrolled face</summary>
        Success = 0,
        /// <summary>Saw the wrong face</summary>
        SawWrongFace = 1,
        /// <summary>Saw multiple faces</summary>
        SawMultipleFaces = 2,
        /// <summary>Timed out</summary>
        TimedOut = 3,
        /// <summary>Face save failed</summary>
        SaveFailed = 4,
        /// <summary>Incomplete</summary>
        Incomplete = 5,
        /// <summary>Cancelled</summary>
        Cancelled = 6,
        /// <summary>Name in use</summary>
        NameInUse = 7,
        /// <summary>Name storage file</summary>
        NameStorageFull = 8,
        /// <summary>Unknown failure</summary>
        UnknownFailure = 9,
    }

    /// <summary>
    /// Face enrollment completed event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class FaceEnrollmentCompletedEventArgs : TimestampedStatusEventArgs
    {
        /// <summary>
        /// Gets the face identifier of the detected face
        /// </summary>
        public int FaceId { get; }

        /// <summary>
        /// Gets the name associated with the face detected
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the result of the face enrollment
        /// </summary>
        public FaceEnrollmentResult Result { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaceEnrollmentCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal FaceEnrollmentCompletedEventArgs(ExternalInterface.Event e) : base(e)
        {
            var faceEnrollmentCompleted = e.TimeStampedStatus.Status.FaceEnrollmentCompleted;
            FaceId = faceEnrollmentCompleted.FaceId;
            Name = faceEnrollmentCompleted.Name;
            Result = (FaceEnrollmentResult)faceEnrollmentCompleted.Result;
        }
    }
}
