// <copyright file="UserIntentEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// List of UserIntent events available to the SDK.
    /// <para>Vector's voice recognition allows for variation in grammar and word selection, so the examples are not the only way to invoke the voice commands.</para>
    /// <para>This list reflect only the voice commands available to the SDK, as some are not available for development use.</para>
    /// </summary>
    public enum UserIntent
    {
        /// <summary>example  "How old are you?"</summary>
        CharacterAge = 0,
        /// <summary>example  "Check the timer."</summary>
        CheckTimer = 1,
        /// <summary>example  "Go explore."</summary>
        ExploreStart = 2,
        /// <summary>example  "Stop the timer."</summary>
        GlobalStop = 3,
        /// <summary>example  "Goodbye!"</summary>
        GreetingGoodbye = 4,
        /// <summary>example  "Good morning!"</summary>
        GreetingGoodmorning = 5,
        /// <summary>example  "Hello!"</summary>
        GreetingHello = 6,
        /// <summary>example  "I hate you."</summary>
        ImperativeAbuse = 7,
        /// <summary>example  "Yes."</summary>
        ImperativeAffirmative = 8,
        /// <summary>example  "I'm sorry."</summary>
        ImperativeApology = 9,
        /// <summary>example  "Come here."</summary>
        ImperativeCome = 10,
        /// <summary>example  "Dance."</summary>
        ImperativeDance = 11,
        /// <summary>example  "Fetch your cube."</summary>
        ImperativeFetchCube = 12,
        /// <summary>example  "Find your cube."</summary>
        ImperativeFindCube = 13,
        /// <summary>example  "Look at me."</summary>
        ImperativeLookAtMe = 14,
        /// <summary>example  "I love you."</summary>
        ImperativeLove = 15,
        /// <summary>example  "Good Robot."</summary>
        ImperativePraise = 16,
        /// <summary>example  "No."</summary>
        ImperativeNegative = 17,
        /// <summary>example  "Bad Robot."</summary>
        ImperativeScold = 18,
        /// <summary>example  "Volume 2."</summary>
        ImperativeVolumeLevel = 19,
        /// <summary>example  "Volume up."</summary>
        ImperativeVolumeUp = 20,
        /// <summary>example  "Volume down."</summary>
        ImperativeVolumeDown = 21,
        /// <summary>example  "Go forward."</summary>
        MovementForward = 22,
        /// <summary>example  "Go backward."</summary>
        MovementBackward = 23,
        /// <summary>example  "Turn left."</summary>
        MovementTurnLeft = 24,
        /// <summary>example  "Turn right."</summary>
        MovementTurnRight = 25,
        /// <summary>example  "Turn around."</summary>
        MovementTurnAround = 26,
        /// <summary>example  "I have a question."</summary>
        KnowledgeQuestion = 27,
        /// <summary>example  "What's my name?"</summary>
        NamesAsk = 28,
        /// <summary>example  "Play a game."</summary>
        PlayAnyGame = 29,
        /// <summary>example  "Play a trick."</summary>
        PlayAnyTrick = 30,
        /// <summary>example  "Let's play Blackjack."</summary>
        PlayBlackjack = 31,
        /// <summary>example  "Fist bump."</summary>
        PlayFistBump = 32,
        /// <summary>example  "Pick up your cube."</summary>
        PlayPickupCube = 33,
        /// <summary>example  "Pop a wheelie."</summary>
        PlayPopAWheelie = 34,
        /// <summary>example  "Roll your cube."</summary>
        PlayRollCube = 35,
        /// <summary>example  "Happy holidays!"</summary>
        SeasonalHappyHolidays = 36,
        /// <summary>example  "Happy new year!"</summary>
        SeasonalHappyNewYear = 37,
        /// <summary>example  "Set timer for 10 minutes"</summary>
        SetTimer = 38,
        /// <summary>example  "What time is it?"</summary>
        ShowClock = 39,
        /// <summary>example  "Take a selfie."</summary>
        TakePhoto = 40,
        /// <summary>example  "What is the weather report?"</summary>
        WeatherResponse = 41
    }

    /// <summary>
    /// User intent event args.
    /// <para>Contains the voice command information from the event stream.  The <see cref="UserIntent"/> enumeration includes all of the voice 
    /// commands that the SDK can intercept.</para>
    /// <para>Some UserIntents include information returned from the cloud and used when evaluating the voice commands.  
    /// This information is available in the <see cref="IntentData"/> JSON formatted string.</para>
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class UserIntentEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the voice command user intent type
        /// </summary>
        public UserIntent Intent { get; }

        /// <summary>
        /// Gets the  voice command specific data in JSON format.
        /// <para>Some voice commands contain information from processing.  For example, asking Vector "Hey Vector, what is the weather?" will 
        /// return the current location and the weather forecast.</para>
        /// <para>This value will be empty for voice commands without additional information.</para>
        /// </summary>
        public string IntentData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserIntentEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal UserIntentEventArgs(ExternalInterface.Event e) : base(e)
        {
            Intent = (UserIntent)e.UserIntent.IntentId;
            IntentData = e.UserIntent.JsonData;
        }
    }
}
