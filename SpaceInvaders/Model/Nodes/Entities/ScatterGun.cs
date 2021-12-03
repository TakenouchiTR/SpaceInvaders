using System;
using static SpaceInvaders.Extensions.DoubleExtensions;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A special gun that fires a spread of three projectiles
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Gun" />
    public class ScatterGun : Gun
    {
        #region Data members

        private const double Spread = 10;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScatterGun" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="collisionLayers">The collision layers.</param>
        /// <param name="collisionMasks">The collision masks.</param>
        /// <param name="gunShotFile">The filename for the gun shot audio.</param>
        public ScatterGun(PhysicsLayer collisionLayers, PhysicsLayer collisionMasks, string gunShotFile) : base(
            collisionLayers, collisionMasks, gunShotFile)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Attempts to shoot the gun, if able.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.CanShoot == false
        /// </summary>
        public override void Shoot()
        {
            if (!CanShoot)
            {
                return;
            }

            CooldownTimer.Start();

            var spreadInRad = Spread.DegreeToRadian();
            var shotAngle = Rotation - spreadInRad;

            for (var i = 0; i < 3; i++)
            {
                var bullet = CreateBullet(shotAngle);
                shotAngle += spreadInRad;

                bullet.Removed += this.onBulletRemoved;
                GetRoot().QueueNodeForAddition(bullet);
                ActiveBullets++;
            }

            var flash = CreateMuzzleFlash();

            ShotPlayer.Play();

            EmitShot();
            QueueNodeForAddition(flash);
        }

        private void onBulletRemoved(object sender, EventArgs e)
        {
            if (!(sender is Bullet bullet))
            {
                return;
            }

            ActiveBullets--;
            bullet.Removed -= this.onBulletRemoved;
        }

        #endregion
    }
}