// <copyright file="AlexaAuthEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Alexa authorization state
    /// </summary>
    public enum AlexaAuthState
    {
        /// <summary>
        /// Invalid/error/versioning issue
        /// </summary>
        AlexaAuthInvalid = 0,
        /// <summary>
        /// Not opted in, or opt-in attempted but failed
        /// </summary>
        AlexaAuthUninitialized = 1,
        /// <summary>
        /// Opted in, and attempting to authorize
        /// </summary>
        AlexaAuthRequestingAuth = 2,
        /// <summary>
        /// Opted in, and waiting on the user to enter a code
        /// </summary>
        AlexaAuthWaitingForCode = 3,
        /// <summary>
        /// Opted in, and authorized / in use
        /// </summary>
        AlexaAuthAuthorized = 4,
    }

    /// <summary>
    /// Alexa authorization event args
    /// </summary>
    /// <seealso cref="Anki.Vector.Events.RobotEventArgs" />
    [Serializable]
    public class AlexaAuthEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the state of the alexa authentication.
        /// </summary>
        public AlexaAuthState AuthState { get; }

        /// <summary>
        /// Gets the extra data associated with this event.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "Invalid")]
        public string Extra { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlexaAuthEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        internal AlexaAuthEventArgs(ExternalInterface.Event e) : base(e)
        {
            AuthState = (AlexaAuthState)e.AlexaAuthEvent.AuthState;
            Extra = e.AlexaAuthEvent.Extra;
        }
    }
}
