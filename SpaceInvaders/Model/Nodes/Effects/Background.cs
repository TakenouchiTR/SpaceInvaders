using System;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Effects
{
    /// <summary>
    ///     Creates a background of celestial bodies
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public class Background : Node
    {
        #region Data members

        private const int StarCount = 100;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Background" /> class.
        /// </summary>
        public Background()
        {
            this.addStars();
            this.addPlanet();
        }

        #endregion

        #region Methods

        private void addStars()
        {
            var starRandom = new Random();

            for (var i = 0; i < StarCount; ++i)
            {
                var star = new BackgroundStar {
                    Y = starRandom.NextDouble() * MainPage.ApplicationHeight
                };
                AttachChild(star);
            }
        }

        private void addPlanet()
        {
            AttachChild(new BackgroundPlanet());
        }

        #endregion
    }
}