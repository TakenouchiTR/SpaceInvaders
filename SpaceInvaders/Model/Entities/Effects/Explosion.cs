using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Effects
{
    public class Explosion : GameObject
    {
        #region Data members

        private readonly DispatcherTimer removalTimer;
        private readonly DispatcherTimer growTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Explosion"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public Explosion(GameManager manager) : base(manager, new ExplosionSprite())
        {
            this.removalTimer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(.25)
            };
            this.growTimer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(.1)
            };

            this.removalTimer.Tick += this.onRemovalTimerTick;
            this.growTimer.Tick += this.onGrowTimerTick;

            this.removalTimer.Start();
            this.growTimer.Start();
        }

        #endregion

        #region Methods

        private void onGrowTimerTick(object sender, object e)
        {
            if (Sprite is ExplosionSprite explosionSprite)
            {
                explosionSprite.RenderTransform = new ScaleTransform();
            }

            this.growTimer.Stop();
            this.growTimer.Tick -= this.onGrowTimerTick;
        }

        private void onRemovalTimerTick(object sender, object e)
        {
            QueueRemoval();
            this.removalTimer.Stop();
            this.removalTimer.Tick -= this.onRemovalTimerTick;
        }

        /// <summary>
        /// The update loop for the GameObject.
        /// Precondition: None
        /// Postcondition: GameObject completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
        }

        #endregion
    }
}