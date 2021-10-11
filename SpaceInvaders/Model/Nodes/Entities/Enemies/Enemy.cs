using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     The base class to derive enemies from
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public abstract class Enemy : Entity
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the score earned from killing the enemy.
        /// </summary>
        /// <value>
        ///     The score earned.
        /// </value>
        public int Score { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Enemy" /> class.
        /// </summary>
        /// <param name="sprite">The enemy sprite.</param>
        protected Enemy(BaseSprite sprite) : base(sprite)
        {
            Collision.CollisionLayers = PhysicsLayer.Enemy;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            Collision.Monitoring = true;
            Collision.Monitorable = true;
            this.Score = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Runs cleanup and invokes the Removed event when removed from the game.<br />
        /// Precondition: None<br />
        /// Postcondition: Removed event is invoked &amp;&amp;<br />
        /// All event subscribers are removed
        /// </summary>
        public override void CompleteRemoval()
        {
            var explosion = new Explosion {
                Center = Center
            };
            GetRoot().QueueNodeForAddition(explosion);
            base.CompleteRemoval();
        }

        #endregion
    }
}