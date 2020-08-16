// <copyright file="CheckUpdateStatusEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Anki.Vector.ExternalInterface;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Check update status event args
    /// </summary>
    [Serializable]
    public class CheckUpdateStatusEventArgs : RobotEventArgs
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "Invalid")]
        public string UpdateVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckUpdateStatusEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        internal CheckUpdateStatusEventArgs(ExternalInterface.Event e) : base(e)
        {
            Expected = e.CheckUpdateStatusResponse.Expected;
            Progress = e.CheckUpdateStatusResponse.Progress;
            UpdateStatus = (UpdateStatus)e.CheckUpdateStatusResponse.UpdateStatus;
            UpdateVersion = e.CheckUpdateStatusResponse.UpdateVersion;
        }
    }
}
