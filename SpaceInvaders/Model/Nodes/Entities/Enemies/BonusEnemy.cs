using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model.Nodes.PowerUps;
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
        private const double MoveSpeed = 100;
        private const double GunCooldown = 2.5;

        private readonly int moveFactor;
        private Gun gun;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BonusEnemy"/> class.
        /// </summary>
        /// <param name="sprite">The enemy sprite.</param>
        public BonusEnemy() : base(createSprite())
        {
            this.Score = 100;
            this.moveFactor = 1;
            Collision.Collided += this.onCollided;

            this.placeShip();
            this.setupGun();
        }

        private void placeShip()
        {
            this.Position = new Vector2(-Sprite.Width, 0);
        }

        private void setupGun()
        {
            this.gun = new EnemyGun() {
                CooldownDuration = GunCooldown,
                Position = Center
            };
            this.gun.ActivateCooldown();
            AttachChild(this.gun);
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
            if (this.IsOffScreen())
            {
                this.ExplodeOnDeath = false;
                this.Score = 0;
                this.QueueForRemoval();
            }
            else if (this.gun.CanShoot)
            {
                var player = (PlayerShip)GetRoot().GetChildByName("PlayerShip");
                if (player == null)
                {
                    return;
                }

                this.gun.Rotation = this.Center.AngleToTarget(player.Center);
                this.gun.Shoot();
            }
            base.Update(delta);
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

    }
}
