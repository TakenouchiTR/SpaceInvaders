using System;
using System.Collections.Generic;
using SpaceInvaders.Extensions;
using SpaceInvaders.Model.Nodes.PowerUps;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     An enemy that randomly flies across the screen, giving the player a power-up when killed
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class BonusEnemy : Enemy
    {
        #region Data members

        private const double MoveSpeed = 100;
        private const double SoundRange = 700;
        private const int MinShotDelay = 1;
        private const int MaxShotDelay = 3;

        private static readonly Random BonusEnemyRandom = new Random();

        private int moveFactor;
        private SoundPlayer hummingPlayer;
        private Gun gun;
        private BonusEnemyState state;

        #endregion

        #region Properties

        private double DistanceFromCenterScreen => Math.Abs(Center.X - MainPage.ApplicationWidth / 2);

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BonusEnemy" /> class.
        /// </summary>
        public BonusEnemy() : base(createSprite())
        {
            Score = 100;
            this.state = BonusEnemyState.EnteringPlay;
            Collision.Collided += this.onCollided;

            this.placeShip();
            this.setupGun();
            this.setupHumming();
        }

        #endregion

        #region Methods

        private void placeShip()
        {
            if (BonusEnemyRandom.Next(2) == 0)
            {
                Position = new Vector2(-SoundRange, 0);
                this.moveFactor = 1;
            }
            else
            {
                Position = new Vector2(MainPage.ApplicationWidth + SoundRange, 0);
                this.moveFactor = -1;
            }
        }

        private void setupGun()
        {
            this.gun = new EnemyGun {
                CooldownDuration = BonusEnemyRandom.NextDouble(MinShotDelay, MaxShotDelay),
                Position = Center
            };
            this.gun.Shot += this.onGunShot;
            AttachChild(this.gun);
        }

        private void setupHumming()
        {
            this.hummingPlayer = new SoundPlayer("modulating.wav") {
                Looping = true,
                Volume = 0
            };
            this.hummingPlayer.Play();

            AttachChild(this.hummingPlayer);
        }

        private static AnimatedSprite createSprite()
        {
            var sprites = new List<BaseSprite> {
                new BonusEnemySprite()
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
            Move(Vector2.Right * this.moveFactor * MoveSpeed * delta);
            this.adjustHummingVolume();

            switch (this.state)
            {
                case BonusEnemyState.EnteringPlay:
                    this.updateEnteringPlay();
                    break;
                case BonusEnemyState.OnScreen:
                    this.updateOnScreen();
                    break;
                case BonusEnemyState.ExitingPlay:
                    this.updateExitingPlay();
                    break;
            }

            base.Update(delta);
        }

        private void updateEnteringPlay()
        {
            if (IsOffScreen())
            {
                return;
            }

            this.state = BonusEnemyState.OnScreen;
            this.gun.ActivateCooldown();
        }

        private void updateOnScreen()
        {
            if (!this.gun.CanShoot)
            {
                return;
            }

            if (IsOffScreen())
            {
                this.state = BonusEnemyState.ExitingPlay;
                ExplodeOnDeath = false;
                Score = 0;
            }

            var player = (PlayerShip) GetRoot().GetChildByName("PlayerShip");
            if (player == null)
            {
                return;
            }

            this.gun.Rotation = Center.AngleToTarget(player.Center);
            this.gun.Shoot();
        }

        private void updateExitingPlay()
        {
            if (this.DistanceFromCenterScreen > SoundRange)
            {
                QueueForRemoval();
            }
        }

        private void adjustHummingVolume()
        {
            this.hummingPlayer.Volume = Math.Max(0, (SoundRange - this.DistanceFromCenterScreen) / SoundRange);
            ;
        }

        private void onCollided(object sender, CollisionArea e)
        {
            var player = (PlayerShip) GetRoot().GetChildByName("PlayerShip");
            if (player != null)
            {
                var powerUp = new ReflectionShieldPowerUp();
                powerUp.AttachToPlayer(player);
            }

            QueueForRemoval();
        }

        private void onGunShot(object sender, EventArgs e)
        {
            this.gun.CooldownDuration = BonusEnemyRandom.NextDouble(MinShotDelay, MaxShotDelay);
        }

        #endregion
    }
}