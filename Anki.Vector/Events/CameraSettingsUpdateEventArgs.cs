// <copyright file="CameraSettingsUpdateEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Camera settings update event args
    /// </summary>
    /// <seealso cref="Anki.Vector.Events.RobotEventArgs" />
    [Serializable]
    public class CameraSettingsUpdateEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets a value indicating whether automatic exposure enabled.
        /// </summary>
        public bool AutoExposureEnabled { get; }

        /// <summary>
        /// Gets the exposure in milliseconds.
        /// </summary>
        public uint ExposureMs { get; }

        /// <summary>
        /// Gets the gain.
        /// </summary>
        public float Gain { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraSettingsUpdateEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        internal CameraSettingsUpdateEventArgs(ExternalInterface.Event e) : base(e)
        {
            AutoExposureEnabled = e.CameraSettingsUpdate.AutoExposureEnabled;
            ExposureMs = e.CameraSettingsUpdate.ExposureMs;
            Gain = e.CameraSettingsUpdate.Gain;
        }
    }
}
