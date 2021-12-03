using System;
using SpaceInvaders.Extensions;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A boss turret that fires rapid bursts of bullets
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.BossTurret" />
    public class BossMachineGunTurret : BossTurret
    {
        #region Data members

        private const double BurstDelay = 3;

        private Gun gun;
        private Timer burstTimer;
        private uint bulletsRemaining;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossMachineGunTurret" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="rotation">The rotation.</param>
        public BossMachineGunTurret(double rotation)
        {
            Sprite.Sprite.Rotation = (float) rotation.RadianToDegree();

            this.setupGun(rotation);
            this.setupTimer();
        }

        #endregion

        #region Methods

        private void setupGun(double rotation)
        {
            this.gun = new EnemyGun {
                Rotation = rotation,
                CooldownDuration = .25,
                MaxBulletsOnScreen = 8
            };

            this.gun.Shot += this.onGunShot;

            AttachChild(this.gun);
        }

        private void setupTimer()
        {
            this.burstTimer = new Timer(BurstDelay);
            this.burstTimer.Start();

            this.burstTimer.Tick += this.onBurstTimerTick;

            AttachChild(this.burstTimer);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (Active && this.bulletsRemaining > 0)
            {
                this.gun.Shoot();
            }

            base.Update(delta);
        }

        private void onBurstTimerTick(object sender, EventArgs e)
        {
            this.bulletsRemaining = this.gun.MaxBulletsOnScreen;
        }

        private void onGunShot(object sender, EventArgs e)
        {
            this.bulletsRemaining--;
        }

        #endregion
    }
}