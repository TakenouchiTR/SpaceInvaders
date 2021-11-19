namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     Creates a gun with the default settings for enemies
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Gun" />
    public class EnemyGun : Gun
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Gun" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Rotation == Vector2.Down.ToAngle() &amp;&amp;<br />
        ///     this.BulletSpeed == 350 &amp;&amp;<br />
        ///     this.MaxBulletsOnScreen == 3 &amp;&amp;<br />
        ///     this.BulletCollisionLayers == PhysicsLayer.EnemyHitbox &amp;&amp;<br />
        ///     this.BulletCollisionMasks == PhysicsLayer.Player | PhysicsLayer.World &amp;&amp;<br />
        ///     this.CooldownDuration == 1
        /// </summary>
        public EnemyGun()
            : base(PhysicsLayer.EnemyHitbox, PhysicsLayer.Player | PhysicsLayer.World, 1)
        {
            Rotation = Vector2.Down.ToAngle();
            BulletSpeed = 350;
            ActivateCooldown();
        }

        #endregion
    }
}