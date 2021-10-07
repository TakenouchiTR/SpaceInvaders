using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     An enemy with no behavior.<br />
    ///     Worth more points than BasicEnemy
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class IntermediateEnemy : Enemy
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="IntermediateEnemy" /> class.
        /// </summary>
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