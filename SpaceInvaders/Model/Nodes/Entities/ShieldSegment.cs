using SpaceInvaders.View.Sprites.Entities;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     Represents one segment of the shield. Blocks bullets from both the player and the enemies.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class ShieldSegment : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShieldSegment"/> class.
        /// </summary>
        public ShieldSegment() : base(new ShieldSegmentSprite())
        {
            Collision.CollisionLayers = PhysicsLayer.World;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox | PhysicsLayer.EnemyHitbox;
            Collision.Monitoring = true;
            Collision.Monitorable = true;

            Collision.Collided += this.onCollision;
        }

        private void onCollision(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }
    }
}
