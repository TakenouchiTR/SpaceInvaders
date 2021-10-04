using System;

namespace SpaceInvaders.Model
{
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
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2"/> class. <br/>
        ///     Precondition: !double.IsNaN(value) && !double.IsInfinity(value)
        ///     PostCondition: this.X == value && this.Y == value
        /// </summary>
        /// <param name="value">The X and Y value.</param>
        public Vector2(double value) : this(value, value)
        {
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2"/> class. <br/>
        ///     Precondition: !double.IsNaN(x) && !double.IsInfinity(x) &&
        ///                   !double.IsNaN(y) && !double.IsInfinity(y)
        ///     PostCondition: this.X == value && this.Y == value
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// x must not be NaN
        /// or
        /// y must not be NaN
        /// or
        /// x must not be infinity
        /// or
        /// y must not be infinity
        /// </exception>
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Methods

        //Todo ask about overriding operators

        /// <summary>
        ///     Implements the operator *.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
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