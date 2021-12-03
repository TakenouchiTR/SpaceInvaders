using System;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.View.Sprites.PowerUps;

namespace SpaceInvaders.Model.Nodes.PowerUps
{
    /// <summary>
    ///     Applies a reflective shield when attached to a player
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.PowerUps.PowerUp" />
    public class ReflectionShieldPowerUp : PowerUp
    {
        #region Data members

        private const double Duration = 4;

        private Timer removalTimer;
        private ReflectionShield shield;
        private ReflectiveShieldSprite shieldSprite;

        #endregion

        #region Methods

        /// <summary>
        ///     Runs the required code to attach and apply the power up to the player<br />
        ///     Precondition: player != null<br />
        ///     Postcondition: player has the power up applied
        /// </summary>
        /// <param name="player">The player.</param>
        /// <exception cref="System.ArgumentNullException">player</exception>
        public override void AttachToPlayer(PlayerShip player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            this.setupTimer();
            this.setupShield(player);

            player.Killed += this.onPlayerKilled;

            player.QueueNodeForAddition(this);
            player.QueueNodeForAddition(this.shield);
        }

        private void setupTimer()
        {
            this.removalTimer = new Timer(Duration) {
                Repeat = false
            };
            this.removalTimer.Tick += this.onRemovalTimerTick;
            this.removalTimer.Start();

            AttachChild(this.removalTimer);
        }

        private void setupShield(PlayerShip player)
        {
            this.shield = new ReflectionShield {
                Center = player.Center
            };
            this.shieldSprite = (ReflectiveShieldSprite) this.shield.Sprite.Sprite;
        }

        /// <summary>
        ///     Removes the power up from player.<br />
        ///     Precondition: None<br />
        ///     Postcondition: The power up is removed
        /// </summary>
        public override void RemoveFromPlayer()
        {
            this.shield.QueueForRemoval();
            QueueForRemoval();
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            this.shieldSprite.Opacity = this.removalTimer.TimeRemaining / this.removalTimer.Duration;
            base.Update(delta);
        }

        private void onRemovalTimerTick(object sender, EventArgs e)
        {
            this.RemoveFromPlayer();
        }

        private void onPlayerKilled(object sender, EventArgs e)
        {
            this.RemoveFromPlayer();
        }

        #endregion
    }
}