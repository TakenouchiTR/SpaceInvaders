using System;
using Windows.UI.Xaml.Media;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    /// <summary>
    ///     Plays a quick animation of an explosion
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.SpriteNode" />
    public class Explosion : SpriteNode
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Explosion" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Sprite is ExplosionSprite &amp;&amp;<br />
        ///     this.Children.Count == 2
        /// </summary>
        public Explosion() : base(new ExplosionSprite())
        {
            this.addTimers();
        }

        #endregion

        #region Methods

        private void addTimers()
        {
            var growTimer = new Timer {
                Duration = .1,
                Repeat = false
            };
            var removalTimer = new Timer {
                Duration = .2,
                Repeat = false
            };

            growTimer.Tick += this.onGrowTimerTick;
            removalTimer.Tick += this.onRemovalTimerTick;

            AttachChild(growTimer);
            AttachChild(removalTimer);

            growTimer.Start();
            removalTimer.Start();
        }

        private void onRemovalTimerTick(object sender, EventArgs e)
        {
            QueueForRemoval();
        }

        private void onGrowTimerTick(object sender, EventArgs e)
        {
            Sprite.RenderTransform = new ScaleTransform();
        }

        #endregion
    }
}