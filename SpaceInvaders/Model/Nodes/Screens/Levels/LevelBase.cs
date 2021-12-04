using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Extensions;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.Model.Nodes.UI;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites.UI;

namespace SpaceInvaders.Model.Nodes.Screens.Levels
{
    /// <summary>
    ///     The basic structure for other levels.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public abstract class LevelBase : Screen
    {
        #region Data members

        /// <summary>
        ///     The default number of shields.
        /// </summary>
        protected const int ShieldCount = 3;

        /// <summary>
        ///     The buffer between the edge of the screen and the UI, in pixels.
        /// </summary>
        protected const double UiBuffer = 4;

        private const int MaxGrazePoints = 150;
        private const int MaxTimeBonus = 500;
        private const int PointsPerLife = 100;
        private const int PointsLostPerSecond = 5;
        private const int PointsPerShieldSegment = 2;

        private const int MinBonusEnemyInterval = 5;
        private const int MaxBonusEnemyInterval = 10;

        private static readonly Random BonusEnemyRandom = new Random();

        private int enemiesRemaining;
        private double timeInLevel;
        private bool levelComplete;
        private bool canEarnPoints;
        private Type nextScreen;
        private Dictionary<PointSource, int> scoreBreakdown;

        private PlayerShip player;
        private Node shieldNode;
        private Label scoreLabel;
        private LifeCounter lifeCounter;
        private GrazeBar grazeBar;
        private Timer bonusShipTimer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score => this.scoreBreakdown.Values.Sum();

        /// <summary>
        ///     Gets or sets the gameplay speed modifier.
        /// </summary>
        /// <value>
        ///     The speed modifier.
        /// </value>
        public double SpeedModifier { get; set; }

        /// <summary>
        ///     Gets or sets the next level.
        /// </summary>
        /// <value>
        ///     The next level.
        /// </value>
        protected Type NextLevel { get; set; }

        /// <summary>
        ///     Gets or sets the remaining bonus enemies.
        /// </summary>
        /// <value>
        ///     The remaining bonus enemies.
        /// </value>
        public int RemainingBonusEnemies { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelBase" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="nextLevel">The next level.</param>
        protected LevelBase(Type nextLevel)
        {
            this.NextLevel = nextLevel;
            SessionStats.Level++;
            this.SpeedModifier = 1;
            this.canEarnPoints = true;

            this.addPlayer();
            this.addUi();
            this.addShields();
            this.setupScoreBreakdown();
            this.setupBonusEnemies();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when the score is changed.
        /// </summary>
        public event EventHandler<int> ScoreChanged;

        private void addPlayer()
        {
            this.player = new PlayerShip();
            this.player.X = MainPage.ApplicationWidth / 2 - this.player.Collision.Width / 2;
            this.player.Y = MainPage.ApplicationHeight - 64;

            this.AttachChild(this.player);

            this.player.Removed += this.onPlayerRemoved;
        }

        private void addUi()
        {
            this.scoreLabel = new Label("Score: 0", RenderLayer.UiTop);
            this.scoreLabel.Position += new Vector2(UiBuffer);
            this.player.CurrentLivesChanged += this.onPlayerLivesChanged;

            this.lifeCounter = new LifeCounter(typeof(FullHeartSprite), typeof(EmptyHeartSprite), 3, RenderLayer.UiTop);
            this.lifeCounter.X = MainPage.ApplicationWidth - this.lifeCounter.Width - UiBuffer;
            this.lifeCounter.Y += UiBuffer;

            this.grazeBar = new GrazeBar(RenderLayer.UiMiddle);
            this.grazeBar.Y = MainPage.ApplicationHeight / 2 - this.grazeBar.Height / 2;
            this.player.GrazeMeterChanged += this.onPlayerGrazeMeterChanged;

            this.AttachChild(this.scoreLabel);
            this.AttachChild(this.lifeCounter);
            this.AttachChild(this.grazeBar);
        }

        private void addShields()
        {
            this.shieldNode = new Node();

            var y = MainPage.ApplicationHeight - 160;
            for (var shieldIndex = 1; shieldIndex <= ShieldCount; shieldIndex++)
            {
                var x = MainPage.ApplicationWidth / (ShieldCount + 1) * shieldIndex;
                var currentShield = new LevelShield(5, 3) {
                    Center = new Vector2(x, y)
                };

                this.shieldNode.AttachChild(currentShield);
            }

            this.AttachChild(this.shieldNode);
        }

        private void setupScoreBreakdown()
        {
            this.scoreBreakdown = new Dictionary<PointSource, int>();
            var sources = (PointSource[]) Enum.GetValues(typeof(PointSource));

            foreach (var source in sources)
            {
                this.scoreBreakdown[source] = 0;
            }

            this.AddPoints(PointSource.PreviousLevel, SessionStats.Score);
        }

        private void setupBonusEnemies()
        {
            this.RemainingBonusEnemies = 3;

            this.bonusShipTimer = new Timer(BonusEnemyRandom.NextDouble(MinBonusEnemyInterval, MaxBonusEnemyInterval)) {
                Repeat = false
            };
            this.bonusShipTimer.Start();
            this.bonusShipTimer.Tick += this.onBonusEnemyTimerTick;
            this.AttachChild(this.bonusShipTimer);
        }

        /// <summary>
        ///     Adds points from a given source. Checks to make sure that Graze points do not go over the graze limit.<br />
        ///     The score for a given category can never go below 0. <br />
        ///     Precondition: None<br />
        ///     Postcondition: if source == PointSource.Graze &amp;&amp; the graze limit was reached, this.Score ==
        ///     this.Score@prev;<br />
        ///     Otherwise: this.Score == this.Score@prev + amount
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="amount">The amount.</param>
        public void AddPoints(PointSource source, int amount)
        {
            if (source == PointSource.Graze)
            {
                amount = Math.Min(amount, MaxGrazePoints - this.scoreBreakdown[PointSource.Graze]);
            }

            this.scoreBreakdown[source] = Math.Max(this.scoreBreakdown[source] + amount, 0);
            this.updateScoreDisplay();
            this.ScoreChanged?.Invoke(this, this.Score);
        }

        /// <summary>
        ///     Sets the level up to detect when the enemy is destroyed.<br />
        ///     Precondition: enemy != null<br />
        ///     Postcondition: Score increases when the enemy is destroyed.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        /// <exception cref="System.ArgumentNullException">enemy - enemy must not be null</exception>
        protected void RegisterEnemy(Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy), "enemy must not be null");
            }

            enemy.Removed += this.onEnemyRemoved;
            enemy.Moved += this.onChildMoved;
            this.enemiesRemaining += 1;
        }

