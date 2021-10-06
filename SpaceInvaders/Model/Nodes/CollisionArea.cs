using System;

namespace SpaceInvaders.Model.Nodes
{
    public class CollisionArea : Area
    {
        /// <summary>
        ///     Gets or sets the collision layers.
        ///     Each bit of CollisionLayers represents a different layer.
        ///     If Monitorable is set to true, other GameObjects will check if any of their flagged
        ///     CollisionMask bits match this object's CollisionLayer when their bounding boxes overlap.
        /// </summary>
        /// <value>
        ///     The collision layers.
        /// </value>
        public PhysicsLayer CollisionLayers { get; set; }

        /// <summary>
        ///     Gets or sets the collision masks.
        ///     Each bit of CollisionMasks represents a different layer.
        ///     If Monitoring is set to true, this GameObject will check if any of its flagged
        ///     CollisionMask bits match another object's CollisionLayer when their bounding boxes overlap.
        /// </summary>
        /// <value>
        ///     The collision masks.
        /// </value>
        public PhysicsLayer CollisionMasks { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GameObject" /> is monitorable.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitorable; otherwise, <c>false</c>.
        /// </value>
        public bool Monitorable { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GameObject" /> is monitoring for collisions.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitoring; otherwise, <c>false</c>.
        /// </value>
        public bool Monitoring { get; set; }

        public EventHandler<CollisionArea> Collided;

        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            if (this.Collided != null)
            {
                foreach (var subscriber in this.Collided?.GetInvocationList())
                {
                    this.Removed -= subscriber as EventHandler;
                }
            }
        }

        public void DetectCollision(CollisionArea target)
        {
            if (!this.isOverlappingWith(target))
            {
                return;
            }

            if (this.isMaskingTarget(target))
            {
                this.Collided?.Invoke(this, target);
            }

            if (target.isMaskingTarget(this))
            {
                target.Collided?.Invoke(target, this);
            }
        }

        private bool isOverlappingWith(CollisionArea target)
        {
            return this.Left <= target.Right &&
                   this.Right >= target.Left &&
                   this.Top <= target.Bottom &&
                   this.Bottom >= target.Top;

        }

        private bool isMaskingTarget(CollisionArea target)
        {
            return this.Monitoring && target.Monitorable && this.CollisionMasks.HasFlag(target.CollisionLayers);
        }
    }
}
