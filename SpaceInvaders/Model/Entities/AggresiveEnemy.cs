using System;
using Windows.UI.Xaml;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     A basic enemy with the ability to shoot at random intervals
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.Enemy" />
    public class AggresiveEnemy : Enemy
    {
        #region Data members

        private const double MinShotDelay = 1;
        private const double MaxShotDelay = 3;
        private static readonly Random ShootRandom = new Random();

        private readonly Vector2 bulletSpawnLocation = new Vector2(22, 40);

        private readonly DispatcherTimer shootTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggresiveEnemy" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public AggresiveEnemy(GameManager parent) : base(parent, new BasicEnemySprite())
        {
            Score = 30;
            this.shootTimer = new DispatcherTimer();
            this.shootTimer.Interval = TimeSpan.FromSeconds(getNextShotDelay());
            this.shootTimer.Tick += this.onShootTimerTick;
            this.shootTimer.Start();
            Removed += this.onRemoved;
        }

        #endregion

        #region Methods

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
            var result = ShootRandom.NextDouble();
            result *= MaxShotDelay - MinShotDelay;
            result += MinShotDelay;

            return result;
        }

        #endregion
    }
}