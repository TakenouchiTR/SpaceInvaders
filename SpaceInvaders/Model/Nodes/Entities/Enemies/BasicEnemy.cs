using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     A basic enemy that has no behavior
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class BasicEnemy : Enemy
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasicEnemy" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 2 &amp;&amp;<br />
        ///     this.Score == 10 &amp;&amp;<br />
        ///     this.Sprite is BasicEnemySprite
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