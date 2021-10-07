using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    /// A basic enemy that has no behavior
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class BasicEnemy : Enemy
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicEnemy"/> class.
        /// </summary>
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