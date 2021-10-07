using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class IntermediateEnemy : Enemy
    {
        #region Constructors

        public IntermediateEnemy() : base(new IntermediateEnemySprite())
        {
            Collision.Collided += this.onCollided;
            Score = 20;
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