using System;
using System.Collections.Generic;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     An enemy that fires frequently in irregular intervals.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class MasterEnemy : Enemy
    {
        #region Data members

        private const double MinShotDelay = 2;
        private const double MaxShotDelay = 6;
        private static readonly Random ShotDelayGenerator = new Random();

        private Gun gun;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MasterEnemy" /> class.
        /// </summary>
        public MasterEnemy() : base(createSprite())
        {
            Score = 40;
            Collision.Collided += this.onCollided;

            this.setupGun();
        }

        #endregion

        #region Methods

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

        private static AnimatedSprite createSprite()
        {
            var sprites = new List<BaseSprite> {
                new MasterEnemySprite1(),
                new MasterEnemySprite2()
            };

            return new AnimatedSprite(1, sprites);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            var player = (PlayerShip) GetRoot().GetChildByName("PlayerShip");

            if (player == null)
            {
                return;
            }

            this.gun.Rotation = Center.AngleToTarget(player.Center);
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