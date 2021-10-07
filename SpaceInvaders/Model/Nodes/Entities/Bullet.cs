using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    public class Bullet : Entity
    {
        #region Properties

        public Vector2 Velocity { get; set; }

        #endregion

        #region Constructors

        public Bullet() : base(new PlayerBulletSprite())
        {
            this.Velocity = new Vector2();
            Collision.Monitoring = true;
            Collision.Monitorable = true;
            Collision.Collided += this.onCollided;
        }

        #endregion

        #region Methods

        public override void Update(double delta)
        {
            Move(this.Velocity * delta);
            if (Sprite.IsOffScreen())
            {
                QueueForRemoval();
            }

            base.Update(delta);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        #endregion
    }
}