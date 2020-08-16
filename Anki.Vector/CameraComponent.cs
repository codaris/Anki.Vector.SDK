// <copyright file="CameraComponent.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Anki.Vector.Events;
    using Anki.Vector.ExternalInterface;
    using Anki.Vector.GrpcUtil;
    using Anki.Vector.Types;

    /// <summary>
    /// Support for Vector’s camera.
    /// <para>Vector has a built-in camera which he uses to observe the world around him.  You can start receiving camera images from Vector by calling the <see cref="StartFeed"/> method.  
    /// The <see cref="ImageReceived"/> event will be raised for each frame received.  Although there is an <see cref="ImageEncoding"/> property, the data received from Vector is always in
    /// the color JPEG format.</para>
    /// <para>The camera resolution is 1280 x 720 with a field of view of 90 deg (H) x 50 deg (V).</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Component is disposed by Teardown method.")]
    public class CameraComponent : Component
    {
        /// <summary>
        /// The camera event loop
        /// </summary>
        private readonly IAsyncEventLoop cameraFeed;

        /// <summary>
        /// Gets the latest image from the camera feed.
        /// </summary>
        public Image LatestImage { get => _latestImage; private set => SetProperty(ref _latestImage, value); }
        private Image _latestImage = null;

        /// <summary>
        /// Gets the configuration and calibration of the camera.  
        /// </summary>
        /// <remarks>Camera configuration is only populated with firmware version 1.7 or greater.  See <see cref="Capabilities.CameraSettings"/>.</remarks>
        public CameraConfig Config { get; private set; }

        /// <summary>
        /// Gets a value indicating whether automatic exposure is enabled.
        /// <para>If auto exposure is enabled the <see cref="Gain"/> and <see cref="ExposureMs"/> values will constantly be updated by Vector.</para>
        /// </summary>
        public bool AutoExposureEnabled { get; private set; } = true;

        /// <summary>
        /// Gets the current camera gain setting.
        /// </summary>
        /// <remarks>Gain is only populated with firmware version 1.7 or greater.  See <see cref="Capabilities.CameraSettings"/>.</remarks>
        public float Gain { get; private set; }

        /// <summary>
        /// Gets the current camera exposure setting in milliseconds.
        /// </summary>
        /// <remarks>Exposure is only populated with firmware version 1.7 or greater.  See <see cref="Capabilities.CameraSettings"/>.</remarks>
        public uint ExposureMs { get; private set; }

        /// <summary>
        /// Occurs when camera feed event.
        /// </summary>
        public event EventHandler<ImageReceivedEventArgs> ImageReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal CameraComponent(Robot robot) : base(robot)
        {
            this.cameraFeed = new AsyncEventLoop<CameraFeedResponse>(
                (token) => robot.StartStream(client => client.CameraFeed(new CameraFeedRequest(), cancellationToken: token)),
                (response) =>
                {
                    var imageReceivedEventArgs = new ImageReceivedEventArgs(response);
                    LatestImage = imageReceivedEventArgs.Image;
                    ImageReceived?.Invoke(this, imageReceivedEventArgs);
                },
                () => OnPropertyChanged(nameof(IsFeedActive)),
                robot.PropagateException
            );

            robot.Connected += Robot_Connected;
            robot.Disconnected += Robot_Disconnected;
            robot.Events.CameraSettingsUpdate += Events_CameraSettingsUpdate;
        }

        /// <summary>
        /// Handles the Connected event of the Robot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ConnectedEventArgs"/> instance containing the event data.</param>
        private async void Robot_Connected(object sender, ConnectedEventArgs e)
        {
            if (Robot.Capabilities.CameraSettings)
            {
                Config = await ReadCameraConfig().ConfigureAwait(false);
                OnPropertyChanged(nameof(Config));
            }
        }

        /// <summary>
        /// Handles the Disconnected event of the Robot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DisconnectedEventArgs"/> instance containing the event data.</param>
        private void Robot_Disconnected(object sender, DisconnectedEventArgs e)
        {
            Config = null;
            OnPropertyChanged(nameof(Config));
        }

        /// <summary>
        /// Handles the CameraSettingsUpdate event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CameraSettingsUpdateEventArgs"/> instance containing the event data.</param>
        private void Events_CameraSettingsUpdate(object sender, CameraSettingsUpdateEventArgs e)
        {
            AutoExposureEnabled = e.AutoExposureEnabled;
            ExposureMs = e.ExposureMs;
            Gain = e.Gain;
            OnPropertyChanged(nameof(AutoExposureEnabled));
            OnPropertyChanged(nameof(ExposureMs));
            OnPropertyChanged(nameof(Gain));
        }

        /// <summary>
        /// Gets a value indicating whether the camera feed is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the camera feed is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsFeedActive => cameraFeed.IsActive;

        /// <summary>
        /// The needs stream enabled
        /// </summary>
        private bool needsStreamEnabled = false;

        /// <summary>
        /// Starts the camera feed.  The feed will run in a background thread and raise the <see cref="ImageReceived" /> event for each received image.  It will
        /// also update the <see cref="LatestImage"/> property.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StartFeed()
        {
            if (!await IsImageStreamingEnabled().ConfigureAwait(false)) needsStreamEnabled = true;
            await cameraFeed.Start().ConfigureAwait(false);
            OnPropertyChanged(nameof(IsFeedActive));
        }

        /// <summary>
        /// Stops the camera feed.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StopFeed()
        {
            needsStreamEnabled = false;
            await cameraFeed.End().ConfigureAwait(false);
        }

        /// <summary>
        /// Request to capture a single image from the robot's camera.
        /// <para>This call requests the robot to capture an image and returns it.  If the camera feed is active, this call returns the contents of the <see cref="LatestImage"/> property and does
        /// not request a new image from the robot.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the image captured from the robot.</returns>
        public async Task<Image> CaptureSingleImage()
        {
            if (IsFeedActive) return LatestImage;
            var response = await Robot.RunMethod(client => client.CaptureSingleImageAsync(new CaptureSingleImageRequest())).ConfigureAwait(false);
            response.Status.Code.EnsureSuccess();
            return new Image(response);
        }

        /// <summary>
        /// Captures a high resolution image from the robot's camera and returns it.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the image captured from the robot.</returns>
        /// <remarks>This metohd is only available with firmware version 1.7 or greater.  See <see cref="Capabilities.HighResolutionImageCapture"/>.</remarks>
        public async Task<Image> CaptureHighResolutionImage()
        {
            Robot.Capabilities.Assert(Capabilities.Version1_7);
            var response = await Robot.RunMethod(client => client.CaptureSingleImageAsync(new CaptureSingleImageRequest() { EnableHighResolution = true })).ConfigureAwait(false);
            response.Status.Code.EnsureSuccess();
            if (IsFeedActive) needsStreamEnabled = true;
            return new Image(response);
        }

        /// <summary>
        /// Enable auto exposure on Vector's Camera.
        /// <para>Enable auto exposure on Vector's camera to constantly update the exposure time and gain values based on the recent images. This is the default mode when any SDK program starts.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <remarks>This method is only available with firmware version 1.7 or greater.  See <see cref="Capabilities.CameraSettings"/>.</remarks>
        public async Task<StatusCode> EnableAutoExposure()
        {
            Robot.Capabilities.Assert(Capabilities.Version1_7);
            var response = await Robot.RunMethod(client => client.SetCameraSettingsAsync(new SetCameraSettingsRequest() { EnableAutoExposure = true })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Set manual exposure values for Vector's Camera.
        /// <para>This will disable auto exposure on Vector's camera and force the specified exposure time and gain values.</para>
        /// <para>See the camera <see cref="Config"/> for valid ranges for the exposure and gain parameters</para>
        /// </summary>
        /// <param name="exposureMs">The exposure in milliseconds.</param>
        /// <param name="gain">The gain.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <remarks>Requires behavior control.  This method is only available with firmware version 1.7 or greater.  See <see cref="Capabilities.CameraSettings"/>.</remarks>
        public async Task<StatusCode> SetManualExposure(uint exposureMs, float gain)
        {
            Robot.Capabilities.Assert(Capabilities.Version1_7);
            if (exposureMs < Config.MinCameraExposureTimeMs || exposureMs > Config.MaxCameraExposureTimeMs) throw new ArgumentOutOfRangeException(nameof(exposureMs), "Exposure value of range");
            if (gain < Config.MinCameraGain || gain > Config.MaxCameraGain) throw new ArgumentOutOfRangeException(nameof(gain), "Gain value is out of range");

            var response = await Robot.RunControlMethod(client => client.SetCameraSettingsAsync(new SetCameraSettingsRequest()
            {
                Gain = gain,
                ExposureMs = exposureMs,
                EnableAutoExposure = false
            })).ConfigureAwait(false);

            Gain = gain;
            ExposureMs = exposureMs;
            AutoExposureEnabled = false;

            OnPropertyChanged(nameof(AutoExposureEnabled));
            OnPropertyChanged(nameof(ExposureMs));
            OnPropertyChanged(nameof(Gain));

            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Retrieves the calibrated camera settings from the robot.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the camera configuration.</returns>
        /// <remarks>This method is only available with firmware version 1.7 or greater.  See <see cref="Capabilities.CameraSettings"/>.</remarks>
        private async Task<CameraConfig> ReadCameraConfig()
        {
            Robot.Capabilities.Assert(Capabilities.Version1_7);
            var response = await Robot.RunMethod(client => client.GetCameraConfigAsync(new CameraConfigRequest())).ConfigureAwait(false);
            return new CameraConfig(response);
        }

        /// <summary>
        /// Enables the high resolution image streaming.  This method does not appear to work.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <remarks>This method will hang if <see cref="RobotStatus.IsInCalmPowerMode"/> is true</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is not yet work.")]
        private async Task<StatusCode> EnableHighResolutionImageStreaming()
        {
            Robot.Capabilities.Assert(Capabilities.Version1_7);
            var response = await Robot.RunMethod(client => client.EnableImageStreamingAsync(new EnableImageStreamingRequest() { Enable = true, EnableHighResolution = true })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Enables the image streaming.  Image streaming needs to be enabled for the robot to receive images from the feed.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <remarks>This method will hang if <see cref="RobotStatus.IsInCalmPowerMode"/> is true</remarks>
        private async Task<StatusCode> EnableImageStreaming()
        {
            var response = await Robot.RunMethod(client => client.EnableImageStreamingAsync(new EnableImageStreamingRequest() { Enable = true, EnableHighResolution = false })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Disables the image streaming.  
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        private async Task<StatusCode> DisableImageStreaming()
        {
            var response = await Robot.RunMethod(client => client.EnableImageStreamingAsync(new EnableImageStreamingRequest() { Enable = false, EnableHighResolution = false })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Determines whether image streaming is enabled.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains whether not streaming is enabled.</returns>
        private async Task<bool> IsImageStreamingEnabled()
        {
            var response = await Robot.RunMethod(client => client.IsImageStreamingEnabledAsync(new IsImageStreamingEnabledRequest())).ConfigureAwait(false);
            return response.IsImageStreamingEnabled;
        }

        /// <summary>
        /// Ensures the stream is enabled.  This is called from the robot status method to ensure that the robot is not in calm power mode.
        /// </summary>
        internal async Task EnsureStreamEnabled()
        {
            if (!needsStreamEnabled) return;
            if (!IsFeedActive) return;
            if (Robot.Status.IsInCalmPowerMode) return;
            needsStreamEnabled = false;
            // Wait for the robot to be availabe after calm power mode
            await Task.Delay(1000).ConfigureAwait(false);
            await EnableImageStreaming().ConfigureAwait(false);
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
