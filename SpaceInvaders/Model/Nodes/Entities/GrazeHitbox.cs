using System;
using System.Collections.Generic;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A hitbox used for checking if the player grazes bullets
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.CollisionArea" />
    public class GrazeHitbox : CollisionArea
    {
        #region Data members

        private readonly HashSet<CollisionArea> grazedBullets;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrazeHitbox" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        public GrazeHitbox()
        {
            CollisionMasks = PhysicsLayer.EnemyHitbox;
            Monitoring = true;

            this.grazedBullets = new HashSet<CollisionArea>();
            Collided += this.onCollided;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [bullet grazed].
        /// </summary>
        public event EventHandler BulletGrazed;

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked if emitRemovedEvent == true &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        /// <param name="emitRemovedEvent">Whether to emit the Removed event</param>
        public override void CompleteRemoval(bool emitRemovedEvent = true)
        {
            base.CompleteRemoval(emitRemovedEvent);
            if (this.BulletGrazed != null)
            {
                foreach (var subscriber in this.BulletGrazed.GetInvocationList())
                {
                    this.BulletGrazed -= subscriber as EventHandler;
                }
            }
        }

        private void onCollided(object sender, CollisionArea e)
        {
            if (this.grazedBullets.Contains(e))
            {
                return;
            }

            this.grazedBullets.Add(e);
            e.Removed += this.onBulletRemoved;
            this.BulletGrazed?.Invoke(this, EventArgs.Empty);
        }

        private void onBulletRemoved(object sender, EventArgs e)
        {
            if (sender is CollisionArea bullet)
            {
                if (this.grazedBullets.Contains(bullet))
                {
                    this.grazedBullets.Remove(bullet);
                }
            }
        }

        #endregion
    }
}