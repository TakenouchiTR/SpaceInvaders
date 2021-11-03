using System;
using SpaceInvaders.View.Sprites.Effects;

namespace SpaceInvaders.Model.Nodes.Effects
{
    public class MuzzleFlash: SpriteNode
    {
        public MuzzleFlash() : base(new MuzzleFlashSprite(), RenderLayer.MainTop)
        {
            this.setupTimer();
        }

        private void setupTimer()
        {
            var removalTimer = new Timer(.1, false);
            removalTimer.Start();
            removalTimer.Tick += this.onRemovalTimerTick;
            AttachChild(removalTimer);
        }

        private void onRemovalTimerTick(object sender, EventArgs e)
        {
            QueueForRemoval();
        }
    }
}
