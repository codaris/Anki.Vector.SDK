// <copyright file="Component.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
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
        /// Gets the robot instance
        /// </summary>
        internal Robot Robot { get ; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal Component(Robot robot) 
        {
            Robot = robot;
        }

        /// <summary>
        /// Called when disconnecting Robot
        /// </summary>
        /// <param name="forced">if set to <c>true</c> the shutdown is forced due to lost connection.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal abstract Task Teardown(bool forced);
    }
}