        /// <summary>
        ///     Readies the level to be ended after the current update tick.<br />
        ///     Precondition: Type != null<br />
        ///     Postcondition: The level is unloaded after the current update tick
        /// </summary>
        /// <param name="screen">The next screen.</param>
        protected void EndLevel(Type screen)
        {
            this.nextScreen = screen;
            this.levelComplete = true;

            SessionStats.Score = this.Score;
        }

        private void updateScoreDisplay()
        {
            this.scoreLabel.Text = $"Score: {this.Score}";
        }

        private void spawnBonusEnemy()
        {
            var bonusEnemy = new BonusEnemy();
            this.RegisterEnemy(bonusEnemy);
            this.RemainingBonusEnemies -= 1;
            QueueNodeForAddition(bonusEnemy);

            if (this.RemainingBonusEnemies > 0)
            {
                bonusEnemy.Removed += this.onBonusEnemyRemoved;
            }
        }

        private static void testForCollisions(IEnumerable<CollisionArea> sourceAreas, IList<CollisionArea> targetAreas)
        {
            foreach (var sourceArea in sourceAreas)
            {
                foreach (var targetArea in targetAreas)
                {
                    sourceArea.DetectCollision(targetArea);
                }
            }
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            base.Update(delta * this.SpeedModifier);
            this.timeInLevel += delta * this.SpeedModifier;

            if (this.levelComplete)
            {
                CompleteScreen(this.nextScreen);
            }
        }

