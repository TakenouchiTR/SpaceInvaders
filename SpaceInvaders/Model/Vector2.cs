using System;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Represents an XY coordinate pair.
    /// </summary>
    public struct Vector2
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
        ///     value must not be NaN
        ///     or
        ///     value must not be infinity
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
        ///     value must not be NaN
        ///     or
        ///     value must not be infinity
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

        /// <summary>
        ///     Gets the magnitude.
        /// </summary>
        /// <value>
        ///     The magnitude.
        /// </value>
        public double Magnitude => Math.Sqrt(this.X * this.X + this.Y * this.Y);

        /// <summary>
        ///     Gets the magnitude squared.
        /// </summary>
        /// <value>
        ///     The magnitude squared.
        /// </value>
        public double MagnitudeSquared => this.X * this.X + this.Y * this.Y;

        /// <summary>
        ///     Creates a Vector2 pointing up (0, -1).
        /// </summary>
        /// <value>
        ///     A Vector2 pointing up
        /// </value>
        public static Vector2 Up => new Vector2(0, -1);

        /// <summary>
        ///     Creates a Vector2 pointing down (0, 1).
        /// </summary>
        /// <value>
        ///     A Vector2 pointing down
        /// </value>
        public static Vector2 Down => new Vector2(0, 1);

        /// <summary>
        ///     Creates a Vector2 pointing left (-1, 0).
        /// </summary>
        /// <value>
        ///     A Vector2 pointing left
        /// </value>
        public static Vector2 Left => new Vector2(-1, 0);

        /// <summary>
        ///     Creates a Vector2 pointing right (1, 0).
        /// </summary>
        /// <value>
        ///     A Vector2 pointing right
        /// </value>
        public static Vector2 Right => new Vector2(1, 0);

        /// <summary>
        ///     Creates a Vector2 with both components set to 1.
        /// </summary>
        /// <value>
        ///     A Vector2 with both components set to 1
        /// </value>
        public static Vector2 One => new Vector2(1);

        /// <summary>
        ///     Creates a Vector2 with both components set to 0.
        /// </summary>
        /// <value>
        ///     A Vector2 with both components set to 0
        /// </value>
        public static Vector2 Zero => new Vector2();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2" /> class. <br />
        ///     Precondition: !double.IsNaN(value) &amp;&amp; !double.IsInfinity(value)
        ///     PostCondition: this.X == value &amp;&amp; this.Y == value
        /// </summary>
        /// <param name="value">The X and Y value.</param>
        /// <exception cref="System.ArgumentException">
        ///     value must not be NaN
        ///     or
        ///     value must not be infinity
        /// </exception>
        public Vector2(double value)
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
            this.y = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2" /> class. <br />
        ///     Precondition: !double.IsNaN(x) &amp;&amp; !double.IsInfinity(x) &amp;&amp;
        ///     !double.IsNaN(y) &amp;&amp; !double.IsInfinity(y)
        ///     PostCondition: this.X == value &amp;&amp; this.Y == value
        /// </summary>
        /// <exception cref="System.ArgumentException">
        ///     x must not be NaN
        ///     or
        ///     y must not be NaN
        ///     or
        ///     x must not be infinity
        ///     or
        ///     y must not be infinity
        /// </exception>
        public Vector2(double x, double y)
        {
            if (double.IsNaN(x))
            {
                throw new ArgumentException("x must not be NaN");
            }

            if (double.IsNaN(y))
            {
                throw new ArgumentException("y must not be NaN");
            }

            if (double.IsInfinity(x))
            {
                throw new ArgumentException("x must not be infinity");
            }

            if (double.IsInfinity(y))
            {
                throw new ArgumentException("y must not be infinity");
            }

            this.x = x;
            this.y = y;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Calculates the angle to the specified Vector2.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The angle to the target Vector2</returns>
        public double AngleToTarget(Vector2 target)
        {
            return Math.Atan2(target.Y - this.Y, target.X - this.X);
        }

        /// <summary>
        ///     Normalizeds the direction to.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public Vector2 NormalizedVectorTo(Vector2 target)
        {
            var angle = this.AngleToTarget(target);
            return AngleToNormalizedVector(angle);
        }

        /// <summary>
        ///     Gets the distance to a specified Vector2.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The distance to the specified Vector2</returns>
        public double DistanceTo(Vector2 target)
        {
            var difference = this - target;
            return difference.Magnitude;
        }

        /// <summary>
        ///     Gets the distance squared to a specified Vector2.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The distance squared to the specified Vector2</returns>
        public double DistanceToSquared(Vector2 target)
        {
            var difference = this - target;
            return difference.MagnitudeSquared;
        }

        /// <summary>
        ///     Converts the Vector2 to an angle.
        /// </summary>
        /// <returns>An angle pointing the same direction as the Vector2</returns>
        public double ToAngle()
        {
            return Math.Atan2(this.Y, this.X);
        }

        /// <summary>
        ///     Creates a normalized version of the Vector2
        /// </summary>
        /// <returns>The normalized Vector2</returns>
        public Vector2 Normalized()
        {
            var magnitude = this.Magnitude;
            return new Vector2(this.X / magnitude, this.Y / magnitude);
        }

        /// <summary>
        ///     Converts an angle to a normalized vector
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>A normalized vector going the same direction as the angle.</returns>
        public static Vector2 AngleToNormalizedVector(double angle)
        {
            return new Vector2(Math.Cos(angle), Math.Sin(angle));
        }

        /// <summary>
        ///     Implements the * operator between a Vector2 and a double.<br />
        ///     Each component of the Vector2 will be multiplied by the scalar.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>
        ///     The result of the operator
        /// </returns>
        public static Vector2 operator *(Vector2 vector, double scalar)
        {
            return new Vector2(vector.X * scalar, vector.Y * scalar);
        }

        /// <summary>
        ///     Implements the operator / between a Vector2 and a double.
        ///     Each component of the Vector 2 will be divided by the scalar. The scalar <i>must not</i> be 0.<br />
        ///     Precondition: scalar != 0<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        /// <exception cref="System.ArgumentException">Scalar cannot be 0</exception>
        public static Vector2 operator /(Vector2 vector, double scalar)
        {
            if (scalar == 0)
            {
                throw new ArgumentException("Scalar cannot be 0");
            }

            return new Vector2(vector.X / scalar, vector.Y / scalar);
        }

        /// <summary>
        ///     Implements the + operator between two Vector2s<br />
        ///     Creates a new Vector2 by adding the X and Y components of the two Vector2s together.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        ///     Implements the - operator between two Vector2s<br />
        ///     Creates a new Vector2 by subtracting the X and Y components of the second Vector2 from the first.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        ///     Converts to the string (this.X, this.Y).<br />
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }

        #endregion
    }
}