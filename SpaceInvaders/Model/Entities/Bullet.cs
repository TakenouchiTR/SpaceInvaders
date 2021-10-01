using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class Bullet : GameObject
    {
        #region Properties

        public Vector2 Speed { get; set; }

        #endregion

        #region Constructors

        public Bullet(GameManager gameManager) : base(gameManager, new PlayerBulletSprite())
        {
            Monitoring = true;
            Monitorable = true;
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

            QueueRemoval();
        }

        #endregion
    }
}