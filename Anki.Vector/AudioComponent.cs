// <copyright file="AudioComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Threading.Tasks;
    using Anki.Vector.Events;
    using Anki.Vector.Exceptions;
    using Anki.Vector.ExternalInterface;
    using Anki.Vector.GrpcUtil;
    using Anki.Vector.Types;

    /// <summary>
    /// Use these values for setting the master audio volume.  See <see cref="AudioComponent.SetMasterVolume"/>.
    /// </summary>
    public enum MasterVolumeLevel
    {
        /// <summary>Lowest volume level</summary>
        Low = 0,
        /// <summary>Medium low volume level</summary>
        MediumLow = 1,
        /// <summary>Medium volume level</summary>
        Medium = 2,
        /// <summary>Medium high volume level</summary>
        MediumHigh = 3,
        /// <summary>Highest volume level</summary>
        High = 4
    }

    /// <summary>
    /// The playback result
    /// </summary>
    public enum PlaybackResult
    {
        /// <summary>The playback completed successfully</summary>
        Completed,
        /// <summary>The robot overran the audio buffer\</summary>
        BufferOverrun,
        /// <summary>An unspecified playback failure</summary>
        OtherFailure,
        /// <summary>The playback was cancelled</summary>
        Cancelled
    }


    /// <summary>
    /// Support for Vector’s speakers
    /// <para>Vector's speakers can be used for playing user-provided audio.  You can use the <see cref="PlayStream(Stream, uint, uint)"/> method to play a stream of 
    /// 16bit mono audio data.</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Component is disposed by Teardown method.")]
    public class AudioComponent : Component
    {
        /// <summary>
        /// The maximum robot audio chunk size
        /// </summary>
        public const int MaxRobotAudioChunkSize = 1024;

        /// <summary>
        /// The audio feed loop
        /// </summary>
        private readonly IAsyncEventLoop audioFeed;

        /// <summary>
        /// The playback feed
        /// </summary>
        private readonly IAsyncDuplexEventLoop<ExternalAudioStreamRequest> playbackFeed;

        /// <summary>
        /// The playback result
        /// </summary>
        private TaskCompletionSource<PlaybackResult> playbackResult = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal AudioComponent(Robot robot) : base(robot)
        {
            this.audioFeed = new AsyncEventLoop<AudioFeedResponse>(
                (token) => robot.StartStream(client => client.AudioFeed(new AudioFeedRequest(), cancellationToken: token)),
                (response) => AudioReceived?.Invoke(this, new AudioReceivedEventArgs(response)),
                () => OnPropertyChanged(nameof(IsAudioFeedActive)),
                robot.PropagateException
            );
            this.playbackFeed = new AsyncDuplexEventLoop<ExternalAudioStreamRequest, ExternalAudioStreamResponse>(
                (token) => Robot.StartStream(client => client.ExternalAudioStreamPlayback(cancellationToken: token)),
                ProcessAudioResponse,
                () => { OnPropertyChanged(nameof(IsPlaybackActive)); playbackResult?.SetResult(PlaybackResult.Completed); },
                Robot.PropagateException
            );
        }

        /// <summary>
        /// Occurs when audio received.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("The audio feed on Vector was never enabled", true)]
        public event EventHandler<AudioReceivedEventArgs> AudioReceived;

        /// <summary>
        /// Gets a value indicating whether the audio feed is active.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsAudioFeedActive => audioFeed.IsActive;

        /// <summary>
        /// Gets a value indicating whether vector is currently playing back audio
        /// </summary>
        public bool IsPlaybackActive => playbackFeed.IsActive;

        /// <summary>
        /// Starts the audio feed.  The feed will run in a background thread and raise the <see cref="AudioReceived" /> event for each received image. 
        /// </summary>
        [Obsolete("The audio feed on Vector was never enabled", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task StartAudioFeed()
        {
            await audioFeed.Start().ConfigureAwait(false); 
            OnPropertyChanged(nameof(IsAudioFeedActive));
        }

        /// <summary>
        /// Ends the audio feed.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task EndAudioFeed()
        {
            await audioFeed.End().ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the master volume.
        /// <para>Note that muting the robot is not supported from the SDK.</para>
        /// </summary>
        /// <param name="volumeLevel">The volume level.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> SetMasterVolume(MasterVolumeLevel volumeLevel)
        {
            var response = await Robot.RunMethod(client => client.SetMasterVolumeAsync(new MasterVolumeRequest()
            {
                VolumeLevel = (ExternalInterface.MasterVolumeLevel)volumeLevel
            })).ConfigureAwait(false);
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Plays audio using Vector's speakers.
        /// </summary>
        /// <param name="stream">16bit audio byte stream containing 1 channel.</param>
        /// <param name="frameRate">The frame rate between 8000-16025 hz.</param>
        /// <param name="volume">The audio playback volume level (0-100).</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<PlaybackResult> PlayStream(Stream stream, uint frameRate, uint volume = 50)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (frameRate < 8000 || frameRate > 16025) throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 100");
            if (volume > 100) throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 100");

            playbackResult?.TrySetCanceled();
            playbackResult = new TaskCompletionSource<PlaybackResult>();

            // Send preparation message
            await playbackFeed.Call(new ExternalAudioStreamRequest()
            {
                AudioStreamPrepare = new ExternalAudioStreamPrepare()
                {
                    AudioFrameRate = frameRate,
                    AudioVolume = volume
                }
            }).ConfigureAwait(false);

            var startTime = DateTime.Now;
            int dataCount = 0;

            var chunkBuffer = new byte[MaxRobotAudioChunkSize];
            while (playbackFeed.IsActive)
            {
                int chunkSize = await stream.ReadAsync(chunkBuffer, 0, MaxRobotAudioChunkSize).ConfigureAwait(false);
                if (chunkSize == 0) break;
                dataCount += chunkSize;

                await playbackFeed.Call(new ExternalAudioStreamRequest()
                {
                    AudioStreamChunk = new ExternalAudioStreamChunk()
                    {
                        AudioChunkSizeBytes = (uint)chunkSize,
                        AudioChunkSamples = Google.Protobuf.ByteString.CopyFrom(chunkBuffer, 0, chunkSize)
                    }
                }).ConfigureAwait(false);
                if (chunkSize != MaxRobotAudioChunkSize) break;

                var elapsed = DateTime.Now.Subtract(startTime).TotalSeconds;
                var expectedDataCount = elapsed * frameRate * 2;
                var timeAhead = (dataCount - expectedDataCount) / frameRate;
                if (timeAhead > 1.0)
                {
                    await Task.Delay((int)((timeAhead - 0.5) * 1000)).ConfigureAwait(false);
                }
            }

            if (playbackFeed.IsActive)
            {
                await playbackFeed.Call(new ExternalAudioStreamRequest()
                {
                    AudioStreamComplete = new ExternalAudioStreamComplete()
                }).ConfigureAwait(false);
            }

            return await playbackResult.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels the audio playback
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CancelPlayback()
        {
            if (!IsPlaybackActive) return;
            await playbackFeed.Call(new ExternalAudioStreamRequest()
            {
                AudioStreamCancel = new ExternalAudioStreamCancel()
            }).ConfigureAwait(false);
            await playbackFeed.End().ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the audio response.
        /// </summary>
        /// <param name="response">The response.</param>
        private void ProcessAudioResponse(ExternalAudioStreamResponse response)
        {
            var type = response.AudioResponseTypeCase;
            switch (type)
            {
                case ExternalAudioStreamResponse.AudioResponseTypeOneofCase.AudioStreamPlaybackComplete:
                    playbackResult?.TrySetResult(PlaybackResult.Completed);
                    break;
                case ExternalAudioStreamResponse.AudioResponseTypeOneofCase.None:
                    return;
                case ExternalAudioStreamResponse.AudioResponseTypeOneofCase.AudioStreamBufferOverrun:
                    playbackResult?.TrySetResult(PlaybackResult.BufferOverrun);
                    throw new VectorAudioPlaybackException("Audio stream buffer overrun");
                case ExternalAudioStreamResponse.AudioResponseTypeOneofCase.AudioStreamPlaybackFailyer:
                    playbackResult?.TrySetResult(PlaybackResult.OtherFailure);
                    throw new VectorAudioPlaybackException("Audio stream failure");
            }
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override Task Teardown()
        {
            return Task.WhenAll(audioFeed.End(), playbackFeed.End());
        }
    }
}