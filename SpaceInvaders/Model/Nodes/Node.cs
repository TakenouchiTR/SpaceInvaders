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

        private string name;
        private readonly Dictionary<string, Node> children;
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
        public IList<Node> Children => this.children.Values.ToList();

        /// <summary>
        ///     Gets or sets the node's parent.
        /// </summary>
        /// <value>
        ///     The node's parent.
        /// </value>
        public Node Parent { get; protected set; }

        /// <summary>
        ///     Gets or sets the name.<br />
        ///     If the node has a parent and the parent already contains a child with the specified name,<br />
        ///     it will append a number to make it unique to the parent's children.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get => this.name;
            set
            {
                if (this.Parent != null)
                {
                    value = this.Parent.CreateValidName(value);
                }

                this.name = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        public Node()
        {
            this.removalQueue = new Queue<Node>();
            this.additionQueue = new Queue<Node>();
            this.children = new Dictionary<string, Node>();
            this.name = GetType().ToString().Split(".").Last();
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
            foreach (var child in this.children.Values)
            {
                child.Update(delta);
            }

            this.addObjectsInQueue();
            this.removeObjectsInQueue();
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked if emitRemovedEvent == true &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        /// <param name="emitRemovedEvent">Whether to emit the Removed event</param>
        public virtual void CompleteRemoval(bool emitRemovedEvent = true)
        {
            foreach (var child in this.children.Values)
            {
                child.CompleteRemoval(emitRemovedEvent);
            }

            if (emitRemovedEvent && this.Removed != null)
            {
                this.Removed.Invoke(this, EventArgs.Empty);
            }

            if (this.Removed != null)
            {
                foreach (var subscriber in this.Removed.GetInvocationList())
                {
                    this.Removed -= subscriber as EventHandler;
                }
            }

            this.Parent = null;
            this.children.Clear();
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
            foreach (var child in this.children.Values)
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
                var node = this.removalQueue.Dequeue();

                if (this.children.ContainsKey(node.Name))
                {
                    this.children.Remove(node.Name);
                    node.CompleteRemoval();
                }
            }

            this.removalQueue.Clear();
        }

        /// <summary>
        ///     Removes all children without firing the child's Removed event
        /// </summary>
        protected void SilentlyRemoveAllChildren()
        {
            foreach (var child in this.children.Values)
            {
                if (child.Removed != null)
                {
                    foreach (var subscriber in child.Removed.GetInvocationList())
                    {
                        child.Removed -= subscriber as EventHandler;
                    }
                }

                child.CompleteRemoval(false);
                child.SilentlyRemoveAllChildren();
                child.Parent = null;
            }

            this.children.Clear();
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
                var node = this.additionQueue.Dequeue();
                this.AttachChild(node);
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

            child.Name = this.CreateValidName(child.Name);
            this.children[child.name] = child;
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

            if (this.children.ContainsKey(child.Name))
            {
                this.children.Remove(child.Name);
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
        ///     Checks if a specified name is unique to the node's children. <br />
        ///     If it is unique, it will return the name without alterations. If it is not unique, a number will be <br />
        ///     added to the end to create a unique name.
        /// </summary>
        /// <param name="nodeName">The name.</param>
        /// <returns>A name that is unique to the node's children.</returns>
        protected string CreateValidName(string nodeName)
        {
            if (nodeName == null)
            {
                throw new ArgumentNullException(nameof(nodeName));
            }

            if (!this.children.ContainsKey(nodeName))
            {
                return nodeName;
            }

            var num = 1;
            var numberedName = nodeName + num;
            while (this.children.ContainsKey(numberedName))
            {
                num += 1;
                numberedName = nodeName + num;
            }

            return numberedName;
        }

        /// <summary>
        ///     Retrieves a node by its name.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>The node with the matching name if one exists, otherwise null</returns>
        public Node GetChildByName(string nodeName)
        {
            return this.children.ContainsKey(nodeName) ? this.children[nodeName] : null;
        }

        /// <summary>
        ///     Gets all collision areas below the node, including the node itself.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <returns>A list of all collision areas below the node</returns>
        public IList<CollisionArea> GetCollisionAreas()
        {
            var areas = new List<CollisionArea>();

            if (this is CollisionArea)
            {
                areas.Add(this as CollisionArea);
            }

            foreach (var child in this.children.Values)
            {
                areas.AddRange(child.GetCollisionAreas());
            }

            return areas;
        }

        #endregion
    }
}