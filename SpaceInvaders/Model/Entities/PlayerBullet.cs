using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class PlayerBullet : GameObject
    {
        private Vector2 velocity;

        public PlayerBullet(GameManager parent) : base(parent, new PlayerBulletSprite())
        {
            this.velocity = new Vector2(0, -100);
            this.Monitoring = true;
            this.Monitorable = true;
            this.CollisionMasks = (int) PhysicsLayer.Enemy;
            this.CollisionLayers = (int) PhysicsLayer.PlayerHitbox;
        }

        public override void Update(double delta)
        {
            this.velocity.Y = -1000 * delta;
            Move(this.velocity);
            if (this.Bottom < 0)
            {
                this.QueueRemoval();
            }
        }

    }
}
