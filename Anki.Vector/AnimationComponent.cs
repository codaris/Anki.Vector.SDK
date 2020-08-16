// <copyright file="AnimationComponent.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Anki.Vector.ExternalInterface;
using Anki.Vector.Types;
using Animation = Anki.Vector.Types.Animation;
using AnimationTrigger = Anki.Vector.Types.AnimationTrigger;

namespace Anki.Vector
{
    /// <summary>
    /// Animation related classes, functions, events and values.
    /// <para>Animations represent a sequence of highly coordinated movements, faces, lights, and sounds used to demonstrate an emotion or reaction.</para><para>Animations can control the following tracks: head, lift, treads, face, audio and backpack lights.</para><para>There are two ways to play an animation on Vector: <see cref="PlayAnimation(Animation, uint, bool, bool, bool)" /> and <see cref="PlayAnimationTrigger(AnimationTrigger, uint, bool, bool, bool, bool)" />.  For PlayAnimationTrigger, you select a pre-defined
    /// group of animations, and the robot will choose which animation from the group to run when you execute the method.  When calling PlayAnimation, you
    /// select the specific animation you want the robot to run.  We advise you to use PlayAnimationTrigger instead of PlayAnimation, since individual
    /// animations can be deleted between Vector OS versions.</para>
    /// By default, when an SDK program starts, the SDK will request a list of known animation triggers and animations from the robot, which will be loaded
    /// and available from <see cref="GetAnimationTriggers" /> and <see cref="GetAnimations" />, respectively.
    /// </summary>
    /// <seealso cref="Anki.Vector.Component" />
    public class AnimationComponent : Component
    {
        /// <summary>
        /// The animation cache
        /// </summary>
        private IReadOnlyDictionary<string, Animation> animations = null;

        /// <summary>
        /// The animation trigger cache
        /// </summary>
        private IReadOnlyDictionary<string, AnimationTrigger> animationTriggers = null;

        /// <summary>
        /// The animation result
        /// </summary>
        private TaskCompletionSource<bool> animationRunningResult = null;

        /// <summary>
        /// The last animation start time
        /// </summary>
        private DateTime lastAnimationStart = DateTime.MinValue;

        /// <summary>
        /// Gets a value indicating whether the SDK is running an animation
        /// </summary>
        public bool IsAnimating => animationRunningResult != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationComponent"/> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal AnimationComponent(Robot robot) : base(robot)
        {
            Robot.Events.RobotState += Events_RobotState;
        }

        /// <summary>
        /// Handles the RobotState event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Events.RobotStateEventArgs"/> instance containing the event data.</param>
        private void Events_RobotState(object sender, Events.RobotStateEventArgs e)
        {
            // If we aren't waiting for a result, return
            if (animationRunningResult == null) return;

            // If we aren't animating and 250 milliseconds has past then set the result.
            var diff = DateTime.Now.Subtract(lastAnimationStart).TotalMilliseconds;
            if (!e.Status.IsAnimating && diff > 250)
            {
                animationRunningResult.TrySetResult(true);
                animationRunningResult = null;
                OnPropertyChanged(nameof(IsAnimating));
            }
        }

        /// <summary>
        /// Gets the list of animations returned from the robot.        
        /// <para>Animations are dynamically retrieved from the robot the first time this method is called and cached for subsequent requests.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains a dictionary of animations by name.</returns>
        public async Task<IReadOnlyDictionary<string, Animation>> GetAnimations()
        {
            if (animations != null) return animations;
            var result = await Robot.RunMethod(client => client.ListAnimationsAsync(new ListAnimationsRequest())).ConfigureAwait(false);
            var animationList = new Dictionary<string, Animation>();
            foreach (var animation in result.AnimationNames) animationList.Add(animation.Name, new Animation(animation));
            animations = new ReadOnlyDictionary<string, Animation>(animationList);
            return animations;
        }

