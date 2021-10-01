﻿using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    /// <summary>
    ///     The most basic enemy. Has no additional functionality by default
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.Enemies.BasicEnemy" />
    public class BasicEnemy : Enemy
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasicEnemy" /> class.
        /// </summary>
        /// <param name="manager">The Manager.</param>
        public BasicEnemy(GameManager manager) : base(manager, new BasicEnemySprite())
        {
            Score = 10;
        }

        #endregion

        #region Methods

        public override void Update(double delta)
        {
        }

        #endregion
    }
}