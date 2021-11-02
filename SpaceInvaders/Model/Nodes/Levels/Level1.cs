using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.Model.Nodes.UI;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites.Entities;
using SpaceInvaders.View.Sprites.UI;

namespace SpaceInvaders.Model.Nodes.Levels
{
    /// <summary>
    ///     The first level of the game.<br />
    ///     Has three layers of four enemies in descending difficulty.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Levels.LevelBase" />
    public class Level1 : LevelBase
    {
        #region Data members

        private const int StarCount = 50;
        private const int TotalMovementSteps = 19;
        private const int XMoveAmount = 10;
        private const double UiBuffer = 4;
        private const VirtualKey ToggleStarsKey = VirtualKey.S;

        private int curMovementStep = 9;
        private int movementFactor = 1;
        private int enemiesRemaining;
        private bool togglePressedLastFrame;

        private PlayerShip player;
        private EnemyGroup enemyGroup;
        private Node backgroundNode;
        private Label scoreLabel;
        private LifeCounter lifeCounter;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level1" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 4
        /// </summary>
        public Level1()
        {
            this.addBackground();
            this.addPlayer();
            this.addEnemyHelperNodes();
            this.addEnemies();
            this.addUi();
        }

        #endregion

        #region Methods

        private void addPlayer()
        {
            this.player = new PlayerShip();
            this.player.X = MainPage.ApplicationWidth / 2 - this.player.Collision.Width / 2;
            this.player.Y = MainPage.ApplicationHeight - 64;

            AttachChild(this.player);

            this.player.Removed += this.onPlayerRemoved;
        }

        private void addEnemyHelperNodes()
        {
            var enemyMoveTimer = new Timer();
            enemyMoveTimer.Start();
            enemyMoveTimer.Tick += this.onEnemyMoveTimerTick;
            AttachChild(enemyMoveTimer);

            this.enemyGroup = new EnemyGroup(new Vector2(55, 64), 8);
            AttachChild(this.enemyGroup);
        }

        private void addEnemies()
        {
            this.enemyGroup.X = MainPage.ApplicationWidth / 2 - this.enemyGroup.Width / 2;
            this.enemyGroup.Y = 24;

            var enemyOrder = this.createEnemyOrder().ToArray();
            var enemies = enemyOrder.Where(enemy => enemy != null).ToArray();

            this.enemyGroup.AddEnemies(enemyOrder);

            foreach (var enemy in enemies)
            {
                enemy.Removed += this.onEnemyRemoved;
            }

            this.enemiesRemaining += enemies.Count();
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

        private void updateScoreDisplay()
        {
            this.scoreLabel.Text = $"Score: {this.Score}";
        }

        private IEnumerable<Enemy> createEnemyOrder()
        {
            Type[] classOrder = {
                typeof(MasterEnemy),
                typeof(AggresiveEnemy),
                typeof(IntermediateEnemy),
                typeof(BasicEnemy)
            };
            var enemyOrder = new List<Enemy>();
            var enemiesPerRow = this.enemyGroup.EnemiesPerRow;

            for (var i = 0; i < classOrder.Length && i * 2 < enemiesPerRow; i++)
            {
                var currentType = classOrder[i];

                for (var j = 0; j < i; j++)
                {
                    enemyOrder.Add(null);
                }

                for (var j = 0; j < enemiesPerRow - 2 * i; j++)
                {
                    //Gets the default constructor for the Type, then invokes it to create a new enemy
                    var constructor = currentType.GetConstructors()[0];
                    var enemy = constructor.Invoke(new object[] { });
                    enemyOrder.Add((Enemy) enemy);
                }

                for (var j = 0; j < i; j++)
                {
                    enemyOrder.Add(null);
                }
            }

            return enemyOrder;
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

            AttachChild(this.backgroundNode);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (Input.IsKeyPressed(ToggleStarsKey) && !this.togglePressedLastFrame)
            {
                this.toggleStarVisibility();
            }

            this.togglePressedLastFrame = Input.IsKeyPressed(ToggleStarsKey);

            base.Update(delta);
        }

        private void toggleStarVisibility()
        {
            foreach (var child in this.backgroundNode.Children)
            {
                if (child is SpriteNode star)
                {
                    star.Visible = !star.Visible;
                }
            }
        }

        private void onPlayerLivesChanged(object sender, int e)
        {
            this.lifeCounter.CurrentLives = e;
        }

        private void onPlayerRemoved(object sender, EventArgs e)
        {
            CompleteGame("Game Over!\nYou have been destroyed!");
        }

        private void onEnemyRemoved(object sender, EventArgs e)
        {
            if (sender is Enemy enemy)
            {
                Score += enemy.Score;

                this.updateScoreDisplay();

                this.enemiesRemaining--;
                if (this.enemiesRemaining <= 0)
                {
                    CompleteGame("You won!\nAll enemies have been defeated!");
                }
            }
        }

        private void onEnemyMoveTimerTick(object sender, EventArgs e)
        {
            this.curMovementStep += this.movementFactor;

            if (this.curMovementStep >= TotalMovementSteps || this.curMovementStep <= 0)
            {
                this.movementFactor *= -1;
            }
            
            this.enemyGroup.X += XMoveAmount * this.movementFactor;
        }

        #endregion
    }
}