// <copyright file="ObservableObject.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using Anki.Vector.Types;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Represents any object Vector can see in the world.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Cancellation token source disposed by timer.")]
    public abstract class ObservableObject : RobotObject
    {
        /// <summary>
        /// Gets a value indicating whether the element has been observed recently.
        /// </summary>
        public bool IsVisible { get => _isVisible; protected set => SetProperty(ref _isVisible, value); }
        private bool _isVisible;

        /// <summary>
        /// Gets the location in 2d camera space where this object was last seen.
        /// </summary>
        public ImageRect LastImageRect { get => _lastImageRect; protected set => SetProperty(ref _lastImageRect, value); }
        private ImageRect _lastImageRect;

        /// <summary>
        /// Gets the time this object was last seen 
        /// </summary>
        public DateTime LastObservedTime { get => _lastObservedTime; protected set => SetProperty(ref _lastObservedTime, value); }
        private DateTime _lastObservedTime;

        /// <summary>
        /// Gets the time this object was last seen according to Vector’s time.
        /// </summary>
        public uint LastObservedTimestamp { get => _lastObservedTimestamp; protected set => SetProperty(ref _lastObservedTimestamp, value); }
        private uint _lastObservedTimestamp;

        /// <summary>
        /// Gets the pose of this object in the world.
        /// </summary>
        public Pose Pose { get => _pose; protected set => SetProperty(ref _pose, value); }
        private Pose _pose;

        /// <summary>
        /// Gets or sets the time of the last event
        /// </summary>
        public DateTime LastEventTime { get => _lastEventTime; protected set => SetProperty(ref _lastEventTime, value); }
        private DateTime _lastEventTime;

        /// <summary>
        /// Gets the cancellation token source for terminating the disappear event
        /// </summary>
        private CancellationTokenSource cancellationTokenSource = null;

        /// <summary>
        /// Gets the name of the object type.
        /// </summary>
        public abstract string ObjectTypeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObject" /> class.
        /// </summary>
        internal ObservableObject()
        {
        }

        /// <summary>
        /// Dispatches the disappear event.  Cancels the previous iteration each time it is called.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task DispatchDisappearEvent(Action<ObservableObject> handler, int timeout)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            if (token.IsCancellationRequested) return;
            await Task.Delay(timeout, token).ConfigureAwait(false);
            if (token.IsCancellationRequested) return;
            IsVisible = false;
            handler(this);
        }
    }
}
