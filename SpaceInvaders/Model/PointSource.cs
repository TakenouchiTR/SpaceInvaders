namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Represents the source of earned points
    /// </summary>
    public enum PointSource
    {
        /// <summary>
        ///     Points that were carried over from the previous level
        /// </summary>
        PreviousLevel,

        /// <summary>
        ///     Points earned by killing enemies
        /// </summary>
        Enemy,

        /// <summary>
        ///     Points earned for having extra lives remaining
        /// </summary>
        Lives,

        /// <summary>
        ///     Points earned by grazing bullets
        /// </summary>
        Graze,

        /// <summary>
        ///     Points earned by clearing a level quickly
        /// </summary>
        Time,

        /// <summary>
        ///     Points earned by having shield segments survive the level
        /// </summary>
        Shields
    }
}