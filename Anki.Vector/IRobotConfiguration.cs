// <copyright file="IRobotConfiguration.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System.Net;

namespace Anki.Vector
{
    /// <summary>
    /// Robot configuration information
    /// </summary>
    public interface IRobotConfiguration
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        string Guid { get; }

        /// <summary>
        /// Gets the certificate text.
        /// </summary>
        string Certificate { get; }

        /// <summary>
        /// Gets the robot IP address
        /// </summary>
        IPAddress IPAddress { get; }

        /// <summary>
        /// Gets the name of the robot.  This is in the form "Vector-XXXX"
        /// </summary>
        string RobotName { get; }

        /// <summary>
        /// Gets the robot serial number.
        /// </summary>
        string SerialNumber { get; }
    }
}
