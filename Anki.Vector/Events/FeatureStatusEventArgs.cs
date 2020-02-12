namespace Anki.Vector.Events
{
    /// <summary>
    /// Feature status event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class FeatureStatusEventArgs : TimestampedStatusEventArgs
    {
        /// <summary>
        /// Gets the name of the feature
        /// </summary>
        public string FeatureName { get; }

        /// <summary>
        /// Gets the source of the feature invocation (voice, app, AI, unknown)
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureStatusEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal FeatureStatusEventArgs(ExternalInterface.Event e) : base(e)
        {
            var featureStatus = e.TimeStampedStatus.Status.FeatureStatus;
            FeatureName = featureStatus.FeatureName;
            Source = featureStatus.Source;
        }
    }
}
