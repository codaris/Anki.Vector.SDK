// <copyright file="VersionState.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// The versioning information for Vector.
    /// </summary>
    public class VersionState
    {
        /// <summary>
        /// Gets the engine build identifier.
        /// </summary>
        public string EngineBuildId { get; }

        /// <summary>
        /// Gets the OS version.
        /// </summary>
        public string OsVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionState"/> class.
        /// </summary>
        /// <param name="versionStateResponse">The version state response.</param>
        internal VersionState(VersionStateResponse versionStateResponse)
        {
            EngineBuildId = versionStateResponse.EngineBuildId;
            OsVersion = versionStateResponse.OsVersion;
        }
    }
}
