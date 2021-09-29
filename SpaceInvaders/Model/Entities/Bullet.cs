using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class Bullet : GameObject
    {
        #region Properties

        public Vector2 Speed { get; set; }

        #endregion

        #region Constructors

        public Bullet(GameManager parent) : base(parent, new PlayerBulletSprite())
        {
            Monitoring = true;
            this.Speed = new Vector2();
        }

        #endregion

        #region Methods

        public override void Update(double delta)
        {
            Move(this.Speed * delta);
            if (IsOffScreen())
            {
                QueueRemoval();
            }
        }

        public override void HandleCollision(GameObject target)
        {
            base.HandleCollision(target);

            target.QueueRemoval();
            QueueRemoval();
        }

        #endregion
    }
}