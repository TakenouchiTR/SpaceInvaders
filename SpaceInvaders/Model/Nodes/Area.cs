using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A node that represents a rectangle, with a specified X coordinate, Y coordinate, width, and height.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node2D" />
    public class Area : Node2D
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the left side of the area.<br />
        ///     Equivalent to this.X.
        /// </summary>
        /// <value>
        ///     The left side of the area.
        /// </value>
        public double Left
        {
            get => X;
            set => X = value;
        }

        /// <summary>
        ///     Gets or sets the right on the area.<br />
        /// </summary>
        /// <value>
        ///     The right side of the area.
        /// </value>
        public double Right
        {
            get => X + this.Width;
            set => X = value - this.Width;
        }

        /// <summary>
        ///     Gets or sets the top side of the area.<br />
        ///     Equivalent to this.Y.
        /// </summary>
        /// <value>
        ///     The top side of the area.
        /// </value>
        public double Top
        {
            get => Y;
            set => Y = value;
        }

        /// <summary>
        ///     Gets or sets the bottom side of the area.<br />
        /// </summary>
        /// <value>
        ///     The bottom.
        /// </value>
        public double Bottom
        {
            get => Y + this.Height;
            set => Y = value - this.Height;
        }

        /// <summary>
        ///     Gets or sets the center coordinate of the area.
        /// </summary>
        /// <value>
        ///     The center coordinate of the area.
        /// </value>
        public Vector2 Center
        {
            get => new Vector2(X + this.Width / 2, Y + this.Height / 2);
            set
            {
                X = value.X - this.Width / 2;
                Y = value.Y - this.Height / 2;
            }
        }

        /// <summary>
        ///     Gets or sets the width of the area.
        /// </summary>
        /// <value>
        ///     The width of the area.
        /// </value>
        public virtual double Width { get; set; }

        /// <summary>
        ///     Gets or sets the height of the area.
        /// </summary>
        /// <value>
        ///     The height of the area.
        /// </value>
        public virtual double Height { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Area" /> class.<br />
        ///     The area will have no width, height, and will be placed at (0, 0).<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        public Area()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Area" /> class with a specified width and height at the coordinates
        ///     (0, 0).<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Height == height &amp;&amp;<br />
        ///     this.Width == width
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Area(double width, double height) : this(0, 0, width, height)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Area" /> class with a specified width, height, and coordinates.
        ///     Precondition: None<br />
        ///     Postcondition: this.Height == height &amp;&amp;<br />
        ///     this.Width == width &amp;&amp;<br />
        ///     this.X == x &amp;&amp;<br />
        ///     this.Y == y
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Area(double x, double y, double width, double height) : base(x, y)
        {
            this.applySize(width, height);
        }

        #endregion

        #region Methods

        private void applySize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        ///     Checks if a specified point is within the Area.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>Whether the specified point is within the area</returns>
        public bool ContainsPoint(Vector2 point)
        {
            return point.X >= this.Left && point.X <= this.Right &&
                   point.Y >= this.Top && point.Y <= this.Bottom;
        }

        /// <summary>
        ///     Determines whether [is off screen].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is off screen]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsOffScreen()
        {
            return this.Left > MainPage.ApplicationWidth ||
                   this.Right < 0 ||
                   this.Top > MainPage.ApplicationHeight ||
                   this.Bottom < 0;
        }

        #endregion
    }
}