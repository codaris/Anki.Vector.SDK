// <copyright file="ActionResultCode.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// The result of an action
    /// </summary>
    public enum ActionResultCode
    {
        ActionResultSuccess = 0,
        ActionResultRunning = 16777216,
        ActionResultCancelledWhileRunning = 33554432,
        NotStarted = 33554433,
        Abort = 50331648,
        AnimAborted = 50331649,
        BadMarker = 50331650,
        BadMessageTag = 50331651,
        BadObject = 50331652,
        BadPose = 50331653,
        BadTag = 50331654,
        ChargerUnpluggedAbort = 50331655,
        CliffAlignFailedTimeout = 50331656,
        CliffAlignFailedNoTurning = 50331657,
        CliffAlignFailedOverTurning = 50331658,
        CliffAlignFailedNoWhite = 50331659,
        CliffAlignFailedStopped = 50331660,
        FailedSettingCalibration = 50331661,
        FollowingPathButNotTraversing = 50331662,
        Interrupted = 50331663,
        InvalidOffTreadsState = 50331664,
        MismatchedUpAxis = 50331665,
        NoAnimName = 50331666,
        NoDistanceSet = 50331667,
        NoFace = 50331668,
        NoGoalSet = 50331669,
        NoPreactionPoses = 50331670,
        NotCarryingObjectAbort = 50331671,
        NotOnChargerAbort = 50331672,
        NullSubaction = 50331673,
        PathPlanningFailedAbort = 50331674,
        PickupObjectUnexpectedlyMoving = 50331675,
        SendMessageToRobotFailed = 50331676,
        StillCarryingObject = 50331677,
        Timeout = 50331678,
        TracksLocked = 50331679,
        UnexpectedDockAction = 50331680,
        UnknownToolCode = 50331681,
        UpdateDerivedFailed = 50331682,
        VisualObservationFailed = 50331683,
        ShouldntDriveOnCharger = 50331684,
        Retry = 67108864,
        DidNotReachPreactionPose = 67108865,
        FailedTraversingPath = 67108866,
        LastPickAndPlaceFailed = 67108867,
        MotorStoppedMakingProgress = 67108868,
        NotCarryingObjectRetry = 67108869,
        NotOnChargerRetry = 67108870,
        PathPlanningFailedRetry = 67108871,
        PlacementGoalNotFree = 67108872,
        PickupObjectUnexpectedlyNotMoving = 67108873,
        StillOnCharger = 67108874,
        UnexpectedPitchAngle = 67108875,
    }

    /// <summary>
    /// Extension methods for working with action result codes
    /// </summary>
    internal static class ActionResultCodeExtensions
    {
        /// <summary>
        /// Converts the specified action result code.
        /// </summary>
        /// <param name="actionResultCode">The action result code.</param>
        /// <returns></returns>
        internal static ActionResultCode Convert(this ExternalInterface.ActionResult.Types.ActionResultCode actionResultCode)
        {
            return (ActionResultCode)actionResultCode;
        }
    }

}
