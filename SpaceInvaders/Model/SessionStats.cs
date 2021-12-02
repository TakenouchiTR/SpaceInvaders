namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Stores information for the current session. Allows data to be maintained between levels.
    /// </summary>
    public static class SessionStats
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public static int Score { get; set; }

        /// <summary>
        ///     Gets or sets the play time.
        /// </summary>
        /// <value>
        ///     The play time.
        /// </value>
        public static double PlayTime { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Resets the stats to the default values.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Score == 0 &amp;&amp;<br />
        ///     PlayTime == 0
        /// </summary>
        public static void Reset()
        {
            Score = 0;
            PlayTime = 0;
        }

        #endregion
    }
}