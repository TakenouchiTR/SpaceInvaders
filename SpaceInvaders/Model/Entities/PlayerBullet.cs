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
        /// <param name="parent">The parent.</param>
        public PlayerBullet(GameManager parent) : base(parent)
        {
            Speed.Y = -1000;
            CollisionMasks = (int) PhysicsLayer.Enemy;
            CollisionLayers = (int) PhysicsLayer.PlayerHitbox;
        }

        #endregion
    }
}