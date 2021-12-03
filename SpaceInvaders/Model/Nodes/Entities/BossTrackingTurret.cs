using System;
using SpaceInvaders.Extensions;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A turret that tracks the player
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.BossTurret" />
    public class BossTrackingTurret : BossTurret
    {
        #region Data members

        private PlayerShip player;
        private Gun gun;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossTrackingTurret" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="player">The player.</param>
        /// <exception cref="System.ArgumentNullException">player</exception>
        public BossTrackingTurret(PlayerShip player)
        {
            this.player = player ?? throw new ArgumentNullException(nameof(player));

            Sprite.Sprite.Rotation = (float) Vector2.Down.ToAngle().RadianToDegree();

            this.player.Removed += this.onPlayerRemoved;

            this.setupGun();
        }

        #endregion

        #region Methods

        private void setupGun()
        {
            this.gun = new EnemyGun {
                CooldownDuration = 2
            };

            this.gun.ActivateCooldown();

            AttachChild(this.gun);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (Active && this.gun.CanShoot && this.player != null)
            {
                this.gun.Rotation = Position.AngleToTarget(this.player.Center);
                Sprite.Sprite.Rotation = (float) this.gun.Rotation.RadianToDegree();
                this.gun.Shoot();
            }

            base.Update(delta);
        }

        private void onPlayerRemoved(object sender, EventArgs e)
        {
            this.player = null;
        }

        #endregion
    }
}