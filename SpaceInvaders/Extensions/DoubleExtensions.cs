using System;

namespace SpaceInvaders.Extensions
{
    /// <summary>
    ///     Extensions for the double data type
    /// </summary>
    public static class DoubleExtensions
    {
        #region Methods

        /// <summary>
        ///     Converts a degree value to radians
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>The radian conversion of the value</returns>
        public static double DegreeToRadian(this double degree)
        {
            return degree * Math.PI / 180;
        }

        /// <summary>
        ///     Converts a radians to degrees.
        /// </summary>
        /// <param name="radian">The radian.</param>
        /// <returns>The degree conversion of the value</returns>
        public static double RadianToDegree(this double radian)
        {
            return radian * 180 / Math.PI;
        }

        #endregion
    }
}