// <copyright file="BehaviorComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using Anki.Vector.Exceptions;
using Anki.Vector.ExternalInterface;
using Anki.Vector.Objects;
using Anki.Vector.Types;
using static Anki.Vector.ExternalInterface.ExternalInterface;
using ActionResult = Anki.Vector.Types.ActionResult;

namespace Anki.Vector
{
    /// <summary>
    /// Which part of the robot to align with an object.
    /// </summary>
    public enum AlignmentType
    {
        /// <summary>Fingers aligned with object</summary>
        LiftFinger = 1,
        /// <summary>Lift plate aligned with object</summary>
        LiftPlate = 2,
        /// <summary>Front of body aligned with object</summary>
        Body = 3
    }

    /// <summary>
    /// Behavior related classes and functions.
    /// <para>Behaviors represent a complex task which requires Vector’s internal logic to determine how long it will take.This may include combinations
    /// of animation, path planning or other functionality.</para><para>For commands such as <see cref="GoToPose" />, <see cref="DriveOnCharger" /> and <see cref="DockWithCube" />, Vector uses path planning, which refers
    /// to the problem of navigating the robot from point A to B without collisions. Vector loads known obstacles from his map, creates a path to navigate
    /// around those objects, then starts following the path.  If a new obstacle is found while following the path, a new plan may be created.</para>
    /// </summary>
    /// <seealso cref="Anki.Vector.Component" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Component is disposed by Teardown method.")]
    public class BehaviorComponent : Component
    {
        /// <summary>
        /// The minimum head angle in radians
        /// </summary>
        public const float MinHeadAngle = -22f;

        /// <summary>
        /// The maximum head angle in radians
        /// </summary>
        public const float MaxHeadAngle = 45f;

        /// <summary>
        /// The minimum lift height in millimeters
        /// </summary>
        private const float MinLiftHeight = 32f;

        /// <summary>
        /// The maximum lift height in millimeters
        /// </summary>
        private const float MaxLiftHeight = 92f;

        /// <summary>
        /// The next action identifier
        /// </summary>
        private int nextActionId = (int)ActionTagConstants.FirstSdkTag;

        /// <summary>
        /// The currently active action identifier
        /// </summary>
        private int? activeActionId = null;

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource cancellationTokenSource = null;

        /// <summary>
        /// Gets a value indicating whether Vector is busy performing a behavior.
        /// </summary>
        public bool IsBusy => cancellationTokenSource != null;

        /// <summary>
        /// Gets the motion profile that tells Vector how to drive when receiving navigation and movement actions such as <see cref="GoToPose"/> and <see cref="DockWithCube"/>.
        /// </summary>
        /// <value>
        /// The motion profile.
        /// </value>
        public MotionProfile MotionProfile { get; } = new MotionProfile();

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal BehaviorComponent(Robot robot) : base(robot)
        {
        }

