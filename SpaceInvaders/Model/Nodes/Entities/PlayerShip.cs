using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly Vector2 bulletSpawnLocation = new Vector2(12, -8);

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
        }

        public override void Update(double delta)
        {
            this.handleMovement(delta);
            //this.handleShooting();
            if (Input.IsKeyPressed(ShootKey))
            {
                this.QueueForRemoval();
            }
            base.Update(delta);
        }

        public override void CompleteRemoval()
        {

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

        //private void handleShooting()
        //{
        //    if (this.canShoot && Input.IsKeyPressed(ShootKey))
        //    {
        //        var bullet = new PlayerBullet(Manager)
        //        {
        //            Position = Position + this.bulletSpawnLocation
        //        };
        //        bullet.Removed += this.onBulletRemoval;

        //        Manager.QueueGameObjectForAddition(bullet);
        //        this.canShoot = false;
        //    }
        //}

        //public override void HandleCollision(GameObject target)
        //{
        //    base.HandleCollision(target);
        //    QueueRemoval();
        //}

        //private void onBulletRemoval(object sender, EventArgs e)
        //{
        //    if (sender is GameObject bullet)
        //    {
        //        this.canShoot = true;
        //        bullet.Removed -= this.onBulletRemoval;
        //    }
        //}

    }
}
