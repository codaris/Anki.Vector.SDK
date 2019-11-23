// <copyright file="Component.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector
{
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract base class for components.
    /// </summary>
    /// <seealso cref="Anki.Vector.RobotObject" />
    public abstract class Component : RobotObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal Component(Robot robot) : base(robot)
        {
        }

        /// <summary>
        /// Called when disconnecting Robot
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal abstract Task Teardown();
    }
}
