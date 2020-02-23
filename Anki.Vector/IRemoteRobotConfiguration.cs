// <copyright file="IRemoteRobotConfiguration.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System.Net;

namespace Anki.Vector
{
    /// <summary>
    /// Robot configuration information for remote (over the Internet) connections.  This structure does not include IP address, instead it has a <see cref="RemoteHost"/>
    /// property which can contain an IP address or hostname and optionally include the port to connect with.
    /// </summary>
    public interface IRemoteRobotConfiguration
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
        /// Gets the remote robot host and port.  This can be an IP address (196.168.5.123) or host name (home.whatever.com") optionally followed by a colon and 
        /// the port to connect to (196.168.5.123:4545 or home.whatever.com:4444)
        /// </summary>
        string RemoteHost { get; }

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
