// <copyright file="AsyncEventLoop.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.GrpcUtil
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Grpc.Core;

    /// <summary>
    /// Async event loop interface
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal interface IAsyncEventLoop : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the event loop is active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Starts the event loop.  The loop will run in a background thread and call the resultAction function every time a response is received
        /// from the stream.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Start();

        /// <summary>
        /// Ends the event loop.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task End();
    }

    /// <summary>
    /// Concrete implementation of the AsyncEventLoop for GRPC.  Takes an AsyncStreamingCall function and an action to process when
    /// each event comes in.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="IAsyncEventLoop" />
    internal class AsyncEventLoop<TResponse> : IDisposable, IAsyncEventLoop
    {
        /// <summary>
        /// The shutdown timeout in milliseconds.
        /// </summary>
        private const int ShutdownTimeout = 10_000;

        /// <summary>
        /// The feed start function
        /// </summary>
        private readonly Func<CancellationToken, AsyncServerStreamingCall<TResponse>> startFunction;

        /// <summary>
        /// The result action
        /// </summary>
        private readonly Action<TResponse> resultAction;

        /// <summary>
        /// The action called when the loop ends
        /// </summary>
        private readonly Action endAction;

        /// <summary>
        /// The action called when an exception occurs
        /// </summary>
        private readonly Action<Exception> exceptionHandler;

        /// <summary>
        /// The cancellation token source for terminating the feed
        /// </summary>
        private CancellationTokenSource cancellationTokenSource = null;

        /// <summary>
        /// The feed loop start task completion source
        /// </summary>
        private TaskCompletionSource<bool> startTaskCompletionSource = null;

        /// <summary>
        /// The feed loop end task completion source
        /// </summary>
        private TaskCompletionSource<bool> endTaskCompletionSource = null;

        /// <summary>
        /// Gets the last exception that terminated the event loop, if it was terminated by exception.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventLoop{TResponse}" /> class.
        /// </summary>
        /// <param name="startFunction">The function called to start the stream.</param>
        /// <param name="resultAction">The function called with each stream result.</param>
        /// <param name="endAction">The optional action called when the stream ends.</param>
        /// <param name="exceptionHandler">The optional exception handler.</param>
        public AsyncEventLoop(Func<CancellationToken, AsyncServerStreamingCall<TResponse>> startFunction, Action<TResponse> resultAction, Action endAction = null, Action<Exception> exceptionHandler = null)
        {
            this.startFunction = startFunction;
            this.resultAction = resultAction;
            this.endAction = endAction;
            this.exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Gets a value indicating whether the event loop is active.
        /// </summary>
        public bool IsActive => cancellationTokenSource != null;

        /// <summary>
        /// Starts the event loop.  The loop will run in a background thread and call the resultAction function every time a response is received
        /// from the stream.  This task will complete when the loop starts.
        /// </summary>
        /// <returns></returns>
        public Task Start()
        {
            startTaskCompletionSource?.TrySetCanceled();
            startTaskCompletionSource = new TaskCompletionSource<bool>();
            Task.Run(StartAsync);
            return startTaskCompletionSource.Task;
        }


        /// <summary>
        /// Starts the event loop asychroniously and call the resultAction function every time a response is received
        /// from the stream.  This task will complete when the loop ends.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">The event loop has already been started.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Calls exception handler action.")]
        public async Task StartAsync()
        {
            if (cancellationTokenSource != null)
            {
                throw new InvalidOperationException("The event loop has already been started.");
            }

            try
            {
                // Reset the exception
                Exception = null;

                // Create the cancellation token source
                cancellationTokenSource = new CancellationTokenSource();
                endTaskCompletionSource = new TaskCompletionSource<bool>();
                var token = cancellationTokenSource.Token;

                using (var feed = startFunction(token))
                {
                    // Trigger the start completion source
                    startTaskCompletionSource?.TrySetResult(true);
                    // While the feed has not been cancelled retrieve images
                    while (!token.IsCancellationRequested)
                    {
                        // Get a single result from the stream
                        var result = await feed.ResponseStream.MoveNext(token).ConfigureAwait(false);
                        if (!result) break;
                        var response = feed.ResponseStream.Current;
                        resultAction(response);
                    }
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                // Ignore cancellation exceptions
            }
            catch (Exception ex)
            {
                // All event loop exception are caught because exceptions don't propagate well on the background thread.
                // Instead, the exception is logged here.
                Exception = ex;
                exceptionHandler?.Invoke(ex);
                endTaskCompletionSource.TrySetException(ex);
            }
            finally
            {
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
                endTaskCompletionSource.TrySetResult(true);
                endTaskCompletionSource = null;
                endAction?.Invoke();
            }
        }

        /// <summary>
        /// Ends the event loop.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task End()
        {
            cancellationTokenSource?.Cancel();
            if (endTaskCompletionSource == null) await Task.CompletedTask.ConfigureAwait(false);
            else await endTaskCompletionSource.Task.ConfigureAwait(false);
        }

        #region IDisposable Support

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            disposedValue = true;
            if (disposing)
            {
                End().Wait(ShutdownTimeout);
                cancellationTokenSource?.Dispose();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
