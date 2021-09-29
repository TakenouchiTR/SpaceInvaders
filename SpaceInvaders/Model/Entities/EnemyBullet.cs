using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class EnemyBullet : GameObject
    {
        private const int travelSpeed = 600;

        private Vector2 velocity;

        public EnemyBullet(GameManager parent) : base(parent, new PlayerBulletSprite())
        {
            this.velocity = new Vector2();
            this.Monitoring = true;
            this.CollisionMasks = (int)PhysicsLayer.Player;
            this.CollisionLayers = (int)PhysicsLayer.EnemyHitbox;
        }

        public override void Update(double delta)
        {
            this.velocity.Y = travelSpeed * delta;
            Move(this.velocity);
            if (this.Y > parent.ScreenHeight)
            {
                this.QueueRemoval();
            }
        }

        public override void HandleCollision(GameObject target)
        {
            target.QueueRemoval();
            this.QueueRemoval();
        }
    }
}
