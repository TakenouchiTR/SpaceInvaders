namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     Represents the state of the master enemy
    /// </summary>
    public enum MasterEnemyState
    {
        /// <summary>
        ///     When the enemy is in formation
        /// </summary>
        InFormation,

        /// <summary>
        ///     When the enemy is charging the player
        /// </summary>
        Charging,

        /// <summary>
        ///     When the enemy is returning to formation
        /// </summary>
        Returning
    }
}