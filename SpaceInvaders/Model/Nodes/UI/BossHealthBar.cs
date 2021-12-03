using System;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View.Sprites.UI;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Draws the boss' health bar
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.RenderableNode" />
    public class BossHealthBar : RenderableNode
    {
        #region Data members

        private const double BarWidth = 660;
        private const double FadeDuration = 4;

        private readonly BossHealthBarSprite healthBarSprite;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossHealthBar" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="boss">The boss.</param>
        /// <exception cref="System.ArgumentNullException">boss</exception>
        public BossHealthBar(Boss boss) : base(new BossHealthBarSprite(), RenderLayer.UiTop)
        {
            if (boss == null)
            {
                throw new ArgumentNullException(nameof(boss));
            }

            Sprite.Opacity = 0;
            this.healthBarSprite = Sprite as BossHealthBarSprite;

            boss.HealthChanged += this.onBossHealthChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            Sprite.Opacity = Math.Min(this.healthBarSprite.Opacity + delta / FadeDuration, 1);
            base.Update(delta);
        }

        private void onBossHealthChanged(object sender, double e)
        {
            if (e < 0)
            {
                return;
            }

            this.healthBarSprite.HealthBar.Width = BarWidth * e;
        }

        #endregion
    }
}