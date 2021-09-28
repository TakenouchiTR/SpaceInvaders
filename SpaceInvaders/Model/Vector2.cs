using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
