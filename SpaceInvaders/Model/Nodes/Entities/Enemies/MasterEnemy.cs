using System;
using System.Collections.Generic;
using SpaceInvaders.Model.Nodes.Effects;
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
        private static readonly Vector2 BulletSpawnLocation = new Vector2(11, 44);
        private static readonly Vector2 MuzzleFlashLocation = new Vector2(21, 44);

        private Timer shotTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MasterEnemy" /> class.
        /// </summary>
        public MasterEnemy() : base(createSprite())
        {
            Score = 40;
            Collision.Collided += this.onCollided;

            this.initializeTimer();
        }

        #endregion

        #region Methods

        private void initializeTimer()
        {
            this.shotTimer = new Timer {
                Duration = getShotDelay(),
                Repeat = true
            };

            this.shotTimer.Tick += this.onShotTimerTick;
            this.shotTimer.Start();

            AttachChild(this.shotTimer);
        }

        private static AnimatedSprite createSprite()
        {
            var sprites = new List<BaseSprite> {
                new MasterEnemySprite1(),
                new MasterEnemySprite2()
            };

            return new AnimatedSprite(1, sprites);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        private void onShotTimerTick(object sender, EventArgs e)
        {
            var bullet = new EnemyBullet {
                Position = Position + BulletSpawnLocation
            };
            var flash = new MuzzleFlash {
                Position = Position + MuzzleFlashLocation,
                Sprite = {
                    Rotation = 180
                }
            };

            this.shotTimer.Duration = getShotDelay();

            GetRoot().QueueNodeForAddition(bullet);
            QueueNodeForAddition(flash);
        }

        private static double getShotDelay()
        {
            return ShotDelayGenerator.NextDouble() * (MaxShotDelay - MinShotDelay) + MinShotDelay;
        }

        #endregion
    }
}