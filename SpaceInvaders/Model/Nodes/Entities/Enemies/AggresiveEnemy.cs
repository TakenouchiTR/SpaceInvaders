using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     An enemy that fires in irregular intervals
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class AggresiveEnemy : Enemy
    {
        #region Data members

        private const double MinShotDelay = 2;
        private const double MaxShotDelay = 4.5;
        private static readonly Random ShotDelayGenerator = new Random();
        private static readonly Vector2 BulletSpawnLocation = new Vector2(12, 40);

        private Timer shotTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggresiveEnemy" /> class.
        /// </summary>
        public AggresiveEnemy() : base(new AggresiveEnemySprite())
        {
            Collision.Collided += this.onCollided;
            Score = 30;

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

        private void onShotTimerTick(object sender, EventArgs e)
        {
            var bullet = new EnemyBullet {
                Position = Position + BulletSpawnLocation,
            };
            this.shotTimer.Duration = getShotDelay();

            GetRoot().QueueGameObjectForAddition(bullet);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        private static double getShotDelay()
        {
            return ShotDelayGenerator.NextDouble() * (MaxShotDelay - MinShotDelay) + MinShotDelay;
        }

        #endregion
    }
}