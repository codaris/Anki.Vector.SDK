// <copyright file="AttentionTransferReason.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Anki.Vector.Types
{
    /// <summary>
    /// The reason for the last attention transfer
    /// </summary>
    public enum AttentionTransferReason
    {
        Invalid = 0,
        NoCloudConnection = 1,
        NoWifi = 2,
        UnmatchedIntent = 3,
    }

    /// <summary>
    /// Extension methods for attention transfer reason enum
    /// </summary>
    internal static class AttentionTransferReasonExtensions
    {
        /// <summary>
        /// Converts from Robot attention transfer reason to SDK attention transfer reason
        /// </summary>
        /// <param name="attentionTransferReason">The attention transfer reason.</param>
        /// <returns>Converted result</returns>
        internal static AttentionTransferReason Convert(this ExternalInterface.AttentionTransferReason attentionTransferReason)
        {
            return (AttentionTransferReason)attentionTransferReason;
        }
    }
}
