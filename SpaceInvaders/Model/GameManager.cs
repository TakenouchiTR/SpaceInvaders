using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Entities;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const double PlayerShipBottomOffset = 30;
        private const double MillisecondsInSecond = 1000;

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private readonly DispatcherTimer updateTimer;
        private readonly HashSet<GameObject> gameObjects;
        private long prevUpdateTime;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0
        /// </summary>
        /// <param name="backgroundHeight">The backgroundHeight of the game play window.</param>
        /// <param name="backgroundWidth">The backgroundWidth of the game play window.</param>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;

            this.updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(.1)
            };
            this.updateTimer.Tick += this.onUpdateTimerTick;
            this.updateTimer.Start();

            this.gameObjects = new HashSet<GameObject>();
        }

        private void onUpdateTimerTick(object sender, object e)
        {
            long curTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long timeSinceLastTick = curTime - this.prevUpdateTime;
            double delta = timeSinceLastTick / MillisecondsInSecond;
            this.prevUpdateTime = curTime;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Update(delta);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the game placing player ship and enemy ship in the game.
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="background">The background canvas.</param>
        public void InitializeGame(Canvas background)
        {
            if (background == null)
            {
                throw new ArgumentNullException(nameof(background));
            }

            this.createAndPlacePlayerShip(background);
        }

        private void createAndPlacePlayerShip(Canvas background)
        {
            PlayerShip playerShip = new PlayerShip();

            background.Children.Add(playerShip.Sprite);
            this.gameObjects.Add(playerShip);

            this.placePlayerShipNearBottomOfBackgroundCentered(playerShip);
        }

        private void placePlayerShipNearBottomOfBackgroundCentered(PlayerShip playerShip)
        {
            playerShip.X = this.backgroundWidth / 2 - playerShip.Width / 2.0;
            playerShip.Y = this.backgroundHeight - playerShip.Height - PlayerShipBottomOffset;
        }
        #endregion


    }
}
