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
        ///     Initializes a new instance of the <see cref="EnemyBullet" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Collision.CollisionLayers == PhysicsLayer.EnemyHitbox &amp;&amp;<br />
        ///     this.Collision.CollisionLayers == PhysicsLayer.Player &amp;&amp;<br />
        ///     this.Velocity.Y == 500
        /// </summary>
        public EnemyBullet()
        {
            Velocity = new Vector2(0, 350);
            Collision.CollisionLayers = PhysicsLayer.EnemyHitbox;
            Collision.CollisionMasks = PhysicsLayer.Player;
        }

        #endregion
    }
}