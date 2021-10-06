using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class IntermediateEnemy : Enemy
    {
        public IntermediateEnemy() : base(new IntermediateEnemySprite())
        {
            this.Collision.Collided += this.onCollided;
            this.Score = 20;
        }

        private void onCollided(object sender, CollisionArea e)
        {
            this.QueueForRemoval();
        }
    }
}
