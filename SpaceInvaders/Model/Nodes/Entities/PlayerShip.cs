using System;
using Windows.System;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    public class PlayerShip : Entity
    {
        private const VirtualKey LeftKey = VirtualKey.Left;
        private const VirtualKey RightKey = VirtualKey.Right;
        private const VirtualKey ShootKey = VirtualKey.Space;

        private readonly Vector2 bulletSpawnLocation = new Vector2(12, -4);

        private readonly int moveSpeed = 200;
        private bool canShoot;
        private Vector2 velocity;

        public PlayerShip() : base(new PlayerShipSprite())
        {
            this.canShoot = true;
            this.velocity = new Vector2();
            this.Collision.Monitorable = true;
            this.Collision.Monitoring = true;

            this.Collision.CollisionLayers = PhysicsLayer.Player;
            this.Collision.CollisionMasks = PhysicsLayer.EnemyHitbox | PhysicsLayer.Enemy;

            this.Collision.Collided += this.onCollision;
        }

        private void onCollision(object sender, CollisionArea e)
        {
            this.QueueForRemoval();
        }

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
                var bullet = new Bullet()
                {
                    Position = Position + this.bulletSpawnLocation,
                    Velocity = new Vector2(0, -1000)
                };
                bullet.Collision.CollisionMasks = PhysicsLayer.Enemy;
                bullet.Collision.CollisionLayers = PhysicsLayer.PlayerHitbox;
                bullet.Removed += this.onBulletRemoval;

                Parent.QueueGameObjectForAddition(bullet);
                this.canShoot = false;
            }
        }
        private void onBulletRemoval(object sender, EventArgs e)
        {
            if (sender is Bullet bullet)
            {
                this.canShoot = true;
                bullet.Removed -= this.onBulletRemoval;
            }
        }

    }
}
