// <copyright file="ScreenComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Anki.Vector.Types;

namespace Anki.Vector
{
    /// <summary>
    /// Manage the state of the faces on the robot.
    /// </summary>
    public class ScreenComponent : Component
    {
        /// <summary>
        /// The screen width in pixels
        /// </summary>
        public const int ScreenWidth = 184;

        /// <summary>
        /// The screen height in pixels
        /// </summary>
        public const int ScreenHeight = 96;

        /// <summary>
        /// The total number of pixels in the display
        /// </summary>
        public const int TotalPixels = ScreenHeight * ScreenWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal ScreenComponent(Robot robot) : base(robot)
        {
        }

        /// <summary>
        /// Display an image on Vector's Screen (his "face").
        /// </summary>
        /// <param name="imageData">A byte array representing all of the pixels (16bit color in rgb565 format)</param>
        /// <param name="durationMs">The number of milliseconds the image should remain on Vector's face.</param>
        /// <param name="interruptRunning">Set to true so any currently-streaming animation will be aborted in favor of this.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  The task result contains the result of the operation.
        /// </returns>
        public async Task<StatusCode> DisplayImageRgb565(byte[] imageData, uint durationMs, bool interruptRunning = true)
        {
            if (imageData == null) throw new ArgumentNullException(nameof(imageData));

            // Ensure image data is the right size
            if (imageData.Length != TotalPixels * 2)
            {
                throw new ArgumentOutOfRangeException(nameof(imageData), $"{nameof(DisplayImageRgb565)} expected {TotalPixels * 2} bytes (2 bytes for each {TotalPixels} pixels");
            }

            var response = await Robot.RunMethod(client => client.DisplayFaceImageRGBAsync(new ExternalInterface.DisplayFaceImageRGBRequest()
            {
                FaceData = Google.Protobuf.ByteString.CopyFrom(imageData),
                DurationMs = durationMs,
                InterruptRunning = interruptRunning
            })).ConfigureAwait(false);
            return (StatusCode)response.Status.Code;
        }

        /// <summary>
        /// Display an image on Vector's Screen (his "face").
        /// </summary>
        /// <param name="imageData">A byte array representing all of the pixels (24bit color in RGB format)</param>
        /// <param name="durationMs">The number of milliseconds the image should remain on Vector's face.</param>
        /// <param name="interruptRunning">Set to true so any currently-streaming animation will be aborted in favor of this.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  The task result contains the result of the operation.
        /// </returns>
        public Task<StatusCode> DisplayImageRgb24(byte[] imageData, uint durationMs, bool interruptRunning = true)
        {
            if (imageData == null) throw new ArgumentNullException(nameof(imageData));

            // Ensure image data is the right size
            if (imageData.Length != TotalPixels * 3)
            {
                throw new ArgumentOutOfRangeException(nameof(imageData), $"{nameof(DisplayImageRgb24)} expected {TotalPixels * 3} bytes (3 bytes for each {TotalPixels} pixels");
            }

            byte[] result = new byte[imageData.Length / 3 * 2];
            int j = 0;
            for (int i = 0; i < imageData.Length;)
            {
                int red5 = imageData[i++] >> 3;
                int green6 = imageData[i++] >> 2;
                int blue5 = imageData[i++] >> 3;

                int green3_hi = green6 >> 3;
                int green3_low = green6 & 0x07;

                result[j++] = (byte)((red5 << 3) | green3_hi);
                result[j++] = (byte)((green3_low << 5) | blue5);
            }
            return DisplayImageRgb565(imageData, durationMs, interruptRunning);
        }

        /// <summary>
        /// Display an image on Vector's Screen (his "face").
        /// </summary>
        /// <param name="imageData">A byte array representing all of the pixels (32bit color in RGBA format)</param>
        /// <param name="durationMs">The number of milliseconds the image should remain on Vector's face.</param>
        /// <param name="interruptRunning">Set to true so any currently-streaming animation will be aborted in favor of this.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  The task result contains the result of the operation.
        /// </returns>
        public Task<StatusCode> DisplayImageRgba32(byte[] imageData, uint durationMs, bool interruptRunning = true)
        {
            if (imageData == null) throw new ArgumentNullException(nameof(imageData));

            // Ensure image data is the right size
            if (imageData.Length != TotalPixels * 4)
            {
                throw new ArgumentOutOfRangeException(nameof(imageData), $"{nameof(DisplayImageRgba32)} expected {TotalPixels * 4} bytes (3 bytes for each {TotalPixels} pixels");
            }

            byte[] result = new byte[imageData.Length / 2];
            int j = 0;
            for (int i = 0; i < imageData.Length;)
            {
                int red5 = imageData[i++] >> 3;
                int green6 = imageData[i++] >> 2;
                int blue5 = imageData[i++] >> 3;
                i++;

                int green3_hi = green6 >> 3;
                int green3_low = green6 & 0x07;

                result[j++] = (byte)((red5 << 3) | green3_hi);
                result[j++] = (byte)((green3_low << 5) | blue5);
            }
            return DisplayImageRgb565(imageData, durationMs, interruptRunning);
        }

        /// <summary>
        /// Set Vector's Screen (his "face"). to a solid color.
        /// </summary>
        /// <param name="color">Desired color to set Vector's Screen.</param>
        /// <param name="durationMs">The number of milliseconds the image should remain on Vector's face.</param>
        /// <param name="interruptRunning">Set to true so any currently-streaming animation will be aborted in favor of this.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  The task result contains the result of the operation.
        /// </returns>
        public Task<StatusCode> DisplaySolidColor(Color color, uint durationMs, bool interruptRunning = true)
        {
            var imageData = new byte[35328];
            byte[] colorBytes = color.ToBytePair();
            for (int i = 0; i < 35328;)
            {
                foreach (byte colorByte in colorBytes) imageData[i++] = colorByte;
            }
            return DisplayImageRgb565(imageData, durationMs, interruptRunning);
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override Task Teardown()
        {
            return Task.CompletedTask;
        }
    }
}
