using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class BasicEnemy : Enemy
    {
        #region Constructors

        public BasicEnemy() : base(new BasicEnemySprite())
        {
            Collision.Collided += this.onCollided;
            Score = 10;
        }

        #endregion

        #region Methods

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        #endregion
    }
}