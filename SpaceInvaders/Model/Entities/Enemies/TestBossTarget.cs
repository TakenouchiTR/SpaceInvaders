using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    public class TestBossTarget : GameObject
    {
        private int health = 3;
        private bool canShoot;
        private DispatcherTimer shootToggleTimer;
        private DispatcherTimer shootTimer;

        public TestBossTarget(GameManager gameManager) : base(gameManager, new BasicEnemySprite())
        {
            this.Monitoring = true;
            this.Monitorable = true;
            this.CollisionLayers = (int) PhysicsLayer.Enemy;
            this.CollisionMasks = (int) PhysicsLayer.PlayerHitbox;

            this.canShoot = false;
            this.shootToggleTimer = new DispatcherTimer();
            this.shootToggleTimer.Interval = TimeSpan.FromSeconds(2);
            this.shootToggleTimer.Tick += this.onShootToggleTimerTick;
            this.shootToggleTimer.Start();

            this.shootTimer = new DispatcherTimer();
            this.shootTimer.Interval = TimeSpan.FromSeconds(.5);
            this.shootTimer.Tick += this.onShooTimerTick;
            this.shootTimer.Start();

            this.Removed += this.onRemoved;
        }

        private void onRemoved(object sender, EventArgs e)
        {
            this.shootTimer.Stop();
            this.shootToggleTimer.Stop();
            this.shootTimer.Tick -= this.onShooTimerTick;
            this.shootToggleTimer.Tick -= this.onShootToggleTimerTick;
        }

        private void onShooTimerTick(object sender, object e)
        {
            if (this.canShoot)
            {
                var bullet = new EnemyBullet(gameManager)
                {
                    Position = this.Position
                };

                gameManager.QueueGameObjectForAddition(bullet);
            }
        }

        private void onShootToggleTimerTick(object sender, object e)
        {
            this.canShoot = !this.canShoot;
        }

        public override void Update(double delta)
        {

        }

        public override void HandleCollision(GameObject target)
        {
            this.health--;
            if (this.health <= 0)
            {
                this.QueueRemoval();

            }
        }
    }
}
