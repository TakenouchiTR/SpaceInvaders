using System;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes
{
    public class Node2D : Node
    {
        #region Data members

        /// <summary>
        ///     Occurs whenever the Node moves
        /// </summary>
        public EventHandler<Vector2> Moved;

        private Vector2 position;

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
                foreach (var child in children)
                {
                    if (child is Node2D node)
                    {
                        node.X += moveDistance;
                    }
                }

                this.Moved?.Invoke(this, new Vector2(moveDistance, 0));
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
                foreach (var child in children)
                {
                    if (child is Node2D node)
                    {
                        node.Y += moveDistance;
                    }
                }

                this.Moved?.Invoke(this, new Vector2(0, moveDistance));
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

        #endregion

        #region Methods

        /// <summary>
        ///     Moves by the specified distance.<br />
        ///     Precondition: None
        ///     Postcondition: this.position == this.position@prev + distance
        /// </summary>
        /// <param name="distance">The distance.</param>
        public void Move(Vector2 distance)
        {
            this.X += distance.X;
            this.Y += distance.Y;
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        public override void CompleteRemoval()
        {
            base.CompleteRemoval();

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