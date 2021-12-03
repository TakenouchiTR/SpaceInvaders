using System;

namespace SpaceInvaders.Extensions
{
    /// <summary>
    ///     Extensions for the Random class
    /// </summary>
    public static class RandomExtensions
    {
        #region Methods

        /// <summary>
        ///     Returns a random double within the specified range, inclusive.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="random">The random object.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>A random double within the specified range</returns>
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        #endregion
    }
}