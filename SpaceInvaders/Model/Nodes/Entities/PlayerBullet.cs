namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    /// A Bullet with the default settings for Bullets fired by the player
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Bullet" />
    public class PlayerBullet : Bullet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBullet"/> class.
        /// </summary>
        public PlayerBullet()
        {
            this.Velocity = new Vector2(0, -1000);
            this.Collision.CollisionLayers = PhysicsLayer.PlayerHitbox;
            this.Collision.CollisionMasks = PhysicsLayer.Enemy;
        }
    }
}