        /// <summary>
        ///     Attaches a new child to the node.<br />
        ///     Precondition: child != null<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="child">The child.</param>
        public override void AttachChild(Node child)
        {
            base.AttachChild(child);
            if (child is Node2D node2D)
            {
                node2D.Moved += this.onChildMoved;
            }
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked if emitRemovedEvent == true &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        /// <param name="emitRemovedEvent">Whether to emit the Removed event</param>
        public override void CompleteRemoval(bool emitRemovedEvent = true)
        {
            base.CompleteRemoval(false);
            if (this.ScoreChanged != null)
            {
                foreach (var subscriber in this.ScoreChanged.GetInvocationList())
                {
                    this.ScoreChanged -= subscriber as EventHandler<int>;
                }
            }
        }

        private void onPlayerLivesChanged(object sender, int e)
        {
            this.lifeCounter.CurrentLives = e;
        }

        private void onPlayerGrazeMeterChanged(object sender, double e)
        {
            this.grazeBar.BarHeight = e;
        }

        private async void onPlayerRemoved(object sender, EventArgs e)
        {
            if (!this.canEarnPoints)
            {
                return;
            }

            var gameOverSound = new OneShotSoundPlayer("game_over.wav");
            this.canEarnPoints = false;
            this.bonusShipTimer.Stop();
            this.RemainingBonusEnemies = 0;
            QueueNodeForAddition(gameOverSound);

            var quitDialog = new ContentDialog {
                Title = "Game Over",
                Content = "You ran out of lives!\n" +
                          "Would you like to restart the level?\n" +
                          "You may restart as many times as you'd like, but your score will be reset to 0.",
                PrimaryButtonText = "Restart",
                SecondaryButtonText = "Return to Menu"
            };

            var dialogResult = await quitDialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                SessionStats.Level -= 1;
                this.scoreBreakdown.Clear();
                this.EndLevel(GetType());
            }
            else
            {
                this.EndLevel(typeof(HighScoresMenu));
            }
        }

        private void onEnemyRemoved(object sender, EventArgs e)
        {
            if (sender is Enemy enemy)
            {
                this.AddPoints(PointSource.Enemy, enemy.Score);
                this.enemiesRemaining--;

                if (this.enemiesRemaining <= 0)
                {
                    this.canEarnPoints = false;
                    this.scoreLabel.Visible = false;
                    this.bonusShipTimer.Stop();
                    this.RemainingBonusEnemies = 0;

                    this.calculateBonusPoints();
                    this.createLevelOverDisplay();
                }
            }
        }

        private void calculateBonusPoints()
        {
            var lifeBonus = (this.player.CurrentLives - 1) * PointsPerLife;
            var timeBonus = (int) Math.Max(MaxTimeBonus - this.timeInLevel * PointsLostPerSecond, 0);
            var shieldBonus = 0;

            foreach (var shield in this.shieldNode.Children)
            {
                shieldBonus += shield.Children.Count * PointsPerShieldSegment;
            }

            this.AddPoints(PointSource.Lives, lifeBonus);
            this.AddPoints(PointSource.Time, timeBonus);
            this.AddPoints(PointSource.Shields, shieldBonus);
        }

        private void createLevelOverDisplay()
        {
            var levelOverDisplay = new LevelOverDisplay(this.scoreBreakdown) {
                Center = new Vector2(MainPage.ApplicationWidth / 2, MainPage.ApplicationHeight / 2)
            };
            levelOverDisplay.ContinueClicked += this.onContinueClicked;
            QueueNodeForAddition(levelOverDisplay);
        }

        private void onContinueClicked(object sender, EventArgs e)
        {
            this.EndLevel(this.NextLevel);
        }

        private void onChildMoved(object sender, Vector2 e)
        {
            if (!(sender is Node senderNode))
            {
                throw new ArgumentException("sender must be a Node");
            }

            var sourceCollisionAreas = senderNode.GetCollisionAreas();

            foreach (var child in Children)
            {
                if (child == sender)
                {
                    continue;
                }

                var targetCollisionAreas = child.GetCollisionAreas();
                testForCollisions(sourceCollisionAreas, targetCollisionAreas);
            }
        }

        private void onBonusEnemyTimerTick(object sender, EventArgs e)
        {
            if (this.RemainingBonusEnemies > 0)
            {
                this.spawnBonusEnemy();
            }
        }

        private void onBonusEnemyRemoved(object sender, EventArgs e)
        {
            this.bonusShipTimer.Duration = BonusEnemyRandom.NextDouble(MinBonusEnemyInterval, MaxBonusEnemyInterval);
            this.bonusShipTimer.Start();
        }

        #endregion
    }
}