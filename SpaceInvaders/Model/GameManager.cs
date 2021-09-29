using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Entities;
using SpaceInvaders.View.Sprites;

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

        private Canvas background;
        private readonly DispatcherTimer updateTimer;
        private readonly HashSet<GameObject> gameObjects;
        private readonly HashSet<GameObject> removalQueue;
        private readonly HashSet<GameObject> additionQueue;
        private long prevUpdateTime;

        #endregion

        #region Properties

        public double ScreenWidth => this.backgroundWidth;
        public double ScreenHeight => this.backgroundHeight;
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
            this.removalQueue = new HashSet<GameObject>();
            this.additionQueue = new HashSet<GameObject>();
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

            this.background = background;
            this.createAndPlacePlayerShip(background);
        }

        private void createAndPlacePlayerShip(Canvas background)
        {
            PlayerShip playerShip = new PlayerShip(this);

            this.placePlayerShipNearBottomOfBackgroundCentered(playerShip);
        }

        private void placePlayerShipNearBottomOfBackgroundCentered(PlayerShip playerShip)
        {
            playerShip.Center = new Vector2(this.backgroundWidth / 2,
                this.backgroundHeight - playerShip.Height - PlayerShipBottomOffset);
            QueueGameObjectForAddition(playerShip);
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

            this.addObjectsInQueue();
            this.removeObjectsInQueue();
        }


        /// <summary>
        ///     Queues the game object for removal at the end of the update tick.
        ///     Removal is deferred in case the object is needed for other purposes during the update tick.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void QueueGameObjectForRemoval(GameObject obj)
        {
            this.removalQueue.Add(obj);
        }

        private void removeObjectsInQueue()
        {
            foreach (var gameObject in this.removalQueue)
            {
                if (this.gameObjects.Contains(gameObject))
                {
                    this.gameObjects.Remove(gameObject);
                    gameObject.Moved -= this.onGameObjectMoved;
                    this.removeSpriteFromBackground(gameObject.Sprite);
                    gameObject.CompleteRemoval();
                }
            }

            this.removalQueue.Clear();
        }

        private void removeSpriteFromBackground(BaseSprite sprite)
        {
            this.background.Children.Remove(sprite);
        }

        public void QueueGameObjectForAddition(GameObject obj)
        {
            this.additionQueue.Add(obj);
        }

        private void addObjectsInQueue()
        {
            foreach (var gameObject in this.additionQueue)
            {
                this.gameObjects.Add(gameObject);
                gameObject.Moved += this.onGameObjectMoved;
                background.Children.Add(gameObject.Sprite);
            }

            this.additionQueue.Clear();
        }

        private void onGameObjectMoved(GameObject sender, EventArgs e)
        {
            foreach (var target in this.gameObjects)
            {
                if (sender == target)
                {
                    continue;
                }

                if (sender.IsCollidingWith(target))
                {
                    sender.HandleCollision(target);
                }

                if (target.IsCollidingWith(sender))
                {
                    target.HandleCollision(sender);
                }
            }
        }

        #endregion


    }
}
