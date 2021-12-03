namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Contains the information related to a score
    /// </summary>
    public class ScoreEntry
    {
        #region Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; }

        /// <summary>
        ///     Gets the time.
        /// </summary>
        /// <value>
        ///     The time.
        /// </value>
        public int Level { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScoreEntry" /> class.<br />
        ///     Precondition: name != null<br />
        ///     Postcondition: this.Name == name &amp;&amp;<br />
        ///     this.Score == score &amp;&amp;<br />
        ///     this.Time == time
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="score">The score.</param>
        /// <param name="level">The level.</param>
        public ScoreEntry(string name, int score, int level)
        {
            this.Name = name;
            this.Score = score;
            this.Level = level;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Converts to string.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Name}: {this.Score}pts; Level {this.Level}";
        }

        #endregion
    }
}