using System;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Represents each of the different physics layers.
    /// </summary>
    [Flags]
    public enum PhysicsLayer
    {
        /// <summary>
        ///     Represents the collision of any item that is part of the world (Obstacles, cover, etc.).
        /// </summary>
        World = 0b0000_0001,

        /// <summary>
        ///     Represents the collision of the player's body.
        /// </summary>
        Player = 0b0000_0010,

        /// <summary>
        ///     Represents the collision of the enemies' bodies.
        /// </summary>
        Enemy = 0b0000_0100,

        /// <summary>
        ///     Represents the collision of anything used by the player to damage the enemies.
        /// </summary>
        PlayerHitbox = 0b0000_1000,

        /// <summary>
        ///     Represents the collision of anything used by the enemies to damage the player.
        /// </summary>
        EnemyHitbox = 0b0001_0000
    }
}