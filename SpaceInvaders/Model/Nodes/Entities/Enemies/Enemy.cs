using System;
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
        ///     Initializes a new instance of the <see cref="Enemy" /> class.<br />
        ///     Precondition: None
        ///     Postcondition: this.Collision.CollisionLayers == PhysicsLayer.Enemy &amp;&amp;<br />
        ///     this.Collision.CollisionMasks == PhysicsLayer.PlayerHitbox &amp;&amp;<br />
        ///     this.Collision.Monitorable == true &amp;&amp;<br />
        ///     this.Collision.Monitoring == true &amp;&amp;<br />
        ///     this.Sprite == sprite
        /// </summary>
        /// <param name="sprite">The enemy sprite.</param>
        protected Enemy(BaseSprite sprite) : base(sprite)
        {
            Collision.CollisionLayers = PhysicsLayer.Enemy;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            Collision.Monitoring = true;
            Collision.Monitorable = true;

            this.Score = 0;
            Removed += this.onRemoved;
        }

        #endregion

        #region Methods

        private void onRemoved(object sender, EventArgs e)
        {
            var explosion = new Explosion {
                Center = Center
            };
            GetRoot().QueueNodeForAddition(explosion);
        }

        #endregion
    }
}