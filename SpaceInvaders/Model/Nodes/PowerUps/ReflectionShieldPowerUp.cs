using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model.Nodes.Entities;

namespace SpaceInvaders.Model.Nodes.PowerUps
{
    /// <summary>
    ///     Applies a reflective shield when attached to a player
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.PowerUps.PowerUp" />
    public class ReflectionShieldPowerUp : PowerUp
    {
        private const double Duration = 4;

        private ReflectionShield shield;

        /// <summary>
        /// Runs the required code to attach and apply the power up to the player<br />
        /// Precondition: player != null<br />
        /// Postcondition: player has the power up applied
        /// </summary>
        /// <param name="player">The player.</param>
        /// <exception cref="System.ArgumentNullException">player</exception>
        public override void AttachToPlayer(PlayerShip player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var removalTimer = new Timer(Duration) 
            {
                Repeat = false
            };
            removalTimer.Tick += this.onRemovalTimerTick;
            removalTimer.Start();

            this.shield = new ReflectionShield {
                Center = player.Center
            };

            player.Killed += this.onPlayerKilled;

            this.AttachChild(removalTimer);
            player.QueueNodeForAddition(this);
            player.QueueNodeForAddition(this.shield);
        }

        /// <summary>
        /// Removes the power up from player.<br />
        /// Precondition: None<br />
        /// Postcondition: The power up is removed
        /// </summary>
        public override void RemoveFromPlayer()
        {
            this.shield.QueueForRemoval();
            QueueForRemoval();
        }
        
        private void onRemovalTimerTick(object sender, EventArgs e)
        {
            this.RemoveFromPlayer();
        }
        private void onPlayerKilled(object sender, EventArgs e)
        {
            this.RemoveFromPlayer();
        }

    }
}
