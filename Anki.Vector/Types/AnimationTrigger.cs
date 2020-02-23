// <copyright file="AnimationTrigger.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// Animation trigger that can be played by the robot.
    /// </summary>
    public class AnimationTrigger : IAnimation
    {
        /// <summary>
        /// The underlying robot animation trigger
        /// </summary>
        private readonly ExternalInterface.AnimationTrigger robotAnimationTrigger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationTrigger"/> class.
        /// </summary>
        /// <param name="robotAnimationTrigger">The robot animation trigger.</param>
        internal AnimationTrigger(ExternalInterface.AnimationTrigger robotAnimationTrigger)
        {
            this.robotAnimationTrigger = robotAnimationTrigger;
        }

        /// <summary>
        /// Gets the name of the animation trigger
        /// </summary>
        public string Name => robotAnimationTrigger.Name;

        /// <summary>
        /// Gets the friendly name of the animation trigger
        /// </summary>
        public string FriendlyName => robotAnimationTrigger.Name;

        /// <summary>
        /// Converts to a robot animation trigger
        /// </summary>
        /// <returns>An SDK animation trigger instance</returns>
        internal ExternalInterface.AnimationTrigger ToRobotAnimationTrigger()
        {
            return this.robotAnimationTrigger;
        }
    }
}
