using System;
using SpaceInvaders.View.Sprites.Effects;

namespace SpaceInvaders.Model.Nodes.Effects
{
    /// <summary>
    ///     Plays a quick muzzle flash effect
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.SpriteNode" />
    public class MuzzleFlash : SpriteNode
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MuzzleFlash" /> class.
        /// </summary>
        public MuzzleFlash() : base(new MuzzleFlashSprite(), RenderLayer.MainTop)
        {
            this.setupTimer();
        }

        #endregion

        #region Methods

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

        #endregion
    }
}