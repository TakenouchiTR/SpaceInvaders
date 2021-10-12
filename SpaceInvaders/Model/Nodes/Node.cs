using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     The base node that can be extended from. Contains all the basic functions required by all nodes
    /// </summary>
    public class Node
    {
        #region Data members

        private readonly HashSet<Node> children;
        private readonly Queue<Node> removalQueue;
        private readonly Queue<Node> additionQueue;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a list of the Node's children.
        ///     Modifying the list will not affect the node's children.
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public List<Node> Children => this.children.ToList();

        /// <summary>
        ///     Gets or sets the node's parent.
        /// </summary>
        /// <value>
        ///     The node's parent.
        /// </value>
        public Node Parent { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        public Node()
        {
            this.removalQueue = new Queue<Node>();
            this.additionQueue = new Queue<Node>();
            this.children = new HashSet<Node>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when a child is added.
        /// </summary>
        public event EventHandler<Node> ChildAdded;

        /// <summary>
        ///     Occurs when the node is removed.
        /// </summary>
        public event EventHandler Removed;

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public virtual void Update(double delta)
        {
            foreach (var child in this.children)
            {
                child.Update(delta);
            }

            this.addObjectsInQueue();
            this.removeObjectsInQueue();
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        public virtual void CompleteRemoval()
        {
            this.Removed?.Invoke(this, EventArgs.Empty);

            foreach (var child in this.children)
            {
                child.CompleteRemoval();
            }

            this.Parent = null;
            this.children.Clear();

            if (this.Removed != null)
            {
                foreach (var subscriber in this.Removed.GetInvocationList())
                {
                    this.Removed -= subscriber as EventHandler;
                }
            }
        }

        /// <summary>
        ///     Gets the root for the node. <br />
        ///     The root is the top-most node for a tree, or the node without a parent.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <returns>The root for the node's tree</returns>
        public Node GetRoot()
        {
            if (this.Parent == null)
            {
                return this;
            }

            return this.Parent.GetRoot();
        }

        /// <summary>
        ///     Queues the node and its children for removal at the end of the current update tick.<br />
        ///     Precondition: None
        ///     Postcondition: The node and its children are queued for removal during the end of their parent's update tick.
        /// </summary>
        public void QueueForRemoval()
        {
            this.Parent?.queueNodeForRemoval(this);
            foreach (var child in this.children)
            {
                child.QueueForRemoval();
            }
        }

        private void queueNodeForRemoval(Node obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("obj must not be null");
            }

            this.removalQueue.Enqueue(obj);
        }

        private void removeObjectsInQueue()
        {
            while (this.removalQueue.Count > 0)
            {
                var Node = this.removalQueue.Dequeue();

                if (this.children.Contains(Node))
                {
                    this.children.Remove(Node);
                    Node.CompleteRemoval();
                }
            }

            this.removalQueue.Clear();
        }

        /// <summary>
        ///     Queues the specified game object for adding at the end of the update tick.<br />
        ///     Addition is deferred to prevent errors with updating the set of game objects while iterating over it.<br />
        ///     Precondition: obj != null<br />
        ///     Postcondition: obj is added to children at the end of the update tick
        /// </summary>
        /// <param name="newNode">The object to add.</param>
        /// <exception cref="System.ArgumentException">obj must not be null</exception>
        public virtual void QueueNodeForAddition(Node newNode)
        {
            if (newNode == null)
            {
                throw new ArgumentException("obj must not be null");
            }

            this.additionQueue.Enqueue(newNode);
        }

        private void addObjectsInQueue()
        {
            while (this.additionQueue.Count > 0)
            {
                var Node = this.additionQueue.Dequeue();
                this.AttachChild(Node);
            }
        }

        /// <summary>
        ///     Attaches a new child to the node.<br />
        ///     Precondition: child != null<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="child">The child.</param>
        /// <exception cref="System.ArgumentException">child must not be null</exception>
        public virtual void AttachChild(Node child)
        {
            if (child == null)
            {
                throw new ArgumentException("child must not be null");
            }

            this.children.Add(child);
            child.Parent = this;

            this.ChildAdded?.Invoke(this, child);
        }

        /// <summary>
        ///     Detaches the child.<br />
        ///     Precondition: child != null<br />
        ///     Postcondition: !this.Children.Contains(child)
        /// </summary>
        /// <param name="child">The child to detach.</param>
        /// <exception cref="System.ArgumentException">child must not be null</exception>
        public virtual void DetachChild(Node child)
        {
            if (child == null)
            {
                throw new ArgumentException("child must not be null");
            }

            if (this.children.Contains(child))
            {
                this.children.Remove(child);
                child.Parent = null;
            }
        }

        /// <summary>
        ///     Detaches self from parent.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Parent == null &amp;&amp;<br />
        ///     !this.Parent@prev.Children.Contains(this)
        /// </summary>
        public void DetachFromParent()
        {
            if (this.Parent != null)
            {
                this.Parent.DetachChild(this);
                this.Parent = null;
            }
        }

        /// <summary>
        ///     Gets all collision areas below the node, including the node itself.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <returns>A list of all collision areas below the node</returns>
        public List<CollisionArea> GetCollisionAreas()
        {
            var areas = new List<CollisionArea>();

            if (this is CollisionArea)
            {
                areas.Add(this as CollisionArea);
            }

            foreach (var child in this.children)
            {
                areas.AddRange(child.GetCollisionAreas());
            }

            return areas;
        }

        #endregion
    }
}