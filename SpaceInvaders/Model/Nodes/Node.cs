using System;
using System.Collections.Generic;

namespace SpaceInvaders.Model.Nodes
{
    public class Node
    {
        private readonly Queue<Node> removalQueue;
        private readonly Queue<Node> additionQueue;

        protected HashSet<Node> children;

        public Node Parent { get; protected set; }

        public event EventHandler Removed;

        public Node()
        {
            this.removalQueue = new Queue<Node>();
            this.additionQueue = new Queue<Node>();
            this.children = new HashSet<Node>();
        }

        /// <summary>
        ///     The update loop for the GameObject.
        ///     Precondition: None
        ///     Postcondition: GameObject completes its update step
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
        ///     Runs cleanup code and invokes the Removed event when removed from the game.
        ///     Precondition: None
        ///     Postcondition: Removed event is invoked
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

        public void QueueForRemoval()
        {
            this.Parent?.QueueGameObjectForRemoval(this);
            foreach (var child in this.children)
            {
                child.QueueForRemoval();
            }
        }

        public void QueueGameObjectForRemoval(Node obj)
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
                var gameObject = this.removalQueue.Dequeue();

                if (this.children.Contains(gameObject))
                {
                    this.children.Remove(gameObject);
                    gameObject.CompleteRemoval();
                }
            }

            this.removalQueue.Clear();
        }

        /// <summary>
        ///     Queues the specified game object for adding at the end of the update tick.
        ///     Addition is deferred to prevent errors with updating the set of game objects while iterating over it.
        ///     Precondition: obj != null
        ///     Postcondition: obj is added at the end of the update tick
        /// </summary>
        /// <param name="obj">The object to add.</param>
        /// <exception cref="System.ArgumentException">obj must not be null</exception>
        public void QueueGameObjectForAddition(Node newNode)
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
                var gameObject = this.additionQueue.Dequeue();
                AttachChild(gameObject);
            }
        }


        /// <summary>
        ///  Adds a child to the GameObject.
        /// Precondition: child != null
        /// Postcondition: None
        /// </summary>
        /// <param name="child">The child.</param>
        /// <exception cref="System.ArgumentException">child must not be null</exception>
        public void AttachChild(Node child)
        {
            if (child == null)
            {
                throw new ArgumentException("child must not be null");
            }

            this.children.Add(child);
            child.Parent = this;
            child.Removed += this.onChildRemoved;
        }

        /// <summary>
        ///     Detaches the child.
        ///     Precondition: child != null
        ///     Postcondition: !this.Children.Contains(child)
        /// </summary>
        /// <param name="child">The child to detach.</param>
        /// <exception cref="System.ArgumentException">child must not be null</exception>
        public void DetachChild(Node child)
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
        ///     Detaches self from parent.
        ///     Precondition: None
        ///     Postcondition: this.Parent == null
        ///                    !this.Parent@prev.Children.Contains(this)
        /// </summary>
        public void DetachFromParent()
        {
            if (this.Parent != null)
            {
                this.Parent.DetachChild(this);
                this.Parent = null;
            }
        }

        private void onChildRemoved(object sender, EventArgs e)
        {
            if (sender is Node child)
            {
                this.children.Remove(child);
                child.Removed -= this.onChildRemoved;
            }
        }

    }
}
