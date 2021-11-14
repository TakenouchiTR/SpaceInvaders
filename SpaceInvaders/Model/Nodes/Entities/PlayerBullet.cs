namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A Bullet with the default settings for Bullets fired by the player
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Bullet" />
    public class PlayerBullet : Bullet
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerBullet" /> class.<br />
        ///     Precondition: None
        ///     Postcondition: this.Velocity.X == -700 &amp;&amp;<br />
        ///     this.Collision.CollisionLayers == PhysicsLayer.PlayerHitbox &amp;&amp;<br />
        ///     this.Collision.CollisionMasks == PhysicsLayer.Enemy<br />
        /// </summary>
        public PlayerBullet()
        {
            Velocity = new Vector2(0, -700);
            Collision.CollisionLayers = PhysicsLayer.PlayerHitbox;
            Collision.CollisionMasks |= PhysicsLayer.Enemy;
        }

        #endregion
    }
}