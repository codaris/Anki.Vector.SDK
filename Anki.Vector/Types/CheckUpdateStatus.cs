// <copyright file="CheckUpdateStatus.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Update status enum
    /// </summary>
    public enum UpdateStatus
    {
        /// <summary>The no update to the current firmware</summary>
        NoUpdate = 0,
        /// <summary>New firmware is ready to install</summary>
        ReadyToInstall = 1,
        /// <summary>The download is in progress</summary>
        InProgressDownload = 2,
    }

    /// <summary>
    /// The result of check update status methods.
    /// </summary>
    public class CheckUpdateStatus
    {
        /// <summary>Gets the expected number of bytes to download</summary>
        public long Expected { get; }

        /// <summary>Gets the current number of bytes downloaded</summary>
        public long Progress { get; }

        /// <summary>Gets the update status.</summary>
        public UpdateStatus UpdateStatus { get; }

        /// <summary>
        /// Gets the update version.
        /// </summary>
        public string UpdateVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckUpdateStatus"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal CheckUpdateStatus(CheckUpdateStatusResponse response)
        {
            Expected = response.Expected;
            Progress = response.Progress;
            UpdateStatus = (UpdateStatus)response.UpdateStatus;
            UpdateVersion = response.UpdateVersion;
        }
    }
}
