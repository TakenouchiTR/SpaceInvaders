using SpaceInvaders.Extensions;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A turret that fires a volley of bullets
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.BossTurret" />
    public class BossScatterTurret : BossTurret
    {
        #region Data members

        private ScatterGun gun;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossScatterTurret" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <exception cref="System.ArgumentNullException">player</exception>
        public BossScatterTurret()
        {
            Sprite.Sprite.Rotation = (float) Vector2.Down.ToAngle().RadianToDegree();

            this.setupGun();
        }

        #endregion

        #region Methods

        private void setupGun()
        {
            this.gun = new ScatterGun(PhysicsLayer.EnemyHitbox, PhysicsLayer.Player | PhysicsLayer.World,
                "enemy_shot.wav") {
                CooldownDuration = 1.5,
                Rotation = Vector2.Down.ToAngle()
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
            if (Active && this.gun.CanShoot)
            {
                this.gun.Shoot();
            }

            base.Update(delta);
        }

        #endregion
    }
}