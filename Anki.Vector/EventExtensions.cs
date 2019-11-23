// <copyright file="EventExtensions.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    /// Internal extension helper methods
    /// </summary>
    internal static class EventExtensions
    {
        /// <summary>
        /// Raises an event.  If the sender implements ISyncEvents then it's synchronization context is used.  
        /// </summary>
        /// <param name="multicast">The multicast.</param>
        /// <param name="sender">The sender component.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void Raise(this MulticastDelegate multicast, RobotObject sender, EventArgs args)
        {
            multicast.Raise(sender.Robot.SyncContext, sender, args);
        }

        /// <summary>
        /// Raises an event on the specified synchronize context.
        /// </summary>
        /// <param name="multicast">The multicast.</param>
        /// <param name="syncContext">The synchronize context.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void Raise(this MulticastDelegate multicast, SynchronizationContext syncContext, object sender, EventArgs args)
        {
            foreach (Delegate del in multicast.GetInvocationList())
            {
                if (del.Target is RobotObject)
                {
                    del.DynamicInvoke(sender, args);
                }
                else if (del.Target is ISynchronizeInvoke syncTarget)
                {
                    if (syncTarget.InvokeRequired) syncTarget.BeginInvoke(del, new object[] { sender, args });
                    else del.DynamicInvoke(sender, args);
                }
                else if (syncContext != null && syncContext != SynchronizationContext.Current)
                {
                    syncContext.Post(o => del.DynamicInvoke(sender, args), null);
                }
                else
                {
                    del.DynamicInvoke(sender, args);
                }
            }
        }
    }
}
