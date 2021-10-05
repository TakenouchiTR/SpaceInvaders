using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Effects
{
    public class Explosion : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Explosion"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public Explosion(GameManager manager) : base(manager, new ExplosionSprite())
        {
            UpdateTimer growTimer = new UpdateTimer(manager) {
                Repeat = false,
                Duration = .1
            };
            UpdateTimer removalTimer = new UpdateTimer(manager) {
                Repeat = false,
                Duration = .2
            };

            growTimer.Tick += this.onGrowTimerTick;
            removalTimer.Tick += this.onRemovalTimerTick;

            AttachChild(growTimer);
            AttachChild(removalTimer);

            growTimer.Start();
            removalTimer.Start();
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