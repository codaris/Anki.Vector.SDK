// <copyright file="NavMapComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector
{
    using System;
    using System.Threading.Tasks;
    using Anki.Vector.Events;
    using Anki.Vector.ExternalInterface;
    using Anki.Vector.GrpcUtil;
    using Anki.Vector.Types;

    /// <summary>
    /// Represents Vector's navigation memory map.
    /// <para>The NavMapComponent object subscribes for nav memory map updates from the robot to store and dispatch.</para>
    /// </summary>
    /// <seealso cref="Anki.Vector.Component" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Component is disposed by Teardown method.")]
    public class NavMapComponent : Component
    {
        /// <summary>
        /// The camera event loop
        /// </summary>
        private readonly IAsyncEventLoop navMapFeed;

        /// <summary>
        /// Gets the map frequency.
        /// </summary>
        public float Frequency { get; private set; } = 0.5f;

        /// <summary>
        /// Gets the latest nav map.
        /// </summary>
        public NavMapGrid LatestNavMap { get; private set; }

        /// <summary>
        /// Occurs when nav map updated
        /// </summary>
        public event EventHandler<NavMapUpdateEventArgs> NavMapUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavMapComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal NavMapComponent(Robot robot) : base(robot)
        {
            this.navMapFeed = new AsyncEventLoop<NavMapFeedResponse>(
                (token) => robot.StartStream(client => client.NavMapFeed(new NavMapFeedRequest() { Frequency = Frequency }, cancellationToken: token)),
                (response) =>
                {
                    var navMapUpdateEventArgs = new NavMapUpdateEventArgs(response);
                    LatestNavMap = navMapUpdateEventArgs.NavMap;
                    NavMapUpdate?.Invoke(this, navMapUpdateEventArgs);
                },
                () => OnPropertyChanged(nameof(IsFeedActive)),
                robot.PropagateException
             );
        }

        /// <summary>
        /// Gets a value indicating whether the nav map feed is active.
        /// </summary>
        public bool IsFeedActive => navMapFeed.IsActive;

        /// <summary>
        /// Starts the nav map feed.  The feed will run in a background thread and raise the <see cref="LatestNavMap" /> event for each map change.  It will
        /// also update the <see cref="LatestNavMap" /> property.
        /// </summary>
        /// <param name="frequency">The navmap polling frequency.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StartFeed(float frequency = 0.5f)
        {
            Frequency = frequency;
            await navMapFeed.Start().ConfigureAwait(false);
            OnPropertyChanged(nameof(IsFeedActive));
        }

        /// <summary>
        /// Stops the nav map feed.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StopFeed()
        {
            await navMapFeed.End().ConfigureAwait(false);
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <param name="forced">if set to <c>true</c> the shutdown is forced due to lost connection.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        internal override Task Teardown(bool forced)
        {
            return StopFeed();
        }
    }
}
