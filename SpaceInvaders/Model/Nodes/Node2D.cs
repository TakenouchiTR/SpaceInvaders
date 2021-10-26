using System;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A node with an X/Y components.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public class Node2D : Node
    {
        #region Data members

        /// <summary>
        ///     Occurs whenever the Node moves
        /// </summary>
        public EventHandler<Vector2> Moved;

        private Vector2 position;
        private bool suppressMoveEvent;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the x coordinate.
        /// </summary>
        /// <value>
        ///     The x coordinate.
        /// </value>
        public virtual double X
        {
            get => this.position.X;
            set
            {
                var moveDistance = value - this.X;
                foreach (var child in Children)
                {
                    if (child is Node2D node)
                    {
                        node.X += moveDistance;
                    }
                }

                if (this.Moved != null && !this.suppressMoveEvent)
                {
                    this.Moved.Invoke(this, new Vector2(moveDistance, 0));
                }

                this.position.X = value;
            }
        }

        /// <summary>
        ///     Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        ///     The y coordinate.
        /// </value>
        public virtual double Y
        {
            get => this.position.Y;
            set
            {
                var moveDistance = value - this.Y;
                foreach (var child in Children)
                {
                    if (child is Node2D node)
                    {
                        node.Y += moveDistance;
                    }
                }

                if (this.Moved != null && !this.suppressMoveEvent)
                {
                    this.Moved.Invoke(this, new Vector2(0, moveDistance));
                }

                this.position.Y = value;
            }
        }

        /// <summary>
        ///     Gets or sets both the X and Y coordinates.
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public virtual Vector2 Position
        {
            get => new Vector2(this.X, this.Y);
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        ///     Gets or sets the position relative to the parent.<br />
        ///     If the node is the root node, this is functionally equivalent to this.Position.
        /// </summary>
        /// <value>
        ///     The position relative to the parent.
        /// </value>
        public virtual Vector2 RelativePosition
        {
            get
            {
                if (Parent is Node2D parentNode2D)
                {
                    return this.Position - parentNode2D.Position;
                }

                return this.Position;
            }
            set
            {
                if (Parent is Node2D parentNode2D)
                {
                    this.Position = parentNode2D.Position + value;
                }

                this.Position = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node2D" /> class at the coordinates (0, 0).<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        public Node2D()
        {
            this.suppressMoveEvent = false;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node2D" /> class and sets its coordinates
        ///     Precondition: None<br />
        ///     Postcondition: this.X == x &amp;&amp;<br />
        ///     this.y == y
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Node2D(double x, double y)
        {
            this.applyCoordinates(x, y);
        }

        #endregion

        #region Methods

        private void applyCoordinates(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        ///     Moves by the specified distance.<br />
        ///     Precondition: None
        ///     Postcondition: this.position == this.position@prev + distance
        /// </summary>
        /// <param name="distance">The distance.</param>
        public void Move(Vector2 distance)
        {
            this.suppressMoveEvent = true;

            this.X += distance.X;
            this.Y += distance.Y;

            this.suppressMoveEvent = false;

            this.Moved?.Invoke(this, distance);
        }

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

            if (this.Moved != null)
            {
                foreach (var subscriber in this.Moved?.GetInvocationList())
                {
                    Removed -= subscriber as EventHandler;
                }
            }
        }

        /// <summary>
        ///     Determines whether [is off screen].<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is off screen]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsOffScreen()
        {
            return this.X > MainPage.ApplicationWidth ||
                   this.X < 0 ||
                   this.Y > MainPage.ApplicationHeight ||
                   this.Y < 0;
        }

        #endregion
    }
}