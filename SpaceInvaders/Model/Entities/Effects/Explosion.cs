using System;
using System.Diagnostics;
using SpaceInvaders.View.Sprites;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SpaceInvaders.Model.Entities.Effects
{
    public class Explosion : GameObject
    {
        private readonly DispatcherTimer removalTimer;
        private readonly DispatcherTimer growTimer;

        public Explosion(GameManager gameManager) : base(gameManager, new ExplosionSprite())
        {
            this.removalTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(.25)
            };
            this.growTimer = new DispatcherTimer 
            {
                Interval = TimeSpan.FromSeconds(.1)
            };

            this.removalTimer.Tick += onRemovalTimerTick;
            this.growTimer.Tick += onGrowTimerTick;

            this.removalTimer.Start();
            this.growTimer.Start();
        }

        private void onGrowTimerTick(object sender, object e)
        {
            if (this.Sprite is ExplosionSprite explosionSprite)
            {
                explosionSprite.RenderTransform = new ScaleTransform();
            }
            this.growTimer.Stop();
            this.growTimer.Tick -= this.onGrowTimerTick;
        }

        private void onRemovalTimerTick(object sender, object e)
        {
            this.QueueRemoval();
            this.removalTimer.Stop();
            this.removalTimer.Tick -= this.onRemovalTimerTick;
        }

        public override void Update(double delta)
        {

        }
    }
}
