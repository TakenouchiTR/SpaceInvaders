using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class PlayerBullet : GameObject
    {
        private Vector2 velocity;

        public PlayerBullet(GameManager parent) : base(parent)
        {
            this.Sprite = new PlayerShipSprite();
            this.velocity = new Vector2(0, -100);
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
