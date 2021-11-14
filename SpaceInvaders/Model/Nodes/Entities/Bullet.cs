using SpaceInvaders.View.Sprites.Entities;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     The general class for any projectile fired
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public abstract class Bullet : Entity
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the velocity.
        /// </summary>
        /// <value>
        ///     The velocity.
        /// </value>
        public Vector2 Velocity { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Bullet" /> class.<br />
        ///     Precondition: None
        ///     Postcondition: this.Collision.Monitoring == true &amp;&amp;<br />
        ///     this.Collision.Monitorable == true &amp;&amp;<br />
        ///     this.Sprite is PlayerBulletSprite
        /// </summary>
        public Bullet() : base(new PlayerBulletSprite())
        {
            this.Velocity = new Vector2();
            Collision.Monitoring = true;
            Collision.Monitorable = true;
            Collision.Collided += this.onCollided;
            Collision.CollisionMasks = PhysicsLayer.World;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            Move(this.Velocity * delta);
            if (Sprite.IsOffScreen())
            {
                QueueForRemoval();
            }

            base.Update(delta);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            QueueForRemoval();
        }

        #endregion
    }
}