using System.Collections.Generic;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

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
        ///     Initializes a new instance of the <see cref="IntermediateEnemy" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 2 &amp;&amp;<br />
        ///     this.Score == 20 &amp;&amp;<br />
        ///     this.Sprite is IntermediateEnemySprite
        /// </summary>
        public IntermediateEnemy() : base(createSprite())
        {
            Collision.Collided += this.onCollided;
            Score = 20;
        }

        #endregion

        #region Methods

        private static AnimatedSprite createSprite()
        {
            var sprites = new List<BaseSprite>()
            {
                new IntermediateEnemySprite1(),
                new IntermediateEnemySprite2()
            };

            return new AnimatedSprite(1, sprites);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        #endregion
    }
}