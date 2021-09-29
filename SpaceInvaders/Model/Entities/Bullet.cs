using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class Bullet : GameObject
    {
        public Vector2 Speed { get; set; }

        public Bullet(GameManager parent) : base(parent, new PlayerBulletSprite())
        {
            this.Monitoring = true;
            this.Speed = new Vector2();
        }

        public override void Update(double delta)
        {
            this.Move(this.Speed * delta);
            if (this.IsOffScreen())
            {
                QueueRemoval();
            }
        }

        public override void HandleCollision(GameObject target)
        {
            base.HandleCollision(target);

            target.QueueRemoval();
            this.QueueRemoval();
        }
    }
}
