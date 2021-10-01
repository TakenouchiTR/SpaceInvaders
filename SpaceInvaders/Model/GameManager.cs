using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Entities;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.Model.Entities.Enemies;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        public event EventHandler ScoreUpdated;

        #region Data members

        private const double PlayerShipBottomOffset = 30;
        private const double MillisecondsInSecond = 1000;
        private const double EnemyStartAreaWidth = 250;
        private const int EnemiesPerRow = 4;

        private Canvas background;
        private readonly DispatcherTimer updateTimer;
        private readonly HashSet<GameObject> gameObjects;
        private readonly HashSet<GameObject> removalQueue;
        private readonly HashSet<GameObject> additionQueue;

        private long prevUpdateTime;
        private int score;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the width of the screen.
        /// </summary>
        /// <value>
        ///     The width of the screen.
        /// </value>
        public double ScreenWidth { get; }

        /// <summary>
        ///     Gets the height of the screen.
        /// </summary>
        /// <value>
        ///     The height of the screen.
        /// </value>
        public double ScreenHeight { get; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score
        {
            get => this.score;
            set
            {
                this.score = value;
                this.ScoreUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

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

            this.ScreenHeight = backgroundHeight;
            this.ScreenWidth = backgroundWidth;

            this.updateTimer = new DispatcherTimer {
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
            this.createAndPlaceEnemyShips();
        }

        private void createAndPlacePlayerShip(Canvas background)
        {
            var playerShip = new PlayerShip(this);
            playerShip.Removed += onPlayerShipRemoved;

            this.placePlayerShipNearBottomOfBackgroundCentered(playerShip);
        }

        private void onPlayerShipRemoved(object sender, EventArgs e)
        {
            //todo add gameover screen
        }

        private void placePlayerShipNearBottomOfBackgroundCentered(PlayerShip playerShip)
        {
            playerShip.Center = new Vector2(this.ScreenWidth / 2,
                this.ScreenHeight - playerShip.Height - PlayerShipBottomOffset);
            this.QueueGameObjectForAddition(playerShip);
        }

        private void createAndPlaceEnemyShips()
        {
            double spaceWidth = EnemyStartAreaWidth / EnemiesPerRow;
            double startX = ScreenWidth / 2 - EnemyStartAreaWidth / 2;
            startX += spaceWidth / 2;

            double startY = 48;
            int yGap = 64;

            var enemies = new List<Enemy>()
            {
                new AggresiveEnemy(this),
                new AggresiveEnemy(this),
                new AggresiveEnemy(this),
                new AggresiveEnemy(this),
                new IntermediateEnemy(this),
                new IntermediateEnemy(this),
                new IntermediateEnemy(this),
                new IntermediateEnemy(this),
                new BasicEnemy(this),
                new BasicEnemy(this),
                new BasicEnemy(this),
                new BasicEnemy(this)
            };

            for (var i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                var xPos = startX + i % EnemiesPerRow * spaceWidth;
                var yPos = startY + i / EnemiesPerRow * yGap;

                enemy.Position = new Vector2(xPos, yPos);
                enemy.Removed += onEnemyRemoved;
                QueueGameObjectForAddition(enemy);
            }
        }

        private void onEnemyRemoved(object sender, EventArgs e)
        {
            if (sender is Enemy enemy)
            {
                Score += enemy.Score;
                enemy.Removed -= this.onEnemyRemoved;
            }
        }

        private void onUpdateTimerTick(object sender, object e)
        {
            var curTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var timeSinceLastTick = curTime - this.prevUpdateTime;
            var delta = timeSinceLastTick / MillisecondsInSecond;
            this.prevUpdateTime = curTime;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Update(delta);
            }

            this.addObjectsInQueue();
            this.removeObjectsInQueue();
        }

        /// <summary>
        ///     Queues the specified game object for removal at the end of the update tick.
        ///     Removal is deferred in case the object is needed for other purposes during the update tick.
        ///     Precondition: obj != null
        ///     Postcondition: obj is removed at the end of the update tick
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <exception cref="System.ArgumentException">obj must not be null</exception>
        public void QueueGameObjectForRemoval(GameObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("obj must not be null");
            }

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

        /// <summary>
        ///     Queues the specified game object for adding at the end of the update tick.
        ///     Addition is deferred to prevent errors with updating the set of game objects while iterating over it.
        ///     Precondition: obj != null
        ///     Postcondition: obj is added at the end of the update tick
        /// </summary>
        /// <param name="obj">The object to add.</param>
        /// <exception cref="System.ArgumentException">obj must not be null</exception>
        public void QueueGameObjectForAddition(GameObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("obj must not be null");
            }

            this.additionQueue.Add(obj);
        }

        private void addObjectsInQueue()
        {
            foreach (var gameObject in this.additionQueue)
            {
                this.gameObjects.Add(gameObject);
                gameObject.Moved += this.onGameObjectMoved;
                this.background.Children.Add(gameObject.Sprite);
            }

            this.additionQueue.Clear();
        }

        private void onGameObjectMoved(object sender, EventArgs e)
        {
            if (sender is GameObject movedObject)
            {
                checkObjectCollisions(movedObject);
            }
        }

        private void checkObjectCollisions(GameObject movedObject)
        {
            foreach (var target in this.gameObjects)
            {
                if (movedObject == target)
                {
                    continue;
                }

                if (movedObject.IsCollidingWith(target))
                {
                    movedObject.HandleCollision(target);
                }

                if (target.IsCollidingWith(movedObject))
                {
                    target.HandleCollision(movedObject);
                }
            }
        }

        #endregion
    }
}