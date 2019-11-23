// <copyright file="RobotObject.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Anki.Vector
{
    /// <summary>
    /// Abstract base class for all robot owned objects.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class RobotObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObject"/> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal RobotObject(Robot robot)
        {
            this.robot = robot;
        }

        /// <summary>
        /// The robot instance
        /// </summary>
        private readonly Robot robot = null;

        /// <summary>
        /// Gets the robot instance
        /// </summary>
        internal Robot Robot { get => robot ?? (this as Robot); }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the property value and raises the changed event.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>True if property was changed</returns>
        internal bool SetProperty<T>(ref T field, T value, [CallerMemberName]string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        /// <summary>
        /// Called when the property changed.
        /// </summary>
        /// <param name="name">The property name.</param>
        internal void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Raise(this, new PropertyChangedEventArgs(name));
        }
    }
}
