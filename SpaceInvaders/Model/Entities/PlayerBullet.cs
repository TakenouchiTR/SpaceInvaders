namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     A bullet with the default values set if fired by an enemy
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.Bullet" />
    public class PlayerBullet : Bullet
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerBullet" /> class.
        /// </summary>
        /// <param name="manager">The Manager.</param>
        public PlayerBullet(GameManager manager) : base(manager)
        {
            Speed.Y = -1000;
            this.CollisionMasks = PhysicsLayer.Enemy;
            this.CollisionLayers = PhysicsLayer.PlayerHitbox;
        }

        #endregion
    }
}