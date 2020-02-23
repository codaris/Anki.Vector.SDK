// <copyright file="VisionComponent.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Anki.Vector.Events;
using Anki.Vector.Exceptions;
using Anki.Vector.Types;

namespace Anki.Vector
{
    /// <summary>
    /// Utility methods for Vector’s vision
    /// <para>Vector’s can detect various types of objects through his camera feed.</para>
    /// </summary>
    public class VisionComponent : Component
    {
        /// <summary>
        /// Gets a value indicating whether face detection is enabled
        /// </summary>
        public bool FaceDetectionEnabled { get => _faceDetectionEnabled; private set => SetProperty(ref _faceDetectionEnabled, value); }
        private bool _faceDetectionEnabled;

        /// <summary>
        /// Gets a value indicating whether custom object detection is enabled
        /// </summary>
        public bool CustomObjectDetectionEnabled { get => _customObjectDetectionEnabled; private set => SetProperty(ref _customObjectDetectionEnabled, value); }
        private bool _customObjectDetectionEnabled;

        /// <summary>
        /// Gets a value indicating whether mirror mode is enabled
        /// </summary>
        public bool MirrorModeEnabled { get => _mirrorModeEnabled; private set => SetProperty(ref _mirrorModeEnabled, value); }
        private bool _mirrorModeEnabled;

        /// <summary>
        /// Gets a value indicating whether motion detection is enabled
        /// </summary>
        public bool MotionDetectionEnabled { get => _motionDetectionEnabled; private set => SetProperty(ref _motionDetectionEnabled, value); }
        private bool _motionDetectionEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisionComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal VisionComponent(Robot robot) : base(robot)
        {
            robot.Events.MirrorModeDisabled += Events_MirrorModeDisabled;
            robot.Events.VisionModesAutoDisabled += Events_VisionModesAutoDisabled;
        }

        /// <summary>
        /// Handles the VisionModesAutoDisabled event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="VisionModesAutoDisabledEventArgs"/> instance containing the event data.</param>
        private void Events_VisionModesAutoDisabled(object sender, VisionModesAutoDisabledEventArgs e)
        {
            FaceDetectionEnabled = false;
            CustomObjectDetectionEnabled = false;
            MirrorModeEnabled = false;
            MotionDetectionEnabled = false;
        }

        /// <summary>
        /// Handles the MirrorModeDisabled event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MirrorModeDisabledEventArgs"/> instance containing the event data.</param>
        private void Events_MirrorModeDisabled(object sender, MirrorModeDisabledEventArgs e)
        {
            MirrorModeEnabled = false;
        }

        /// <summary>
        /// Enable face detection on the robot’s camera
        /// </summary>
        /// <param name="estimateEspression">if set to <c>true</c> to estimate expression.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> EnableFaceDetection(bool estimateEspression = false)
        {
            var response = await Robot.RunMethod(client => client.EnableFaceDetectionAsync(new ExternalInterface.EnableFaceDetectionRequest()
            {
                Enable = true,
                EnableBlinkDetection = false,
                EnableGazeDetection = false,
                EnableSmileDetection = false,
                EnableExpressionEstimation = estimateEspression
            })).ConfigureAwait(false);
            FaceDetectionEnabled = true;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Disables face detection on the robot’s camera
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> DisableFaceDetection()
        {
            var response = await Robot.RunMethod(client => client.EnableFaceDetectionAsync(new ExternalInterface.EnableFaceDetectionRequest()
            {
                Enable = false,
                EnableBlinkDetection = false,
                EnableGazeDetection = false,
                EnableSmileDetection = false,
                EnableExpressionEstimation = false
            })).ConfigureAwait(false);
            FaceDetectionEnabled = false;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Enables the custom object detection.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> EnableCustomObjectDetection()
        {
            var response = await Robot.RunMethod(client => client.EnableMarkerDetectionAsync(new ExternalInterface.EnableMarkerDetectionRequest()
            {
                Enable = true
            })).ConfigureAwait(false);
            CustomObjectDetectionEnabled = true;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Disables the custom object detection.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> DisableCustomObjectDetection()
        {
            var response = await Robot.RunMethod(client => client.EnableMarkerDetectionAsync(new ExternalInterface.EnableMarkerDetectionRequest()
            {
                Enable = false
            })).ConfigureAwait(false);
            CustomObjectDetectionEnabled = false;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Enables display of the robot’s camera feed on its face along with any detections (if enabled)
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> EnableMirrorMode()
        {
            var response = await Robot.RunMethod(client => client.EnableMirrorModeAsync(new ExternalInterface.EnableMirrorModeRequest()
            {
                Enable = true
            })).ConfigureAwait(false);
            MirrorModeEnabled = true;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Disables display of the robot’s camera feed on its face along with any detections (if enabled)
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> DisableMirrorMode()
        {
            var response = await Robot.RunMethod(client => client.EnableMirrorModeAsync(new ExternalInterface.EnableMirrorModeRequest()
            {
                Enable = false
            })).ConfigureAwait(false);
            MirrorModeEnabled = false;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Enables motion detection (not tested).
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> EnableMotionDetection()
        {
            var response = await Robot.RunMethod(client => client.EnableMotionDetectionAsync(new ExternalInterface.EnableMotionDetectionRequest()
            {
                Enable = true
            })).ConfigureAwait(false);
            MotionDetectionEnabled = true;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Disabled motion detection (not tested).
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> DisableMotionDetection()
        {
            var response = await Robot.RunMethod(client => client.EnableMotionDetectionAsync(new ExternalInterface.EnableMotionDetectionRequest()
            {
                Enable = false
            })).ConfigureAwait(false);
            MotionDetectionEnabled = false;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Closes all the running vision modes and waits for a response.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DisableAllVisionModes()
        {
            if (FaceDetectionEnabled) await DisableFaceDetection().ConfigureAwait(false);
            if (CustomObjectDetectionEnabled) await DisableCustomObjectDetection().ConfigureAwait(false);
            if (MirrorModeEnabled) await DisableMirrorMode().ConfigureAwait(false);
            if (MotionDetectionEnabled) await DisableMotionDetection().ConfigureAwait(false);
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <param name="forced">if set to <c>true</c> the shutdown is forced due to lost connection.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Suppress all exceptions during teardown")]
        internal override async Task Teardown(bool forced)
        {
            try
            {
                if (!forced) await DisableAllVisionModes().ConfigureAwait(false);
            }
            catch (VectorNotConnectedException)
            {
                // Ignore
            }
        }
    }
}
