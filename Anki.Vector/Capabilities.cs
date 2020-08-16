// <copyright file="Capabilities.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Anki.Vector
{
    /// <summary>
    /// Features supported by the current robot firmware.
    /// </summary>
    public class Capabilities
    {
        /// <summary>
        /// Gets the robot instance
        /// </summary>
        private Robot Robot { get; }

        /// <summary>
        /// Gets the version 1.7 constant
        /// </summary>
        internal static Version Version1_7 { get; } = new Version(1, 7);

        /// <summary>
        /// Gets a value indicating whether high resolution image capture are supported.
        /// </summary>
        public bool HighResolutionImageCapture => Robot.FirmwareVersion >= Version1_7;

        /// <summary>
        /// Gets a value indicating whether behaviors can be cancelled.
        /// </summary>
        internal bool CancelBehaviors => Robot.FirmwareVersion >= Version1_7;

        /// <summary>
        /// Gets a value indicating whether camera settings are supported.
        /// </summary>
        public bool CameraSettings => Robot.FirmwareVersion >= Version1_7;

        /// <summary>
        /// Gets a value indicating whether SayText supports the pitch parameter.
        /// </summary>
        public bool SayTextPitch => Robot.FirmwareVersion >= Version1_7;

        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal Capabilities(Robot robot)
        {
            this.Robot = robot;
        }

        /// <summary>
        /// Asserts the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <exception cref="Exceptions.VectorInvalidVersionException">This method requires at least firmware version XXX.</exception>
        internal void Assert(Version version)
        {
            if (Robot.FirmwareVersion >= version) return;
            throw new Exceptions.VectorInvalidVersionException($"This method requires at least firmware version {version.ToString(2)}.");
        }
    }
}
