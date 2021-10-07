using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    /// The weak points for the test boss
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class TestBossTarget : Entity
    {
        #region Data members

        private int health;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBossTarget"/> class.
        /// </summary>
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