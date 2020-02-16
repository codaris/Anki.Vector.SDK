// <copyright file="StatusCode.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Exceptions;

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

    public static class StatusCodeExtensions
    {
        /// <summary>
        /// Ensures that the status code was successful.  If not successful, throws error code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <exception cref="VectorRequestException">Received status code of {statusCode}</exception>
        public static void EnsureSuccess(this StatusCode statusCode)
        {
            // If successful status codes, do nothing
            if (statusCode == StatusCode.Ok) return;
            if (statusCode == StatusCode.ResponseReceived) return;
            if (statusCode == StatusCode.RequestProcessing) return;
            // Throw exception
            throw new VectorRequestException(statusCode);
        }

        /// <summary>
        /// Ensures that the status code was successful.  If not successful, throws error code.
        /// </summary>
        /// <param name="responseStatus">The response status.</param>
        internal static void EnsureSuccess(this ExternalInterface.ResponseStatus responseStatus)
        {
            if (responseStatus == null) return;
            EnsureSuccess(responseStatus.Code);
        }

        /// <summary>
        /// Ensures that the status code was successful.  If not successful, throws error code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        internal static void EnsureSuccess(this ExternalInterface.ResponseStatus.Types.StatusCode statusCode)
        {
            EnsureSuccess(statusCode.Convert());
        }

        /// <summary>
        /// Converts from Robot status code to SDK status code
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>Converted result</returns>
        internal static StatusCode Convert(this ExternalInterface.ResponseStatus.Types.StatusCode statusCode)
        {
            return (StatusCode)statusCode;
        }
    }
}
