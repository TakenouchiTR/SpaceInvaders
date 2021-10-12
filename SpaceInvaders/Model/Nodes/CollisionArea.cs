using System;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A specialized area that checks if it is overlapping with other CollisionAreas
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Area" />
    public class CollisionArea : Area
    {
        #region Data members

        /// <summary>
        ///     Occurs whenever the CollisionArea collides with another Collision Area that is is masking
        /// </summary>
        public EventHandler<CollisionArea> Collided;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the collision layers.<br />
        ///     Each bit of CollisionLayers represents a different layer.<br />
        ///     If Monitorable is set to true, other Nodes will check if any of their flagged<br />
        ///     CollisionMask bits match this object's CollisionLayer when their bounding boxes overlap.
        /// </summary>
        /// <value>
        ///     The collision layers.
        /// </value>
        public PhysicsLayer CollisionLayers { get; set; }

        /// <summary>
        ///     Gets or sets the collision masks.<br />
        ///     Each bit of CollisionMasks represents a different layer.<br />
        ///     If Monitoring is set to true, this Node will check if any of its flagged<br />
        ///     CollisionMask bits match another object's CollisionLayer when their bounding boxes overlap.
        /// </summary>
        /// <value>
        ///     The collision masks.
        /// </value>
        public PhysicsLayer CollisionMasks { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="CollisionArea" /> is monitorable.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitorable; otherwise, <c>false</c>.
        /// </value>
        public bool Monitorable { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="CollisionArea" /> is monitoring for collisions.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitoring; otherwise, <c>false</c>.
        /// </value>
        public bool Monitoring { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CollisionArea" /> class.<br />
        ///     The area will have no width, height, and will be placed at (0, 0).
        /// </summary>
        public CollisionArea()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CollisionArea" /> class with a specified width and height at the
        ///     coordinates (0, 0).
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public CollisionArea(double width, double height) : base(width, height)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CollisionArea" /> class with a specified width, height, and
        ///     coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public CollisionArea(double x, double y, double width, double height) : base(x, y, width, height)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            if (this.Collided != null)
            {
                foreach (var subscriber in this.Collided?.GetInvocationList())
                {
                    Removed -= subscriber as EventHandler;
                }
            }
        }

        /// <summary>
        ///     Detects if collision is occurring between this CollisionArea and the target.<br />
        ///     Precondition: target != null<br />
        ///     Postcondition: this.Collided fires if collision occurs and this is masking target &amp;&amp;<br />
        ///     target.Collided fires is collision occurs and target is masking this
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentException">Target must not be null</exception>
        public void DetectCollision(CollisionArea target)
        {
            if (target == null)
            {
                throw new ArgumentException("Target must not be null");
            }

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
            return Left <= target.Right &&
                   Right >= target.Left &&
                   Top <= target.Bottom &&
                   Bottom >= target.Top;
        }

        private bool isMaskingTarget(CollisionArea target)
        {
            return this.Monitoring && target.Monitorable && this.CollisionMasks.HasFlag(target.CollisionLayers);
        }

        #endregion
    }
}