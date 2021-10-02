namespace SpaceInvaders.Model
{
    public class Vector2
    {
        private double x;
        private double y;

        #region Properties

        /// <summary>
        ///     Gets or sets the x coordinate.
        /// </summary>
        /// <value>
        ///     The x coordinate.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// value must not be NaN
        /// or
        /// value must not be infinity
        /// </exception>
        public double X
        {
            get => this.x;
            set
            {
                if (double.IsNaN(value))
                {
                    throw new ArgumentException("value must not be NaN");
                }
                if (double.IsInfinity(value))
                {
                    throw new ArgumentException("value must not be infinity");
                }

                this.x = value;
            }
        }

        /// <summary>
        ///     Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        ///     The y coordinate.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// value must not be NaN
        /// or
        /// value must not be infinity
        /// </exception>
        public double Y
        {
            get => this.y;
            set
            {
                if (double.IsNaN(value))
                {
                    throw new ArgumentException("value must not be NaN");
                }
                if (double.IsInfinity(value))
                {
                    throw new ArgumentException("value must not be infinity");
                }

                this.y = value;
            }
        }

        #endregion

        #region Constructors

        public Vector2() : this(0)
        {
        }

        public Vector2(double value) : this(value, value)
        {
        }

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Methods

        public static Vector2 operator *(Vector2 vector, double value)
        {
            return new Vector2(vector.X * value, vector.Y * value);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }

        #endregion
    }
}