namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A Bullet with the default settings for Bullets fired by an enemy
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Bullet" />
    public class EnemyBullet : Bullet
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyBullet" /> class.
        /// </summary>
        public EnemyBullet()
        {
            Velocity = new Vector2(0, 500);
            Collision.CollisionLayers = PhysicsLayer.EnemyHitbox;
            Collision.CollisionMasks = PhysicsLayer.Player;
        }

        #endregion
    }
}