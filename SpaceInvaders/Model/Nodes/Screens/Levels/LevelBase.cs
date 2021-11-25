﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Nodes.Effects;
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
        ///     The background star count.
        /// </summary>
        protected const int StarCount = 50;

        /// <summary>
        ///     The buffer between the edge of the screen and the UI, in pixels.
        /// </summary>
        protected const double UiBuffer = 4;

        private const double MillisecondsInSecond = 1000;
        private const double UpdateSkipThreshold = 1;

        private const int MaxGrazePoints = 150;
        private const int MaxTimeBonus = 500;
        private const int PointsPerLife = 100;
        private const int PointsLostPerSecond = 15;
        private const int PointsPerShieldSegment = 2;

        private readonly DispatcherTimer updateTimer;

        private long prevUpdateTime;
        private int enemiesRemaining;
        private double timeInLevel;
        private bool levelComplete;
        private bool canEarnPoints;
        private Type nextScreen;
        private Dictionary<PointSource, int> scoreBreakdown;

        private PlayerShip player;
        private Node backgroundNode;
        private Node shieldNode;
        private Label scoreLabel;
        private LifeCounter lifeCounter;

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

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelBase" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        protected LevelBase(Type nextLevel)
        {
            this.NextLevel = nextLevel;
            this.SpeedModifier = 1;
            this.canEarnPoints = true;

            this.updateTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(.1)
            };
            this.updateTimer.Tick += this.onUpdateTimerTick;
            this.updateTimer.Start();

            this.prevUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            this.addPlayer();
            this.addUi();
            this.addShields();
            this.addBackground();
            this.setupScoreBreakdown();
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

            this.AttachChild(this.scoreLabel);
            this.AttachChild(this.lifeCounter);
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

        private void addBackground()
        {
            this.backgroundNode = new Node();
            var starRandom = new Random();

            for (var i = 0; i < StarCount; ++i)
            {
                var star = new BackgroundStar {
                    Y = starRandom.NextDouble() * MainPage.ApplicationHeight
                };
                this.backgroundNode.AttachChild(star);
            }

            this.backgroundNode.AttachChild(new BackgroundPlanet());

            this.AttachChild(this.backgroundNode);
        }

        private void setupScoreBreakdown()
        {
            this.scoreBreakdown = new Dictionary<PointSource, int>();
            var sources = (PointSource[]) Enum.GetValues(typeof(PointSource));

            foreach (var source in sources)
            {
                this.scoreBreakdown[source] = 0;
            }
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
        }

        private void updateScoreDisplay()
        {
            this.scoreLabel.Text = $"Score: {this.Score}";
        }

        private void testForCollisions(IEnumerable<CollisionArea> sourceAreas, IList<CollisionArea> targetAreas)
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
            this.updateTimer.Stop();
            this.updateTimer.Tick -= this.onUpdateTimerTick;

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

        private async void onPlayerRemoved(object sender, EventArgs e)
        {
            if (!this.canEarnPoints)
            {
                return;
            }

            var gameOverSound = new OneShotSoundPlayer("game_over.wav");
            this.canEarnPoints = false;
            QueueNodeForAddition(gameOverSound);

            var quitDialog = new ContentDialog {
                Title = "Game Over",
                Content = "You ran out of lives!\nWould you like to restart the level?",
                PrimaryButtonText = "Restart",
                SecondaryButtonText = "Return to Menu"
            };

            var dialogResult = await quitDialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                this.EndLevel(GetType());
            }
            else
            {
                this.EndLevel(typeof(MainMenu));
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

        private void onUpdateTimerTick(object sender, object e)
        {
            var curTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var timeSinceLastTick = curTime - this.prevUpdateTime;
            var delta = timeSinceLastTick / MillisecondsInSecond;
            this.prevUpdateTime = curTime;

            if (delta < UpdateSkipThreshold)
            {
                this.Update(delta);
            }
        }

        private void onChildMoved(object sender, Vector2 e)
        {
            var senderNode = sender as Node;
            if (senderNode == null)
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
                this.testForCollisions(sourceCollisionAreas, targetCollisionAreas);
            }
        }

        #endregion
    }
}