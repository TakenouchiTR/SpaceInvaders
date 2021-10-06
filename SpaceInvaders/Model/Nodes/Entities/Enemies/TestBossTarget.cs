using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class TestBossTarget : Entity
    {
        private int health;

        public TestBossTarget() : base(new BasicEnemySprite())
        {
            this.health = 3;

            Collision.CollisionLayers = PhysicsLayer.Enemy;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            Collision.Collided += this.onCollided;
            Collision.Monitoring = true;
            Collision.Monitorable = true;
        }

        private void onCollided(object sender, CollisionArea e)
        {
            this.health--;

            if (this.health <= 0)
            {
                this.QueueForRemoval();
            }
        }
    }
}
