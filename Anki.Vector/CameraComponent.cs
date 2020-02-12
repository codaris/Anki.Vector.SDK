// <copyright file="CameraComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector
{
    using System;
    using System.ComponentModel;
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
        /// Gets the image data for the last image received by the camera if the feed is active
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Image data")]
        public byte[] ImageData { get => _imageData; private set => SetProperty(ref _imageData, value); }
        private byte[] _imageData = null;

        /// <summary>
        /// Gets the image identifier of the last image received by the camera
        /// </summary>
        public uint ImageId { get => _imageId; private set => SetProperty(ref _imageId, value); }
        private uint _imageId = 0;

        /// <summary>
        /// Gets the image encoding of the last image received by the camera
        /// </summary>
        public ImageEncoding ImageEncoding { get => _imageEncoding; private set => SetProperty(ref _imageEncoding, value); }
        private ImageEncoding _imageEncoding;

        /// <summary>
        /// Gets the frame robot timestamp of the last image received by the camera
        /// </summary>
        public uint FrameTimestamp { get => _frameTimestamp; private set => SetProperty(ref _frameTimestamp, value); }
        private uint _frameTimestamp;

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
                    ImageData = imageReceivedEventArgs.ImageData;
                    ImageId = imageReceivedEventArgs.ImageId;
                    ImageEncoding = imageReceivedEventArgs.ImageEncoding;
                    FrameTimestamp = imageReceivedEventArgs.FrameTimestamp;
                    ImageReceived?.Invoke(this, imageReceivedEventArgs);
                },
                () => OnPropertyChanged(nameof(IsFeedActive)),
                robot.PropagateException
             );
        }

        /// <summary>
        /// Gets a value indicating whether the camera feed is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the camera feed is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsFeedActive => cameraFeed.IsActive;

        /// <summary>
        /// Starts the camera feed.  The feed will run in a background thread and raise the <see cref="ImageReceived" /> event for each received image.  It will
        /// also update the <see cref="ImageData"/> property and the <see cref="ImageId"/> property whenever a new image is received.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StartFeed()
        {
            await cameraFeed.Start().ConfigureAwait(false);
            OnPropertyChanged(nameof(IsFeedActive));
        }

        /// <summary>
        /// Stops the camera feed.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StopFeed()
        {
            await cameraFeed.End().ConfigureAwait(false);
        }

        /// <summary>
        /// Request to capture a single image from the robot's camera.
        /// <para>This call requests the robot to capture an image and fills the properties of this component with the image information.  If the camera
        /// feed is active this call does nothing as the camera component properties already contain the latest image received from the robot.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<StatusCode> CaptureSingleImage()
        {
            if (IsFeedActive) return StatusCode.Ok;
            var response = await Robot.RunMethod(client => client.CaptureSingleImageAsync(new CaptureSingleImageRequest())).ConfigureAwait(false);
            ImageData = response.Data.ToByteArray();
            ImageId = response.ImageId;
            ImageEncoding = MapImageEncoding(response.ImageEncoding);
            FrameTimestamp = response.FrameTimeStamp;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Enables the image streaming.
        /// <para>This method is not necessary for retrieving camera images and is only implemented here for completeness.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is not necessary for image streaming.")]
        public async Task<StatusCode> EnableImageStreaming()
        {
            var response = await Robot.RunMethod(client => client.EnableImageStreamingAsync(new EnableImageStreamingRequest() { Enable = true })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Disables the image streaming.
        /// <para>This method is not necessary for retrieving camera images and is only implemented here for completeness.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is not necessary for image streaming.")]
        public async Task<StatusCode> DisableImageStreaming()
        {
            var response = await Robot.RunMethod(client => client.EnableImageStreamingAsync(new EnableImageStreamingRequest() { Enable = false })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Determines whether image streaming is enabled.
        /// <para>This should be identical to the <see cref="IsFeedActive" /> property.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains whether not streaming is enabled.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is not necessary for image streaming.")]
        public async Task<bool> IsImageStreamingEnabled()
        {
            var response = await Robot.RunMethod(client => client.IsImageStreamingEnabledAsync(new IsImageStreamingEnabledRequest())).ConfigureAwait(false);
            return response.IsImageStreamingEnabled;
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override Task Teardown()
        {
            return StopFeed();
        }

        /// <summary>
        /// Maps the image encoding.
        /// </summary>
        /// <param name="imageEncoding">The robot image encoding.</param>
        /// <returns>The SDK image encoding</returns>
        internal static ImageEncoding MapImageEncoding(ImageChunk.Types.ImageEncoding imageEncoding)
        {
            switch (imageEncoding)
            {
                case ImageChunk.Types.ImageEncoding.NoneImageEncoding: return ImageEncoding.NoneImageEncoding;
                case ImageChunk.Types.ImageEncoding.RawGray: return ImageEncoding.RawGray;
                case ImageChunk.Types.ImageEncoding.RawRgb: return ImageEncoding.RawRgb;
                case ImageChunk.Types.ImageEncoding.Yuyv: return ImageEncoding.Yuyv;
                case ImageChunk.Types.ImageEncoding.Yuv420Sp: return ImageEncoding.Yuv420Sp;
                case ImageChunk.Types.ImageEncoding.Bayer: return ImageEncoding.Bayer;
                case ImageChunk.Types.ImageEncoding.JpegGray: return ImageEncoding.JpegGray;
                case ImageChunk.Types.ImageEncoding.JpegColor: return ImageEncoding.JpegColor;
                case ImageChunk.Types.ImageEncoding.JpegColorHalfWidth: return ImageEncoding.JpegColorHalfWidth;
                case ImageChunk.Types.ImageEncoding.JpegMinimizedGray: return ImageEncoding.JpegMinimizedGray;
                case ImageChunk.Types.ImageEncoding.JpegMinimizedColor: return ImageEncoding.JpegMinimizedColor;
                default:
                    throw new NotSupportedException($"ImageEncoding {imageEncoding} is not supported");
            }
        }
    }
}
