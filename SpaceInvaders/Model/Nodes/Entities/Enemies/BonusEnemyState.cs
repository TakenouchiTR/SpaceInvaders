namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     Represents the different game states for the BonusEnemy
    /// </summary>
    public enum BonusEnemyState
    {
        /// <summary>
        ///     When the enemy is entering the screen
        /// </summary>
        EnteringPlay,

        /// <summary>
        ///     When the enemy is actively on the screen
        /// </summary>
        OnScreen,

        /// <summary>
        ///     When the enemy is leaving the screen
        /// </summary>
        ExitingPlay
    }
}