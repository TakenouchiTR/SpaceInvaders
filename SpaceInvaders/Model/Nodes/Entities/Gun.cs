using System;
using SpaceInvaders.Extensions;
using SpaceInvaders.Model.Nodes.Effects;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     Handles the creations of bullets
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node2D" />
    public class Gun : Node2D
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the active bullets.
        /// </summary>
        /// <value>
        ///     The active bullets.
        /// </value>
        protected uint ActiveBullets { get; set; }

        /// <summary>
        ///     Gets or sets the angle that the bullet is fired.
        /// </summary>
        /// <value>
        ///     The rotation.
        /// </value>
        public double Rotation { get; set; }

        /// <summary>
        ///     Gets or sets the bullet speed.
        /// </summary>
        /// <value>
        ///     The bullet speed.
        /// </value>
        public double BulletSpeed { get; set; }

        /// <summary>
        ///     Gets or sets the maximum bullets on screen.
        /// </summary>
        /// <value>
        ///     The maximum bullets on screen.
        /// </value>
        public uint MaxBulletsOnScreen { get; set; }

        /// <summary>
        ///     Gets or sets the bullet's collision layers.
        /// </summary>
        /// <value>
        ///     The bullet's collision layers.
        /// </value>
        public PhysicsLayer BulletCollisionLayers { get; set; }

        /// <summary>
        ///     Gets or sets the bullet's collision masks.
        /// </summary>
        /// <value>
        ///     The bullet's collision masks.
        /// </value>
        public PhysicsLayer BulletCollisionMasks { get; set; }

        /// <summary>
        ///     Gets or sets the time between shots.
        /// </summary>
        /// <value>
        ///     The duration of the cooldown.
        /// </value>
        public double CooldownDuration
        {
            get => this.CooldownTimer.Duration;
            set => this.CooldownTimer.Duration = value;
        }

        /// <summary>
        ///     Gets or sets the cooldown timer.
        /// </summary>
        /// <value>
        ///     The cooldown timer.
        /// </value>
        protected Timer CooldownTimer { get; private set; }

        /// <summary>
        ///     Gets the shot player.
        /// </summary>
        /// <value>
        ///     The shot player.
        /// </value>
        protected SoundPlayer ShotPlayer { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance can shoot.<br />
        ///     The shot cooldown must not be active and there must be less than the maximum amount of shots active to be able to
        ///     shoot.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can shoot; otherwise, <c>false</c>.
        /// </value>
        public bool CanShoot => this.ActiveBullets < this.MaxBulletsOnScreen && !this.CooldownTimer.IsActive;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Gun" /> class.<br />
        ///     Precondition: cooldownDuration > 0<br />
        ///     Postcondition: this.Rotation == 0 &amp;&amp;<br />
        ///     this.BulletSpeed == 500 &amp;&amp;<br />
        ///     this.MaxBulletsOnScreen == 3 &amp;&amp;<br />
        ///     this.BulletCollisionLayers == collisionLayers &amp;&amp;<br />
        ///     this.BulletCollisionMasks == collisionMasks &amp;&amp;<br />
        ///     this.CooldownDuration == .25&amp;&amp;<br />
        ///     gunShotFile plays whenever a bullet is fired
        /// </summary>
        /// <param name="collisionLayers">The collision layers.</param>
        /// <param name="collisionMasks">The collision masks.</param>
        /// <param name="gunShotFile">The filename for the gun shot audio.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">cooldownDuration - cooldownDuration must be positive</exception>
        public Gun(PhysicsLayer collisionLayers, PhysicsLayer collisionMasks, string gunShotFile)
        {
            this.BulletCollisionLayers = collisionLayers;
            this.BulletCollisionMasks = collisionMasks;
            this.BulletSpeed = 500;
            this.MaxBulletsOnScreen = 3;

            this.setupTimer(.25);
            this.ShotPlayer = new SoundPlayer(gunShotFile);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when the Gun is [shot].
        /// </summary>
        public event EventHandler Shot;

        private void setupTimer(double cooldownDuration)
        {
            this.CooldownTimer = new Timer(cooldownDuration) {
                Repeat = false
            };

            AttachChild(this.CooldownTimer);
        }

        /// <summary>
        ///     Attempts to shoot the gun, if able.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.CanShoot == false
        /// </summary>
        public virtual void Shoot()
        {
            if (!this.CanShoot)
            {
                return;
            }

            this.CooldownTimer.Start();
            this.ActiveBullets++;

            var bullet = this.CreateBullet(this.Rotation);
            var flash = this.CreateMuzzleFlash();

            this.ShotPlayer.Play();

            GetRoot().QueueNodeForAddition(bullet);
            QueueNodeForAddition(flash);

            bullet.Removed += this.onBulletRemoved;
            this.Shot?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Creates the bullet with a specified rotation
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="rotation">The rotation.</param>
        /// <returns>A new bullet</returns>
        protected Bullet CreateBullet(double rotation)
        {
            var bulletVelocity = Vector2.AngleToNormalizedVector(rotation) * this.BulletSpeed;
            var bullet = new Bullet {
                Collision = {
                    CollisionLayers = this.BulletCollisionLayers,
                    CollisionMasks = this.BulletCollisionMasks
                },
                Velocity = bulletVelocity,
                Center = Position
            };

            return bullet;
        }

        /// <summary>
        ///     Creates the muzzle flash.
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <returns>A new Muzzle flash</returns>
        protected MuzzleFlash CreateMuzzleFlash()
        {
            var flash = new MuzzleFlash {
                Position = Position,
                Sprite = {
                    Rotation = (float) this.Rotation.RadianToDegree()
                }
            };
            return flash;
        }

        /// <summary>
        ///     Emits the shot event.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        protected void EmitShot()
        {
            this.Shot?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Activates the cooldown timer without shooting a bullet.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.CanShoot == false
        /// </summary>
        public void ActivateCooldown()
        {
            this.CooldownTimer.Start();
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked if emitRemovedEvent == true &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        /// <param name="emitRemovedEvent">Whether to emit the Removed event</param>
        public override void CompleteRemoval(bool emitRemovedEvent = true)
        {
            base.CompleteRemoval(emitRemovedEvent);

            if (this.Shot != null)
            {
                foreach (var subscriber in this.Shot.GetInvocationList())
                {
                    this.Shot -= subscriber as EventHandler;
                }
            }
        }

        private void onBulletRemoved(object sender, EventArgs e)
        {
            if (!(sender is Bullet bullet))
            {
                return;
            }

            this.ActiveBullets--;
            bullet.Removed -= this.onBulletRemoved;
        }

        #endregion
    }
}