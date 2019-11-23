// <copyright file="KnownFaceAppearedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Known face detected event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class KnownFaceAppearedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Gets the face that triggered the event
        /// </summary>
        public Face Face => (Face)Object;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaceAppearedEventArgs" /> class.
        /// </summary>
        /// <param name="face">The face.</param>
        internal KnownFaceAppearedEventArgs(Face face) : base(face)
        {
        }
    }
}
