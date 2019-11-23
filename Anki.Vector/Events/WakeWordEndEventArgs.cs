// <copyright file="WakeWordEndEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Wake word end event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class WakeWordEndEventArgs : WakeWordEventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the intent was heard.
        /// </summary>
        public bool IntentHeard { get; }

        /// <summary>
        /// Gets the intent JSON value.
        /// </summary>
        public string IntentJson { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WakeWordEndEventArgs" /> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal WakeWordEndEventArgs(ExternalInterface.Event e) : base(e)
        {
            IntentHeard = e.WakeWord.WakeWordEnd.IntentHeard;
            IntentJson = e.WakeWord.WakeWordEnd.IntentJson;
        }
    }
}
