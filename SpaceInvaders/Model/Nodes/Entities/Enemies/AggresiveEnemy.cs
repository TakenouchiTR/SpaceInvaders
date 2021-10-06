using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class AggresiveEnemy : Enemy
    {
        private const double MinShotDelay = 2;
        private const double MaxShotDelay = 4.5;
        private static readonly Random ShotDelayGenerator = new Random();
        private static readonly Vector2 BulletSpawnLocation = new Vector2(12, 40);

        private Timer shotTimer;

        public AggresiveEnemy() : base(new AggresiveEnemySprite())
        {
            this.Collision.Collided += this.onCollided;
            this.Score = 30;

            this.initializeTimer();
        }

        private void initializeTimer()
        {
            this.shotTimer = new Timer()
            {
                Duration = getShotDelay(),
                Repeat = true
            };

            this.shotTimer.Tick += this.onShotTimerTick;
            this.shotTimer.Start();

            AttachChild(shotTimer);
        }

        private void onShotTimerTick(object sender, EventArgs e)
        {
            Bullet bullet = new Bullet() 
            {
                Position = this.Position + BulletSpawnLocation,
                Velocity = new Vector2(0, 500)
            };
            bullet.Collision.CollisionLayers = PhysicsLayer.EnemyHitbox;
            bullet.Collision.CollisionMasks = PhysicsLayer.Player;

            this.shotTimer.Duration = getShotDelay();

            this.GetRoot().QueueGameObjectForAddition(bullet);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            this.QueueForRemoval();
        }

        private static double getShotDelay()
        {
            return ShotDelayGenerator.NextDouble() * (MaxShotDelay - MinShotDelay) + MinShotDelay;
        }
    }
}
