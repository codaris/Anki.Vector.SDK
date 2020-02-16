using System;
using System.Collections.Generic;
using System.Text;

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
        /// <param name="statusCode">The status code.</param>
        /// <returns>Converted result</returns>
        internal static AttentionTransferReason Convert(this ExternalInterface.AttentionTransferReason attentionTransferReason)
        {
            return (AttentionTransferReason)attentionTransferReason;
        }
    }
}
