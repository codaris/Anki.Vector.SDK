// <copyright file="StatusCode.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// Status result 
    /// </summary>
    public enum StatusCode
    {
        /// <summary>Unknown</summary>
        Unknown = 0,
        /// <summary>Response received</summary>
        ResponseReceived = 1,
        /// <summary>Request is processing</summary>
        RequestProcessing = 2,
        /// <summary>Ok</summary>
        Ok = 3,
        /// <summary>Forbidden</summary>
        Forbidden = 100,
        /// <summary>Not found</summary>
        NotFound = 101,
        /// <summary>Update is in progress</summary>
        ErrorUpdateInProgress = 102,
    }
}
