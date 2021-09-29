using Windows.System;
using SpaceInvaders.View.Sprites;
using System;

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

        private readonly Vector2 velocity;
        private int moveSpeed = 100;
        private bool canShoot;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip(GameManager parent) : base(parent)
        {
            Sprite = new PlayerShipSprite();
            this.canShoot = true;
            this.velocity = new Vector2();
            this.CollisionLayers = (int) PhysicsLayer.Player;
            this.CollisionMasks = (int) PhysicsLayer.EnemyHitbox;
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
                this.velocity.X = moveDistance;

                this.Move(velocity);
            }
        }

        private void handleShooting()
        {
            if (this.canShoot && Input.IsKeyPressed(ShootKey))
            {
                var bullet = new PlayerBullet(parent) 
                {
                    X = this.X,
                    Y = this.Y
                };
                bullet.Removed += onBulletRemoval;
                this.canShoot = false;
            }
        }

        private void onBulletRemoval(GameObject sender, EventArgs e)
        {
            this.canShoot = true;
            sender.Removed -= this.onBulletRemoval;
        }

        #endregion
    }
}
