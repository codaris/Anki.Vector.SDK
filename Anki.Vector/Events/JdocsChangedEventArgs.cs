// <copyright file="JdocsChangedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anki.Vector.Events
{
    /// <summary>
    /// The JDOC type that changed
    /// </summary>
    public enum JdocType
    {
        /// <summary>The robot settings </summary>
        RobotSettings = 0,
        /// <summary>The robot lifetime stats</summary>
        RobotLifetimeStats = 1,
        /// <summary>The account settings</summary>
        AccountSettings = 2,
        /// <summary>The user entitlements</summary>
        UserEntitlements = 3,
    }

    /// <summary>
    /// JDocs changes event args
    /// </summary>
    /// <seealso cref="Anki.Vector.Events.RobotEventArgs" />
    [Serializable]
    public class JdocsChangedEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the changed jdocs.
        /// </summary>
        public IReadOnlyList<JdocType> ChangedJdocs { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JdocsChangedEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal JdocsChangedEventArgs(ExternalInterface.Event e) : base(e)
        {
            // List the change jdocs
            ChangedJdocs = e.JdocsChanged.JdocTypes.Select(jdoc => (JdocType)jdoc).ToList();
        }
    }
}
