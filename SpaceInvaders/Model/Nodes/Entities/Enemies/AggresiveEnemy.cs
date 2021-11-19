using System;
using System.Collections.Generic;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     An enemy that fires in irregular intervals
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class AggresiveEnemy : Enemy
    {
        #region Data members

        private const double MinShotDelay = 3;
        private const double MaxShotDelay = 8;
        private static readonly Random ShotDelayGenerator = new Random();

        private Gun gun;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggresiveEnemy" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 3 &amp;&amp;<br />
        ///     this.Score == 30 &amp;&amp;<br />
        ///     this.Sprite is AggresiveEnemySprite
        /// </summary>
        public AggresiveEnemy() : base(createSprite())
        {
            Collision.Collided += this.onCollided;
            Score = 30;

            this.setupGun();
        }

        #endregion

        #region Methods

        private static AnimatedSprite createSprite()
        {
            var sprites = new List<BaseSprite> {
                new AggressiveEnemySprite1(),
                new AggressiveEnemySprite2(),
                new AggressiveEnemySprite3()
            };

            return new AnimatedSprite(1, sprites);
        }

        private void setupGun()
        {
            var initialCooldown = getShotDelay();

            this.gun = new EnemyGun {
                Position = Center,
                CooldownDuration = initialCooldown
            };

            this.gun.Shot += this.onGunShot;

            AttachChild(this.gun);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            this.gun.Shoot();
            base.Update(delta);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        private void onGunShot(object sender, EventArgs e)
        {
            this.gun.CooldownDuration = getShotDelay();
        }

        private static double getShotDelay()
        {
            return ShotDelayGenerator.NextDouble() * (MaxShotDelay - MinShotDelay) + MinShotDelay;
        }

        #endregion
    }
}