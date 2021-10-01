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

        public override void Update(double delta)
        {
        }

        #endregion
    }
}