        /// <summary>
        /// Gets the the set of animation triggers returned from the robot.
        /// <para>Animation triggers are dynamically retrieved from the robot the first time this method is called and cached for subsequent requests.</para>
        /// <para>Playing an animation trigger causes the robot to play an animation of a particular type.</para>
        /// <para>The robot may pick one of a number of actual animations to play based on Vector’s mood or emotion, or with random weighting. Thus playing the same trigger twice may not result in the exact same underlying animation playing twice.</para>
        /// <para>To play an exact animation, use <see cref="PlayAnimation(Animation, uint, bool, bool, bool)"/>.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains a dictionary of animations triggers by name.</returns>
        public async Task<IReadOnlyDictionary<string, AnimationTrigger>> GetAnimationTriggers()
        {
            if (animationTriggers != null) return animationTriggers;
            var result = await Robot.RunMethod(client => client.ListAnimationTriggersAsync(new ListAnimationTriggersRequest())).ConfigureAwait(false);
            var animationTriggerList = new Dictionary<string, AnimationTrigger>();
            foreach (var animationTrigger in result.AnimationTriggerNames) animationTriggerList.Add(animationTrigger.Name, new AnimationTrigger(animationTrigger));
            animationTriggers = new ReadOnlyDictionary<string, AnimationTrigger>(animationTriggerList);
            return animationTriggers;
        }

