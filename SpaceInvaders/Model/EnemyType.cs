namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Represents a type of enemy
    /// </summary>
    public enum EnemyType
    {
        /// <summary>
        ///     A basic enemy with no offensive abilities
        /// </summary>
        BasicEnemy,

        /// <summary>
        ///     An intermediate enemy with no offensive abilities
        /// </summary>
        IntermediateEnemy,

        /// <summary>
        ///     An aggresive enemy that is capable of firing downward
        /// </summary>
        AggressiveEnemy,

        /// <summary>
        ///     A master enemy that is capable of both firing directly at the player and<br />
        ///     moving to perform a swooping attack.
        /// </summary>
        MasterEnemy,

        /// <summary>
        ///     A fast moving enemy that will shoot at the player.<br />
        ///     Provides a temporary bonus when destroyed.
        /// </summary>
        BonusEnemy
    }
}