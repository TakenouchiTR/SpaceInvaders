namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     A bullet with the default values set if fired by an enemy
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.Bullet" />
    public class EnemyBullet : Bullet
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyBullet" /> class.
        /// </summary>
        /// <param name="manager">The Manager.</param>
        public EnemyBullet(GameManager manager) : base(manager)
        {
            Speed.Y = 500;
            CollisionMasks = (int) PhysicsLayer.Player;
            CollisionLayers = (int) PhysicsLayer.EnemyHitbox;
        }

        #endregion
    }
}