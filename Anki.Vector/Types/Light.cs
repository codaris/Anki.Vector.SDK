// <copyright file="Light.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// Lights are used with Vector's Cube.
    /// <para>Lights may either be "on" or "off", though in practice any colors may be assigned to either state (including no color/light).</para>
    /// </summary>
    public class Light
    {
        /// <summary>
        /// Gets or sets the color shown when the light is on.
        /// </summary>
        public Color OnColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the color shown when the light is off.
        /// </summary>
        public Color OffColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the number of milliseconds the light should be "on" for for each cycle.
        /// </summary>
        public uint OnPeriodMs { get; set; } = 250;

        /// <summary>
        /// Gets or sets the number of milliseconds the light should be "off" for for each cycle.
        /// </summary>
        public uint OffPeriodMs { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of milliseconds to take to transition the light to the on color.
        /// </summary>
        public uint TransitionOnPeriodMs { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of milliseconds to take to transition the light to the off color.
        /// </summary>
        public uint TransitionOffPeriodMs { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        public Light()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        /// <param name="onColor">Color shown when the light is on.</param>
        public Light(Color onColor)
        {
            OnColor = onColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        /// <param name="onColor">Color shown when the light is on.</param>
        /// <param name="offColor">Color shown when the light is off.</param>
        /// <param name="onPeriodMs">The number of milliseconds the light should be "on" for for each cycle.</param>
        /// <param name="offPeriodMs">The number of milliseconds the light should be "off" for for each cycle.</param>
        /// <param name="transitionOnPeriodMs">The number of milliseconds to take to transition the light to the on color.</param>
        /// <param name="transitionOffPeriodMs">The number of milliseconds to take to transition the light to the off color.</param>
        public Light(Color onColor, Color offColor, uint onPeriodMs, uint offPeriodMs, uint transitionOnPeriodMs, uint transitionOffPeriodMs)
        {
            OnColor = onColor;
            OffColor = offColor;
            OnPeriodMs = onPeriodMs;
            OffPeriodMs = offPeriodMs;
            TransitionOnPeriodMs = transitionOnPeriodMs;
            TransitionOffPeriodMs = transitionOffPeriodMs;
        }

        /// <summary>
        /// Add this light to the request
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="colorProfile">The color profile.</param>
        internal void AddToRequest(ExternalInterface.SetCubeLightsRequest request, ColorProfile colorProfile)
        {
            request.OnColor.Add(colorProfile.AugmentColor(OnColor).ToRobotColor());
            request.OffColor.Add(colorProfile.AugmentColor(OffColor).ToRobotColor());
            request.OnPeriodMs.Add(OnPeriodMs);
            request.OffPeriodMs.Add(OffPeriodMs);
            request.TransitionOnPeriodMs.Add(TransitionOnPeriodMs);
            request.TransitionOnPeriodMs.Add(TransitionOffPeriodMs);
            request.Offset.Add(0);
        }
    }
}
