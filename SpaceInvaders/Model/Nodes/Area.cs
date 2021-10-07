using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes
{
    public class Area : Node2D
    {
        #region Properties

        /// <summary>
        ///     The left side of the area.<br />
        ///     Equivalent to this.X.
        /// </summary>
        /// <value>
        ///     The left side of the area.
        /// </value>
        public double Left => X;

        /// <summary>
        ///     Gets the right on the area.<br />
        /// </summary>
        /// <value>
        ///     The right side of the area.
        /// </value>
        public double Right => X + this.Width;

        /// <summary>
        ///     Gets the top side of the area.<br />
        ///     Equivalent to this.Y.
        /// </summary>
        /// <value>
        ///     The top side of the area.
        /// </value>
        public double Top => Y;

        /// <summary>
        ///     Gets the bottom side of the area.<br />
        /// </summary>
        /// <value>
        ///     The bottom.
        /// </value>
        public double Bottom => Y + this.Height;

        /// <summary>
        ///     Gets or sets the center coordinate of the area.
        /// </summary>
        /// <value>
        ///     The center coordinate of the area.
        /// </value>
        public Vector2 Center
        {
            get => new Vector2(X + this.Width / 2, Y + this.Width / 2);
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

        #region Methods

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