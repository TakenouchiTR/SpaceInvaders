using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     An enemy that randomly flies across the screen, giving the player a power-up when killed
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class BonusEnemy : Enemy
    {
        private const double MoveSpeed = 100;

        private readonly int moveFactor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BonusEnemy"/> class.
        /// </summary>
        /// <param name="sprite">The enemy sprite.</param>
        public BonusEnemy() : base(createSprite())
        {
            this.Score = 100;
            this.moveFactor = 1;
            Collision.Collided += this.onCollided;

            this.placeShip();
        }

        private void placeShip()
        {
            this.Position = new Vector2(-Sprite.Width, 0);
        }

        private static AnimatedSprite createSprite()
        {
            var sprites = new List<BaseSprite> {
                new BonusEnemySprite()
            };

            return new AnimatedSprite(1, sprites);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            Move(Vector2.Right * this.moveFactor * MoveSpeed * delta);
            if (this.IsOffScreen())
            {
                this.Score = 0;
                this.QueueForRemoval();
            }
        }
        
        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

    }
}
