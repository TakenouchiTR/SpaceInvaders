using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     The most basic enemy. Has no additional functionality by default
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.Enemy" />
    public class BasicEnemy : Enemy
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasicEnemy" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public BasicEnemy(GameManager parent) : base(parent, new BasicEnemySprite())
        {
            Score = 10;
        }

        public override void Update(double delta)
        {

        }
    }
}
