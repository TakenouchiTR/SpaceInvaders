using System;
using Windows.System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     Manages the player ship.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.GameObject" />
    public class PlayerShip : GameObject
    {
        #region Data members

        private const VirtualKey LeftKey = VirtualKey.Left;
        private const VirtualKey RightKey = VirtualKey.Right;
        private const VirtualKey ShootKey = VirtualKey.Space;

        private readonly Vector2 bulletSpawnLocation = new Vector2(12, -8);

        private readonly Vector2 velocity;
        private readonly int moveSpeed = 200;
        private bool canShoot;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip(GameManager parent) : base(parent, new PlayerShipSprite())
        {
            this.canShoot = true;
            this.velocity = new Vector2();
            Monitorable = true;
            Monitoring = true;

            CollisionLayers = (int) PhysicsLayer.Player;
            CollisionMasks = (int) PhysicsLayer.EnemyHitbox;
        }

        #endregion

        #region Methods

        public override void Update(double delta)
        {
            this.handleMovement(delta);
            this.handleShooting();
        }

        private void handleMovement(double delta)
        {
            double moveDistance = 0;

            if (Input.IsKeyPressed(LeftKey))
            {
                moveDistance -= 1;
            }

            if (Input.IsKeyPressed(RightKey))
            {
                moveDistance += 1;
            }

            if (moveDistance != 0)
            {
                moveDistance *= this.moveSpeed * delta;

                if (X + moveDistance < 0)
                {
                    moveDistance = -X;
                }
                else if (Right + moveDistance > parent.ScreenWidth)
                {
                    moveDistance = parent.ScreenWidth - Right;
                }

                this.velocity.X = moveDistance;

                Move(this.velocity);
            }
        }

        private void handleShooting()
        {
            if (this.canShoot && Input.IsKeyPressed(ShootKey))
            {
                var bullet = new PlayerBullet(parent) {
                    Position = Position + this.bulletSpawnLocation
                };
                bullet.Removed += this.onBulletRemoval;

                parent.QueueGameObjectForAddition(bullet);
                this.canShoot = false;
            }
        }

        private void onBulletRemoval(Object sender, EventArgs e)
        {
            if (sender is GameObject bullet)
            {
                this.canShoot = true;
                bullet.Removed -= this.onBulletRemoval;
            }
        }

        #endregion
    }
}