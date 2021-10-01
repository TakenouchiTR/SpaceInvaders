using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    /// <summary>
    ///     An intermediate enemy which has the same behavior as a basic enemy, but is worth more points.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.Enemies.Enemy" />
    public class IntermediateEnemy : Enemy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntermediateEnemy"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public IntermediateEnemy(GameManager parent) : base(parent, new IntermediateEnemySprite())
        {
            this.Score = 20;
        }

        public override void Update(double delta)
        {
        }
    }
}
