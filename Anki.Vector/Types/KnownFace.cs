// <copyright file="KnownFace.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// A face that Vector has detected.
    /// </summary>
    public class KnownFace
    {
        /// <summary>
        /// Gets the internal ID assigned to the face.
        /// </summary>
        public int FaceId { get; }

        /// <summary>
        /// Gets the name Vector has associated with the face.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the seconds since face was first enrolled.
        /// </summary>
        public long SecondsSinceFirstEnrolled { get; }

        /// <summary>
        /// Gets the seconds since face was last seen.
        /// </summary>
        public long SecondsSinceLastSeen { get; }

        /// <summary>
        /// Gets the seconds since face was last updated.
        /// </summary>
        public long SecondsSinceLastUpdated { get; }

        /// <summary>
        /// Gets the first enrolled.
        /// </summary>
        public DateTime FirstEnrolled { get; }

        /// <summary>
        /// Gets the last seen.
        /// </summary>
        public DateTime LastSeen { get; }

        /// <summary>
        /// Gets the last updated.
        /// </summary>
        public DateTime LastUpdated { get; }

        /// <summary>
        /// Gets the last seen timestamp.
        /// </summary>
        public DateTimeOffset LastSeenTimestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFace"/> class.
        /// </summary>
        /// <param name="knownFace">The known face.</param>
        internal KnownFace(LoadedKnownFace knownFace)
        {
            FaceId = knownFace.FaceId;
            Name = knownFace.Name;
            SecondsSinceFirstEnrolled = knownFace.SecondsSinceFirstEnrolled;
            SecondsSinceLastSeen = knownFace.SecondsSinceLastSeen;
            SecondsSinceLastUpdated = knownFace.SecondsSinceLastUpdated;
            FirstEnrolled = DateTime.Now.AddSeconds(-knownFace.SecondsSinceFirstEnrolled);
            LastSeen = DateTime.Now.AddSeconds(-knownFace.SecondsSinceLastSeen);
            LastUpdated = DateTime.Now.AddSeconds(-knownFace.SecondsSinceLastUpdated);
            LastSeenTimestamp = DateTimeOffset.FromUnixTimeSeconds(knownFace.LastSeenSecondsSinceEpoch);
        }
    }
}
