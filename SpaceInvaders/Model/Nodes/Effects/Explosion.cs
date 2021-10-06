using System;
using Windows.UI.Xaml.Media;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    public class Explosion : SpriteNode
    {
        public Explosion() : base(new ExplosionSprite())
        {
            this.addTimers();
        }

        private void addTimers()
        {
            Timer growTimer = new Timer() {
                Duration = .1,
                Repeat = false
            };
            Timer removalTimer = new Timer() {
                Duration = .2,
                Repeat = false
            };

            growTimer.Tick += this.onGrowTimerTick;
            removalTimer.Tick += this.onRemovalTimerTick;

            AttachChild(growTimer);
            AttachChild(removalTimer);
        }

        private void onRemovalTimerTick(object sender, EventArgs e)
        {
            this.Sprite.RenderTransform = new ScaleTransform();
        }

        private void onGrowTimerTick(object sender, EventArgs e)
        {
            this.QueueForRemoval();
        }
    }
}
