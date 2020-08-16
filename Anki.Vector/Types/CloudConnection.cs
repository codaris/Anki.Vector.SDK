// <copyright file="CloudConnection.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Cloud connection code
    /// </summary>
    public enum CloudConnectionCode
    {
        /// <summary>Unknown</summary>
        Unknown = 0,
        /// <summary>Available</summary>
        Available = 1,
        /// <summary>Bad connectivity</summary>
        BadConnectivity = 2,
        /// <summary>Failed TLS</summary>
        FailedTls = 3,
        /// <summary>Failed Authorization</summary>
        FailedAuth = 4,
        /// <summary>Insufficient bandwidth</summary>
        InsufficientBandwidth = 5,
    }

    /// <summary>
    /// Cloud connection information
    /// </summary>
    public class CloudConnection
    {
        /// <summary>
        /// Gets the cloud connection code.
        /// </summary>
        public CloudConnectionCode Code { get; }

        /// <summary>
        /// Gets the expected packets.
        /// </summary>
        public int ExpectedPackets { get; }

        /// <summary>
        /// Gets the number packets.
        /// </summary>
        public int NumPackets { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudConnection"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal CloudConnection(CheckCloudResponse response)
        {
            Code = (CloudConnectionCode)response.Code;
            ExpectedPackets = response.ExpectedPackets;
            NumPackets = response.NumPackets;
        }
    }
}
