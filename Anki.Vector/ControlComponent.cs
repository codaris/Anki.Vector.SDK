// <copyright file="ControlComponent.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Anki.Vector.Events;
using Anki.Vector.Exceptions;
using Anki.Vector.ExternalInterface;
using Anki.Vector.GrpcUtil;

namespace Anki.Vector
{
    /// <summary>
    /// Management of the control of Vector's behaviors.
    /// <para>When you connect to Vector with this SDK he will behave normally.  Taking control of Vector will disable
    /// his internal behaviors (driving around autonomously, responding to voice commands, etc).  You are required to
    /// take control of Vector in order to execute commands that move Vector or provide other output.</para>
    /// <para>Call the <see cref="RequestControl(int)"/> method to request control of Vector and <see cref="ReleaseControl"/> to 
    /// release control.  Unless you use priority level <see cref="ControlPriority.OverrideBehaviors"/> Vector will regain
    /// control automatically if he senses an edge or runs low on battery.  You can subscribe to the <see cref="ControlGranted"/> and 
    /// <see cref="ControlLost"/> events to get feedback on when you have acquired or lost control.</para>
    /// </summary>
    /// <seealso cref="Anki.Vector.Component" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Component is disposed by Teardown method.")]
    public class ControlComponent : Component
    {
        /// <summary>
        /// Occurs when control of Vector is granted.
        /// </summary>
        public event EventHandler<ControlGrantedEventArgs> ControlGranted;

        /// <summary>
        /// Occurs when control of Vector is lost.
        /// </summary>
        public event EventHandler<ControlLostEventArgs> ControlLost;

        /// <summary>
        /// Gets a value indicating whether SDK has control over Vector
        /// </summary>
        public bool HasControl { get => _hasControl; private set => SetProperty(ref _hasControl, value); }
        private bool _hasControl;

        /// <summary>
        /// Gets a value indicating whether SDK will attempt to acquire control automatically when a behavior method is called after
        /// losing control to higher priority behaviors such as returning home to charge a low battery.  
        /// </summary>
        public bool MaintainBehaviorControl { get => _maintainBehaviorControl; private set => SetProperty(ref _maintainBehaviorControl, value); }
        private bool _maintainBehaviorControl;

        /// <summary>
        /// Gets the last requested control priority.
        /// </summary>
        public ControlPriority ControlPriority { get => _controlPriority; private set => SetProperty(ref _controlPriority, value); }
        private ControlPriority _controlPriority = ControlPriority.Default;

        /// <summary>
        /// The behavior feed
        /// </summary>
        private readonly IAsyncDuplexEventLoop<BehaviorControlRequest> behaviorFeed;

        /// <summary>
        /// The behavior result
        /// </summary>
        private TaskCompletionSource<bool> behaviorResult = null;

        /// <summary>
        /// The connection timeout in milliseconds
        /// </summary>
        private const int DefaultControlTimeout = 10_000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlComponent"/> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal ControlComponent(Robot robot) : base(robot)
        {
            // Create the behavior feed event loop
            this.behaviorFeed = new AsyncDuplexEventLoop<BehaviorControlRequest, BehaviorControlResponse>(
                (token) => robot.StartStream(client => client.BehaviorControl(cancellationToken: token)),
                ProcessControlResponse,
                () => {
                    behaviorResult?.TrySetResult(false);
                    HasControl = false;
                },
                robot.PropagateException
            );
        }

        /// <summary>
        /// Requests the control of Vector.
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result indicated whether control was granted.</returns>
        public Task<bool> RequestControl(int timeout = DefaultControlTimeout)
        {
            return RequestControl(ControlPriority.Default, timeout);
        }

        /// <summary>
        /// Requests the control.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result indicated whether control was granted.</returns>
        /// <exception cref="VectorControlTimeoutException">Surpassed control timeout</exception>
        public async Task<bool> RequestControl(ControlPriority priority, int timeout = DefaultControlTimeout)
        {
            // If a behavior result already exists, set it.
            MaintainBehaviorControl = priority != ControlPriority.ReserveControl;
            if (HasControl && ControlPriority == priority) return true;
            ControlPriority = priority;
            behaviorResult?.TrySetCanceled();
            behaviorResult = new TaskCompletionSource<bool>();
            await behaviorFeed.Call(new BehaviorControlRequest()
            {
                ControlRequest = new ControlRequest()
                {
                    Priority = (ControlRequest.Types.Priority)priority
                }
            }).ConfigureAwait(false);

            // If timeout, wait for timeout
            if (timeout != Timeout.Infinite)
            {
                // Otherwise wait for result and timeout
                var delay = Task.Delay(timeout);
                if (await Task.WhenAny(behaviorResult.Task, delay).ConfigureAwait(false) == delay)
                {
                    throw new VectorControlTimeoutException($"Surpassed control timeout of {timeout}ms");
                }
            }
            return await behaviorResult.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Releases the control of Vector
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result indicated whether control was released.</returns>
        public Task<bool> ReleaseControl()
        {
            MaintainBehaviorControl = false;
            return InternalReleaseControl();
        }

        /// <summary>
        /// Releases the control of Vector without setting the MaintainBehaviorControl flag.  Used by internal methods to release
        /// control.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result indicated whether control was released.</returns>
        internal async Task<bool> InternalReleaseControl()
        {
            if (!behaviorFeed.IsActive) return true;
            if (!HasControl) return true;
            await behaviorFeed.Call(new BehaviorControlRequest()
            {
                ControlRelease = new ControlRelease()
            }).ConfigureAwait(false);
            HasControl = false;
            return true;
        }

        /// <summary>
        /// Waits for control to change.
        /// <para>The task will complete when control has changed and the result is true if SDK has control over Vector.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains whether not SDK has control.</returns>
        public Task<bool> WaitForControlChange()
        {
            if (behaviorResult == null) behaviorResult = new TaskCompletionSource<bool>();
            return behaviorResult.Task;
        }

        /// <summary>
        /// Processes the control response.
        /// </summary>
        /// <param name="response">The response.</param>
        private void ProcessControlResponse(BehaviorControlResponse response)
        {
            var type = response.ResponseTypeCase;
            switch (type)
            {
                case BehaviorControlResponse.ResponseTypeOneofCase.ControlGrantedResponse:
                case BehaviorControlResponse.ResponseTypeOneofCase.ControlLostEvent:
                    HasControl = type == BehaviorControlResponse.ResponseTypeOneofCase.ControlGrantedResponse;
                    behaviorResult?.TrySetResult(HasControl);
                    if (type == BehaviorControlResponse.ResponseTypeOneofCase.ControlGrantedResponse) ControlGranted?.Invoke(this, new ControlGrantedEventArgs());
                    if (type == BehaviorControlResponse.ResponseTypeOneofCase.ControlLostEvent) ControlLost?.Invoke(this, new ControlLostEventArgs());
                    break;
                case BehaviorControlResponse.ResponseTypeOneofCase.ReservedControlLostEvent:
                    HasControl = false;
                    behaviorResult?.TrySetResult(false);
                    ControlLost?.Invoke(this, new ControlLostEventArgs());
                    break;
            }
        }

        /// <summary>
        /// Called when disconnecting Robot
        /// </summary>
        /// <param name="forced">if set to <c>true</c> the shutdown is forced due to lost connection.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override async Task Teardown(bool forced)
        {
            await behaviorFeed.End().ConfigureAwait(false);
        }
    }
}
