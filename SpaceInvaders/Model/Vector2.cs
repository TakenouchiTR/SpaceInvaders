namespace SpaceInvaders.Model
{
    public class Vector2
    {
        #region Properties

        public double X { get; set; }
        public double Y { get; set; }

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

        #endregion
    }
}