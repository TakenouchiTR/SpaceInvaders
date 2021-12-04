using System;
using System.Collections.Generic;
using SpaceInvaders.Extensions;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     An enemy that fires frequently in irregular intervals.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class MasterEnemy : Enemy
    {
        #region Data members

        private const double MinShotDelay = 2;
        private const double MaxShotDelay = 8;
        private const double MinChargeDelay = 8;
        private const double MaxChargeDelay = 18;
        private const double ChargingShotCutoff = 200;
        private const double ChargingShotCooldownMultiplier = .1;
        private const double ChargeMovementSpeed = 300;
        private const double ReturnStartingYLocation = -300;
        private static readonly Random MasterShipRandom = new Random();

        private Vector2 chargeVelocity;
        private Gun gun;
        private Timer chargeTimer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the state.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        public MasterEnemyState State { get; private set; }

        /// <summary>
        ///     Gets the formation location.
        /// </summary>
        /// <value>
        ///     T   he formation location.
        /// </value>
        public Vector2 FormationLocation { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MasterEnemy" /> class.
        /// </summary>
        public MasterEnemy() : base(createSprite())
        {
            Score = 40;
            Collision.Collided += this.onCollided;
            this.State = MasterEnemyState.InFormation;

            this.setupGun();
            this.setupTimer();
        }

        #endregion

        #region Methods

        private void setupGun()
        {
            var initialCooldown = MasterShipRandom.NextDouble(MinShotDelay, MaxShotDelay);

            this.gun = new EnemyGun {
                Position = Center,
                CooldownDuration = initialCooldown
            };

            this.gun.Shot += this.onGunShot;

            AttachChild(this.gun);
        }

        private void setupTimer()
        {
            this.chargeTimer = new Timer(MasterShipRandom.NextDouble(MinChargeDelay, MaxChargeDelay)) {
                Repeat = false
            };

            this.chargeTimer.Tick += this.onChargeTimerTick;
            this.chargeTimer.Start();

            AttachChild(this.chargeTimer);
        }

        private static AnimatedSprite createSprite()
        {
            var sprites = new List<BaseSprite> {
                new MasterEnemySprite1(),
                new MasterEnemySprite2()
            };

            return new AnimatedSprite(1, sprites);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            switch (this.State)
            {
                case MasterEnemyState.InFormation:
                    this.updateInFormation();
                    break;
                case MasterEnemyState.Charging:
                    this.updateCharging(delta);
                    break;
                case MasterEnemyState.Returning:
                    this.updateReturning(delta);
                    break;
            }

            base.Update(delta);
        }

        private void updateInFormation()
        {
            if (this.gun.CanShoot)
            {
                this.aimAndShoot();
            }
        }

        private void updateCharging(double delta)
        {
            Move(this.chargeVelocity * delta);

            if (this.Y < MainPage.ApplicationHeight - ChargingShotCutoff &&  this.gun.CanShoot)
            {
                this.aimAndShoot();
            }

            if (IsOffScreen())
            {
                this.State = MasterEnemyState.Returning;
                Y = ReturnStartingYLocation;

                this.chargeVelocity = Center.NormalizedVectorTo(this.FormationLocation) * ChargeMovementSpeed;
            }
        }

        private void updateReturning(double delta)
        {
            var moveDistance = this.chargeVelocity * delta;

            if (Center.DistanceToSquared(this.FormationLocation) < moveDistance.MagnitudeSquared)
            {
                Center = this.FormationLocation;
                this.State = MasterEnemyState.InFormation;
                this.gun.ActivateCooldown();
                this.chargeTimer.Start();
            }
            else
            {
                Move(moveDistance);
            }
        }

        private void aimAndShoot()
        {
            var player = (PlayerShip)GetRoot().GetChildByName("PlayerShip");
            
            if (player == null)
            {
                return;
            }
            
            this.gun.Rotation = Center.AngleToTarget(player.Center);
            this.gun.Shoot();
        }

        /// <summary>
        ///     Moves the with the enemy group.<br />
        ///     Precondition: None
        ///     Postcondition: if in formation, this.position == this.position@prev + distance;<br />
        ///     otherwise None
        /// </summary>
        /// <param name="distance">The distance.</param>
        public override void MoveWithGroup(Vector2 distance)
        {
            this.FormationLocation += distance;

            if (this.State == MasterEnemyState.InFormation)
            {
                Move(distance);
            }
            else if (this.State == MasterEnemyState.Returning)
            {
                this.chargeVelocity = Center.NormalizedVectorTo(this.FormationLocation) * ChargeMovementSpeed;
            }
        }

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        private void onGunShot(object sender, EventArgs e)
        {
            this.gun.CooldownDuration = MasterShipRandom.NextDouble(MinShotDelay, MaxShotDelay);
            if (this.State == MasterEnemyState.Charging)
            {
                this.gun.CooldownDuration *= ChargingShotCooldownMultiplier;
            }
        }

        private void onChargeTimerTick(object sender, EventArgs e)
        {
            var player = (PlayerShip) GetRoot().GetChildByName("PlayerShip");
            if (player == null)
            {
                return;
            }

            this.State = MasterEnemyState.Charging;
            this.FormationLocation = Center;
            this.gun.CooldownDuration *= ChargingShotCooldownMultiplier;

            this.chargeVelocity = Center.NormalizedVectorTo(player.Center) * ChargeMovementSpeed;
            this.chargeTimer.Duration = MasterShipRandom.NextDouble(MinChargeDelay, MaxChargeDelay);
        }

        #endregion
    }
}