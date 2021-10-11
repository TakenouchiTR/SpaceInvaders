using System;
using Windows.System;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     The player character
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class PlayerShip : Entity
    {
        #region Data members

        private const VirtualKey LeftKey = VirtualKey.Left;
        private const VirtualKey RightKey = VirtualKey.Right;
        private const VirtualKey ShootKey = VirtualKey.Space;

        private readonly Vector2 bulletSpawnLocation = new Vector2(12, -4);

        private readonly int moveSpeed = 200;
        private bool canShoot;
        private Vector2 velocity;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip() : base(new PlayerShipSprite())
        {
            this.canShoot = true;
            this.velocity = new Vector2();
            Collision.Monitorable = true;
            Collision.Monitoring = true;

            Collision.CollisionLayers = PhysicsLayer.Player;
            Collision.CollisionMasks = PhysicsLayer.EnemyHitbox | PhysicsLayer.Enemy;

            Collision.Collided += this.onCollision;
        }

        #endregion

        #region Methods

        private void onCollision(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            this.handleMovement(delta);
            this.handleShooting();

            base.Update(delta);
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
                else if (Collision.Right + moveDistance > MainPage.ApplicationWidth)
                {
                    moveDistance = MainPage.ApplicationWidth - Collision.Right;
                }

                this.velocity.X = moveDistance;

                Move(this.velocity);
            }
        }

        private void handleShooting()
        {
            if (this.canShoot && Input.IsKeyPressed(ShootKey))
            {
                var bullet = new PlayerBullet {
                    Position = Position + this.bulletSpawnLocation
                };
                bullet.Removed += this.onBulletRemoval;

                Parent.QueueNodeForAddition(bullet);
                this.canShoot = false;
            }
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        public override void CompleteRemoval()
        {
            var explosion = new Explosion {
                Center = Center
            };
            GetRoot().QueueNodeForAddition(explosion);
            base.CompleteRemoval();
        }

        private void onBulletRemoval(object sender, EventArgs e)
        {
            if (sender is Bullet bullet)
            {
                this.canShoot = true;
                bullet.Removed -= this.onBulletRemoval;
            }
        }

        #endregion
    }
}