        /// <summary>
        /// Starts an animation playing on a robot.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="animation">The animation to play.</param>
        /// <param name="loopCount">Number of times to play the animation.</param>
        /// <param name="ignoreBodyTrack">if set to <c>true</c> ignore the animation track for Vector’s body (i.e. the wheels / treads).</param>
        /// <param name="ignoreHeadTrack">if set to <c>true</c> ignore the animation track for Vector’s head.</param>
        /// <param name="ignoreLiftTrack">if set to <c>true</c> ignore the animation track for Vector’s lift.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> PlayAnimation(Animation animation, uint loopCount = 1, bool ignoreBodyTrack = false, bool ignoreHeadTrack = false, bool ignoreLiftTrack = false)
        {
            var response = await Robot.RunControlMethod(client => client.PlayAnimationAsync(new PlayAnimationRequest()
            {
                Animation = animation.ToRobotAnimation(),
                Loops = loopCount,
                IgnoreBodyTrack = ignoreBodyTrack,
                IgnoreHeadTrack = ignoreHeadTrack,
                IgnoreLiftTrack = ignoreLiftTrack
            }
            )).ConfigureAwait(false);
            StartAnimationTracking();
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Starts an animation playing on a robot.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="animationName">Name of the animation to play.</param>
        /// <param name="loopCount">Number of times to play the animation.</param>
        /// <param name="ignoreBodyTrack">if set to <c>true</c> ignore the animation track for Vector’s body (i.e. the wheels / treads).</param>
        /// <param name="ignoreHeadTrack">if set to <c>true</c> ignore the animation track for Vector’s head.</param>
        /// <param name="ignoreLiftTrack">if set to <c>true</c> ignore the animation track for Vector’s lift.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="KeyNotFoundException">Unknown animation '{animationName}'</exception>
        public async Task<StatusCode> PlayAnimation(string animationName, uint loopCount = 1, bool ignoreBodyTrack = false, bool ignoreHeadTrack = false, bool ignoreLiftTrack = false)
        {
            var animations = await GetAnimations().ConfigureAwait(false);
            if (!animations.ContainsKey(animationName)) throw new KeyNotFoundException($"Unknown animation '{animationName}'");
            return await PlayAnimation(animations[animationName], loopCount, ignoreBodyTrack, ignoreHeadTrack, ignoreLiftTrack).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts an animation trigger playing on a robot.
        /// <para>Playing a trigger requests that an animation of a certain class starts playing, rather than an exact animation name.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="animationTrigger">The animation trigger to play.</param>
        /// <param name="loopCount">The loop count.</param>
        /// <param name="useLiftSafe">if set to <c>true</c> automatically ignore the lift track if Vector is currently carrying an object.</param>
        /// <param name="ignoreBodyTrack">if set to <c>true</c> ignore the animation track for Vector’s body (i.e. the wheels / treads).</param>
        /// <param name="ignoreHeadTrack">if set to <c>true</c> ignore the animation track for Vector’s head.</param>
        /// <param name="ignoreLiftTrack">if set to <c>true</c> ignore the animation track for Vector’s lift.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> PlayAnimationTrigger(AnimationTrigger animationTrigger, uint loopCount = 1, bool useLiftSafe = false, bool ignoreBodyTrack = false, bool ignoreHeadTrack = false, bool ignoreLiftTrack = false)
        {
            var response = await Robot.RunControlMethod(client => client.PlayAnimationTriggerAsync(new PlayAnimationTriggerRequest()
            {
                AnimationTrigger = animationTrigger.ToRobotAnimationTrigger(),
                Loops = loopCount,
                UseLiftSafe = useLiftSafe,
                IgnoreBodyTrack = ignoreBodyTrack,
                IgnoreHeadTrack = ignoreHeadTrack,
                IgnoreLiftTrack = ignoreLiftTrack
            }
            )).ConfigureAwait(false);
            StartAnimationTracking();
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Starts an animation trigger playing on a robot.
        /// <para>Playing a trigger requests that an animation of a certain class starts playing, rather than an exact animation name.</para>
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="animationTriggerName">Name of the animation trigger to play.</param>
        /// <param name="loopCount">The loop count.</param>
        /// <param name="useLiftSafe">if set to <c>true</c> automatically ignore the lift track if Vector is currently carrying an object.</param>
        /// <param name="ignoreBodyTrack">if set to <c>true</c> ignore the animation track for Vector’s body (i.e. the wheels / treads).</param>
        /// <param name="ignoreHeadTrack">if set to <c>true</c> ignore the animation track for Vector’s head.</param>
        /// <param name="ignoreLiftTrack">if set to <c>true</c> ignore the animation track for Vector’s lift.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> PlayAnimationTrigger(string animationTriggerName, uint loopCount = 1, bool useLiftSafe = false, bool ignoreBodyTrack = false, bool ignoreHeadTrack = false, bool ignoreLiftTrack = false)
        {
            var animationTriggers = await GetAnimationTriggers().ConfigureAwait(false);
            if (!animationTriggers.ContainsKey(animationTriggerName)) throw new KeyNotFoundException($"Unknown animation trigger '{animationTriggerName}'");
            return await PlayAnimationTrigger(animationTriggers[animationTriggerName], loopCount, useLiftSafe, ignoreBodyTrack, ignoreHeadTrack, ignoreLiftTrack).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts an animation or animation trigger playing on robot.
        /// </summary>
        /// <remarks>Requires behavior control; see <see cref="ControlComponent.RequestControl(int)"/>.</remarks>
        /// <param name="animationOrTrigger">The animation or animation trigger to play.</param>
        /// <param name="loopCount">The loop count.</param>
        /// <param name="useLiftSafe">if set to <c>true</c> automatically ignore the lift track if Vector is currently carrying an object.</param>
        /// <param name="ignoreBodyTrack">if set to <c>true</c> ignore the animation track for Vector’s body (i.e. the wheels / treads).</param>
        /// <param name="ignoreHeadTrack">if set to <c>true</c> ignore the animation track for Vector’s head.</param>
        /// <param name="ignoreLiftTrack">if set to <c>true</c> ignore the animation track for Vector’s lift.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        /// <exception cref="ArgumentException">Parameter is not an Animation or AnimationTrigger</exception>
        public async Task<StatusCode> Play(IAnimation animationOrTrigger, uint loopCount = 1, bool useLiftSafe = false, bool ignoreBodyTrack = false, bool ignoreHeadTrack = false, bool ignoreLiftTrack = false)
        {
            if (animationOrTrigger is Animation animation) return await PlayAnimation(animation, loopCount, ignoreBodyTrack, ignoreHeadTrack, ignoreLiftTrack).ConfigureAwait(false);
            if (animationOrTrigger is AnimationTrigger animationTrigger) return await PlayAnimationTrigger(animationTrigger, loopCount, useLiftSafe, ignoreBodyTrack, ignoreHeadTrack, ignoreLiftTrack).ConfigureAwait(false);
            throw new ArgumentException($"Parameter is not an Animation or AnimationTrigger");
        }

        /// <summary>
        /// Starts the animation tracking.
        /// </summary>
        private void StartAnimationTracking()
        {
            if (animationRunningResult != null) animationRunningResult.TrySetResult(true);
            lastAnimationStart = DateTime.Now;
            animationRunningResult = new TaskCompletionSource<bool>();
            OnPropertyChanged(nameof(IsAnimating));
        }

        /// <summary>
        /// Waits for the current animation to complete.
        /// <para>The task will complete when animation is finished.  If no animation is running, this method will return immediately.</para>
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result is false if no task was running.</returns>
        public Task<bool> WaitForAnimationCompletion()
        {
            if (animationRunningResult == null) return Task.FromResult(false);
            return animationRunningResult.Task;
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
            animationRunningResult?.TrySetCanceled();
            animationRunningResult = null;
            animations = null;
            animationTriggers = null;
            return Task.CompletedTask;
        }
    }
}
