using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class PlayerBullet : GameObject
    {
        private const int travelSpeed = -1000;

        private Vector2 velocity;

        public PlayerBullet(GameManager parent) : base(parent, new PlayerBulletSprite())
        {
            this.velocity = new Vector2();
            this.Monitoring = true;
            this.CollisionMasks = (int) PhysicsLayer.Enemy;
            this.CollisionLayers = (int) PhysicsLayer.PlayerHitbox;
        }

        public override void Update(double delta)
        {
            this.velocity.Y = travelSpeed * delta;
            this.Move(this.velocity);
            if (this.Bottom < 0)
            {
                this.QueueRemoval();
            }
        }

    }
}
