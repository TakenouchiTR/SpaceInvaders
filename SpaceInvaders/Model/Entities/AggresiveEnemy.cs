using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class AggresiveEnemy : Enemy
    {
        private static double minShotDelay = 1;
        private static double maxShotDelay = 3;
        private static Random shootRandom = new Random();

        private DispatcherTimer shootTimer;

        private readonly Vector2 bulletSpawnLocation = new Vector2(22, 40);
        public AggresiveEnemy(GameManager parent) : base(parent, new BasicEnemySprite())
        {
            Score = 30;
            this.shootTimer = new DispatcherTimer();
            this.shootTimer.Interval = TimeSpan.FromSeconds(getNextShotDelay());
            this.shootTimer.Tick += this.onShootTimerTick;
            this.shootTimer.Start();
            this.Removed += this.onRemoved;
        }

        private void onRemoved(GameObject sender, EventArgs e)
        {
            this.shootTimer.Stop();
        }

        private void onShootTimerTick(object sender, object e)
        {
            this.shootTimer.Interval = TimeSpan.FromSeconds(getNextShotDelay());
            var bullet = new EnemyBullet(parent) {
                Position = this.Position + this.bulletSpawnLocation
            };


            parent.QueueGameObjectForAddition(bullet);
        }

        public override void Update(double delta)
        {
            
        }

        private static double getNextShotDelay()
        {
            double result = shootRandom.NextDouble();
            result *= maxShotDelay - minShotDelay;
            result += minShotDelay;

            return result;
        }
    }
}
