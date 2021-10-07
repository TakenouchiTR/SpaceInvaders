using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class TestBossTarget : Entity
    {
        #region Data members

        private int health;

        #endregion

        #region Constructors

        public TestBossTarget() : base(new BasicEnemySprite())
        {
            this.health = 3;

            Collision.CollisionLayers = PhysicsLayer.Enemy;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            Collision.Collided += this.onCollided;
            Collision.Monitoring = true;
            Collision.Monitorable = true;
        }

        #endregion

        #region Methods

        private void onCollided(object sender, CollisionArea e)
        {
            this.health--;

            if (this.health <= 0)
            {
                QueueForRemoval();
            }
        }

        #endregion
    }
}