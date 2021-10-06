﻿using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    public class Bullet : Entity
    {
        public Vector2 Velocity { get; set; }
        public Bullet() : base(new PlayerBulletSprite())
        {
            this.Velocity = new Vector2();
            Collision.Monitoring = true;
            Collision.Monitorable = true;
            this.Collision.Collided += this.onCollided;
        }

        public override void Update(double delta)
        {
            this.Move(Velocity * delta);
            if (this.Sprite.IsOffScreen())
            {
                this.QueueForRemoval();
            }
            base.Update(delta);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            this.QueueForRemoval();
        }
    }
}
