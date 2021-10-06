using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class BasicEnemy : Enemy
    {
        public BasicEnemy() : base(new BasicEnemySprite())
        {
            this.Collision.Collided += this.onCollided;
            this.Score = 10;
        }

        private void onCollided(object sender, CollisionArea e)
        {
            this.QueueForRemoval();
        }
    }
}