        /// <summary>
        /// Make Vector speak text.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="text">The words for Vector to say.</param>
        /// <param name="useVectorVoice">Whether to use Vector’s robot voice (otherwise, he uses a generic human male voice).</param>
        /// <param name="durationScalar">Adjust the relative duration of the generated text to speech audio.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<StatusCode> SayText(string text, bool useVectorVoice = true, float durationScalar = 1.0f)
        {
            var response = await Robot.RunControlMethod(client => client.SayTextAsync(new SayTextRequest()
            {
                Text = text,
                UseVectorVoice = useVectorVoice,
                DurationScalar = durationScalar,
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Drive Vector off the charger
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<BehaviorResult> DriveOffCharger()
        {
            var response = await RunBehavior((client, cancellationToken) => client.DriveOffChargerAsync(new ExternalInterface.DriveOffChargerRequest(), cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new BehaviorResult(response.Status.Code, response.Result);
        }

        /// <summary>
        /// Drive Vector onto the charger
        /// <para>Vector will attempt to find the charger and, if successful, he will back onto it and start charging.
        /// Vector’s charger has a visual marker so that the robot can locate it for self-docking.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<BehaviorResult> DriveOnCharger()
        {
            var response = await RunBehavior((client, cancellationToken) => client.DriveOnChargerAsync(new ExternalInterface.DriveOnChargerRequest(), cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new BehaviorResult(response.Status.Code, response.Result);
        }

        /// <summary>
        /// Look around for faces
        /// <para>Turn in place and move head to look for faces.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<BehaviorResult> FindFaces()
        {
            var response = await RunBehavior((client, cancellationToken) => client.FindFacesAsync(new FindFacesRequest(), cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new BehaviorResult(response.Status.Code, response.Result);
        }

        /// <summary>
        /// Look around in place
        /// <para>Turn in place and move head to see what's around Vector.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<BehaviorResult> LookAroundInPlace()
        {
            var response = await RunBehavior((client, cancellationToken) => client.LookAroundInPlaceAsync(new LookAroundInPlaceRequest(), cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new BehaviorResult(response.Status.Code, response.Result);
        }

        /// <summary>
        /// Roll a cube that is currently known to the robot.
        /// <para>This behavior will move into position as necessary based on relative distance and orientation.  Vector needs to see the block for this to succeed.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<BehaviorResult> RollVisibleCube()
        {
            var response = await RunBehavior((client, cancellationToken) => client.RollBlockAsync(new RollBlockRequest(), cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new BehaviorResult(response.Status.Code, response.Result);
        }

        /// <summary>
        /// Tells Vector to drive in a straight line.  Vector will drive for the specified distance(forwards or backwards).  Vector must be off of the charger for this movement action.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="distanceMm">The distance in mm. (positive for forwards, negative for backwards)</param>
        /// <param name="speedMmps">The speed mm/s.</param>
        /// <param name="shouldPlayAnimation">Whether to play idle animations whilst driving</param>
        /// <param name="numRetries">Number of times to re-attempt action in case of a failure</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="System.NotSupportedException">The action result is not supported</exception>
        public async Task<ActionResult> DriveStraight(float distanceMm, float speedMmps, bool shouldPlayAnimation = true, int numRetries = 0)
        {
            var response = await RunAction((client, cancellationToken, idTag) => client.DriveStraightAsync(new ExternalInterface.DriveStraightRequest()
            {
                IdTag = idTag,
                DistMm = distanceMm,
                SpeedMmps = speedMmps,
                ShouldPlayAnimation = shouldPlayAnimation,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Tells Vector to drive to the specified pose and orientation.
        /// <para>In navigating to the requested pose, Vector will use path planning.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="pose">The pose.</param>
        /// <param name="relativeToRobot">Whether the given pose is relative to the robot's pose.</param>
        /// <param name="retries">The retries.</param>
        /// <returns>
        /// A task that represents the asynchronous operation; the task result contains the result from the robot.
        /// </returns>
        public async Task<ActionResult> GoToPose(Pose pose, bool relativeToRobot = false, int retries = 0)
        {
            if (relativeToRobot) pose = Robot.Pose.RelativeToThis(pose);
            var response = await RunAction((client, cancellationToken, idTag) => client.GoToPoseAsync(new GoToPoseRequest()
            {
                IdTag = idTag,
                MotionProf = MotionProfile.ToPathMotionProfile(),
                XMm = pose.Position.X,
                YMm = pose.Position.Y,
                Rad = pose.Rotation.AngleZ,
                NumRetries = retries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Tells Vector to dock with a light cube, optionally using a given approach angle and distance.
        /// <para>While docking with the cube, Vector will use path planning.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="lightCube">The light cube.</param>
        /// <param name="approachAngle">The approach angle in radians.</param>
        /// <param name="alignment">Which part of the robot to align with the object.</param>
        /// <param name="distanceFromMarker">The distance from marker in mm (0 to dock).</param>
        /// <param name="numRetries">Number of times to re-attempt action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<ActionResult> DockWithCube(LightCube lightCube, float? approachAngle = null, AlignmentType alignment = AlignmentType.LiftPlate, float distanceFromMarker = 0f, int numRetries = 0)
        {
            var response = await RunAction((client, cancellationToken, idTag) => client.DockWithCubeAsync(new DockWithCubeRequest()
            {
                IdTag = idTag,
                AlignmentType = (ExternalInterface.AlignmentType)alignment,
                MotionProf = MotionProfile.ToPathMotionProfile(),
                ObjectId = lightCube.ObjectId,
                ApproachAngleRad = approachAngle ?? 0,
                UseApproachAngle = approachAngle.HasValue,
                UsePreDockPose = approachAngle.HasValue,
                DistanceFromMarkerMm = distanceFromMarker,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Set Vector’s eye color.
        /// </summary>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> SetEyeColor(float hue, float saturation)
        {
            var response = await Robot.RunMethod(client => client.SetEyeColorAsync(new SetEyeColorRequest()
            {
                Hue = hue,
                Saturation = saturation
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Tell Vector's head to move to a given angle.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="angle">Desired angle for Vector’s head.  Clamped to <see cref="MinHeadAngle"/> and <see cref="MaxHeadAngle"/>.</param>
        /// <param name="acceleration">Acceleration of Vector’s head in radians per second squared.</param>
        /// <param name="maxSpeed">Maximum speed of Vector’s head in radians per second.</param>
        /// <param name="duration">Time for Vector’s head to move in seconds. A value of zero will make Vector try to do it as quickly as possible.</param>
        /// <param name="numRetries">Number of times to re-attempt the action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<ActionResult> SetHeadAngle(float angle, float acceleration = 10f, float maxSpeed = 10f, float duration = 0f, int numRetries = 0)
        {
            if (angle < MinHeadAngle) angle = MinHeadAngle;
            else if (angle > MaxHeadAngle) angle = MaxHeadAngle;
            var response = await RunAction((client, cancellationToken, idTag) => client.SetHeadAngleAsync(new SetHeadAngleRequest()
            {
                IdTag = idTag,
                AngleRad = angle,
                MaxSpeedRadPerSec = maxSpeed,
                AccelRadPerSec2 = acceleration,
                DurationSec = duration,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Tell Vector’s lift to move to a given height.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="height">Desired height for Vector’s lift clamped to 0.0 (bottom) to 1.0 (top).</param>
        /// <param name="acceleration">Acceleration of Vector’s lift in radians per second squared.</param>
        /// <param name="maxSpeed">Maximum speed of Vector’s lift in radians per second.</param>
        /// <param name="duration">Time for Vector’s lift to move in seconds. A value of zero will make Vector try to do it as quickly as possible.</param>
        /// <param name="numRetries">Number of times to re-attempt the action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<ActionResult> SetLiftHeight(float height, float acceleration = 10f, float maxSpeed = 10f, float duration = 0f, int numRetries = 0)
        {
            if (height < 0.0) height = MinLiftHeight;
            else if (height > 1.0) height = MaxLiftHeight;
            else height = MinLiftHeight + (height * (MaxLiftHeight - MinLiftHeight));

            var response = await RunAction((client, cancellationToken, idTag) => client.SetLiftHeightAsync(new SetLiftHeightRequest()
            {
                IdTag = idTag,
                HeightMm = height,
                MaxSpeedRadPerSec = maxSpeed,
                AccelRadPerSec2 = acceleration,
                DurationSec = duration,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Turn the robot around its current position.
        /// <para>Vector must be off of the charger for this movement action.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="angle">The angle to turn. Positive values turn to the left, negative values to the right.</param>
        /// <param name="speed">Angular turn speed (per second).</param>
        /// <param name="acceleration"> Acceleration of angular turn (per second squared).</param>
        /// <param name="angleTolerance">angular tolerance to consider the action complete (this is clamped to a minimum of 2 degrees internally).</param>
        /// <param name="isAbsolute">True to turn to a specific angle, False to turn relative to the current pose.</param>
        /// <param name="numRetries">Number of times to re-attempt the turn in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<ActionResult> TurnInPlace(float angle, float speed = 0, float acceleration = 0, float angleTolerance = 0, bool isAbsolute = false, int numRetries = 0)
        {
            var response = await RunAction((client, cancellationToken, idTag) => client.TurnInPlaceAsync(new TurnInPlaceRequest()
            {
                IdTag = idTag,
                AngleRad = angle,
                SpeedRadPerSec = speed,
                AccelRadPerSec2 = acceleration,
                TolRad = angleTolerance,
                IsAbsolute = isAbsolute ? 1u : 0u,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Tells Vector to turn towards this face.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="face">The face Vector will turn towards.</param>
        /// <param name="numRetries">Number of times to reattempt the action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<ActionResult> TurnTowardsFace(Face face, int numRetries = 0)
        {
            var response = await RunAction((client, cancellationToken, idTag) => client.TurnTowardsFaceAsync(new TurnTowardsFaceRequest()
            {
                IdTag = idTag,
                FaceId = face.FaceId,
                MaxTurnAngleRad = 3.14159f,  // 180 degrees
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Tells Vector to drive to his Cube.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="distanceFromCube">The distance from the cube to stop in mm.  This is the distance between the origins.<para>For instance, the distance from the robot's origin (between Vector's two front wheels) to the cube's origin (at the center of the cube) is ~40mm.</para></param>
        /// <param name="numRetries">Number of times to reattempt action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="InvalidOperationException">Light Cube must have been seen by Vector to run this command.</exception>
        public async Task<ActionResult> GoToCube(float distanceFromCube, int numRetries = 0)
        {
            if (Robot.World.LightCube == null)
            {
                throw new InvalidOperationException("Light Cube must have been seen by Vector to run this command.");
            }

            var response = await RunAction((client, cancellationToken, idTag) => client.GoToObjectAsync(new GoToObjectRequest()
            {
                IdTag = idTag,
                DistanceFromObjectOriginMm = distanceFromCube,
                UsePreDockPose = false,
                ObjectId = Robot.World.LightCube.ObjectId,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Tells Vector to roll a specified cube object.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="approachAngle">The angle to approach the cube from in radians. <para>For example, 180 degrees will cause Vector to drive past the cube and approach it from behind.</para></param>
        /// <param name="numRetries">Number of times to reattempt action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="InvalidOperationException">Light Cube must have been seen by Vector to run this command.</exception>
        public async Task<ActionResult> RollCube(float? approachAngle = null, int numRetries = 0)
        {
            if (Robot.World.LightCube == null)
            {
                throw new InvalidOperationException("Light Cube must have been seen by Vector to run this command.");
            }

            var response = await RunAction((client, cancellationToken, idTag) => client.RollObjectAsync(new RollObjectRequest()
            {
                IdTag = idTag,
                ApproachAngleRad = approachAngle ?? 0,
                UseApproachAngle = approachAngle.HasValue,
                UsePreDockPose = approachAngle.HasValue,
                ObjectId = Robot.World.LightCube.ObjectId,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Tells Vector to "pop a wheelie" using his light cube.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="approachAngle">The angle to approach the cube from in radians. <para>For example, 180 degrees will cause Vector to drive past the cube and approach it from behind.</para></param>
        /// <param name="numRetries">Number of times to reattempt action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="InvalidOperationException">Light Cube must have been seen by Vector to run this command.</exception>
        public async Task<ActionResult> PopAWheelie(float? approachAngle = null, int numRetries = 0)
        {
            if (Robot.World.LightCube == null)
            {
                throw new InvalidOperationException("Light Cube must have been seen by Vector to run this command.");
            }

            var response = await RunAction((client, cancellationToken, idTag) => client.PopAWheelieAsync(new PopAWheelieRequest()
            {
                IdTag = idTag,
                ApproachAngleRad = approachAngle ?? 0,
                UseApproachAngle = approachAngle.HasValue,
                UsePreDockPose = approachAngle.HasValue,
                ObjectId = Robot.World.LightCube.ObjectId,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Instruct the robot to pick up his LightCube.
        /// <para>While picking up the cube, Vector will use path planning.</para><para>Note that actions that use the wheels cannot be performed at the same time, otherwise you may see a TRACKS_LOCKED error.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="lightCube">The light cube.</param>
        /// <param name="usePreDockPose">Whether or not to try to immediately pick up an object or first position the robot next to the object.</param>
        /// <param name="numRetries">Number of times to reattempt action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="System.ArgumentException">Light cube object must be provided. - lightCube</exception>
        public async Task<ActionResult> PickupObject(LightCube lightCube, bool usePreDockPose = true, int numRetries = 0)
        {
            if (lightCube == null) throw new ArgumentException("Light cube object must be provided.", nameof(lightCube));

            var response = await RunAction((client, cancellationToken, idTag) => client.PickupObjectAsync(new PickupObjectRequest()
            {
                IdTag = idTag,
                UsePreDockPose = usePreDockPose,
                ObjectId = lightCube.ObjectId,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Ask Vector to place the object he is carrying on the ground at the current location.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="numRetries">Number of times to reattempt action in case of a failure.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="InvalidOperationException">Light Cube must have been seen by Vector to run this command.</exception>
        public async Task<ActionResult> PlaceObjectOnGroundHere(int numRetries = 0)
        {
            if (Robot.World.LightCube == null)
            {
                throw new InvalidOperationException("Light Cube must have been seen by Vector to run this command.");
            }

            var response = await RunAction((client, cancellationToken, idTag) => client.PlaceObjectOnGroundHereAsync(new PlaceObjectOnGroundHereRequest()
            {
                IdTag = idTag,
                NumRetries = numRetries
            }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new ActionResult(response.Status.Code, response.Result.Code);
        }

        /// <summary>
        /// Request that Vector listens for a beat.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> ListenForBeat()
        {
            return AppIntent("intent_imperative_dance");
        }

        /// <summary>
        /// Request that Vector start's exploring
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> StartExploring()
        {
            return AppIntent("explore_start");
        }

        /// <summary>
        /// Request that Vector looks for a face and says the associated name.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> SayName()
        {
            return AppIntent("intent_names_ask");
        }

        /// <summary>
        /// Requests that Vector goes to sleep
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> Sleep()
        {
            return AppIntent("intent_system_sleep");
        }

        /// <summary>
        /// Requests that Vector listen for a knowledge question and provide response.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> ListenForQuestion()
        {
            return AppIntent("knowledge_question");
        }

        /// <summary>
        /// Requests that Vector finds his cube.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> FindCube()
        {
            return AppIntent("intent_imperative_findcube");
        }

        /// <summary>
        /// Submit an intent for Vector to carry out.
        /// </summary>
        /// <param name="intent">The intent for Vector carry out.</param>
        /// <param name="param">Any extra parameters.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The intent is not the same namespace as UserIntent</remarks>
        /// <remarks>Requires releasing behavior control before.  Otherwise, the intent is at too low of priority to run.</remarks>
        public async Task<StatusCode> AppIntent(string intent, string param = "")
        {
            var response = await Robot.RunMethod(client => client.AppIntentAsync(new AppIntentRequest()
            {
                Intent = intent,
                Param = param
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Cancel the currently active behavior
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Cancel()
        {
            // Cancel the active action by ID
            if (activeActionId.HasValue)
            {
                await Robot.RunMethod(client => client.CancelActionByIdTagAsync(new CancelActionByIdTagRequest() { IdTag = (uint)activeActionId.Value })).ConfigureAwait(false);
                activeActionId = null;
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource = null;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <param name="forced">if set to <c>true</c> the shutdown is forced due to lost connection.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Not a general exception")]
        internal override async Task Teardown(bool forced)
        {
            try
            {
                if (!forced) await Cancel().ConfigureAwait(false);
            }
            catch (VectorNotConnectedException)
            {
                // Ignore
            }
        }

        /// <summary>
        /// Runs the behavior by wrapping it in code that allows for cancellation.
        /// </summary>
        /// <typeparam name="T">The result of the behavior</typeparam>
        /// <param name="function">The function to execute with CancellationToken parameter.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the function.</returns>
        private async Task<T> RunBehavior<T>(Func<ExternalInterfaceClient, CancellationToken, Grpc.Core.AsyncUnaryCall<T>> function)
        {
            // If operation in progress, cancel it
            await Cancel().ConfigureAwait(false);

            // Create a new cancellation token
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            // Mark vector as busy
            OnPropertyChanged(nameof(IsBusy));
            try
            {
                // Run the function inside this catch block
                return await Robot.RunControlMethod(client => function(client, token)).ConfigureAwait(false);
            }
            finally
            {
                // Clear the token and mark vector as no longer busy
                cancellationTokenSource = null;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        /// <summary>
        /// Runs the action by wrapping it in code that allows for cancellation.  Provides the IdValue in callback
        /// </summary>
        /// <typeparam name="T">The result type of the action to run.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task<T> RunAction<T>(Func<ExternalInterfaceClient, CancellationToken, int, Grpc.Core.AsyncUnaryCall<T>> function)
        {
            // If operation in progress, cancel it
            await Cancel().ConfigureAwait(false);

            // Create a new cancellation token
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            // Store and post increment nextActionId (and loop within the SDK TAG range)
            activeActionId = nextActionId++;
            if (nextActionId > (int)ActionTagConstants.LastSdkTag) nextActionId = (int)ActionTagConstants.FirstSdkTag;

            // Mark vector as busy
            OnPropertyChanged(nameof(IsBusy));
            try
            {
                // Run the function inside this catch block
                return await Robot.RunControlMethod(client => function(client, token, activeActionId.Value)).ConfigureAwait(false);
            }
            finally
            {
                // Clear the token and mark vector as no longer busy
                cancellationTokenSource = null;
                activeActionId = null;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
    }
}
