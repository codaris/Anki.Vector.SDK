// <copyright file="Animation.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Animation that can be played by the robot.
    /// </summary>
    public class Animation : IAnimation
    {
        /// <summary>
        /// The underlying robot animation
        /// </summary>
        private readonly ExternalInterface.Animation robotAnimation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="robotAnimation">The robot animation.</param>
        internal Animation(ExternalInterface.Animation robotAnimation)
        {
            this.robotAnimation = robotAnimation;
        }

        /// <summary>
        /// Gets the name of the animation
        /// </summary>
        public string Name => robotAnimation.Name;

        /// <summary>
        /// Gets the friendly name of the animation
        /// </summary>
        public string FriendlyName
        {
            get
            {
                var name = robotAnimation.Name.Replace("anim_", "");
                name = name.Replace("_", " ");
                name = name.Replace("victor", "");
                name = Regex.Replace(name, @"0([0-9]+)", m => "(" + m.Groups[1].Value + ")");
                name = Regex.Replace(name, @"(\b[a-z](?!\s))", m => m.Value.ToUpperInvariant());
                return name;
            }
        }

        /// <summary>
        /// Converts to a robot animation.
        /// </summary>
        /// <returns>An SDK animation instance</returns>
        internal ExternalInterface.Animation ToRobotAnimation()
        {
            return this.robotAnimation;
        }
    }
}
