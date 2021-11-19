﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.Model.Nodes.UI;
using SpaceInvaders.View;
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

        private const int ShieldCount = 3;
        private const int StarCount = 50;
        private const int TotalMovementSteps = 20;
        private const int XMoveAmount = 20;
        private const double UiBuffer = 4;
        private const double SpeedChangeAmount = .01;
        private const VirtualKey ToggleStarsKey = VirtualKey.S;
        private const VirtualKey SpeedUpKey = VirtualKey.Up;
        private const VirtualKey SpeedDownKey = VirtualKey.Down;

        private int curMovementStep;
        private int movementFactor;
        private double gameSpeed;
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
            this.curMovementStep = 9;
            this.movementFactor = 1;
            this.gameSpeed = 1;

            this.addBackground();
            this.addPlayer();
            this.addEnemyHelperNodes();
            this.addEnemies();
            this.addUi();
            this.addShields();
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

            this.enemyGroup = new EnemyGroup(new Vector2(70, 70), 8);
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

            AttachChild(this.scoreLabel);
            AttachChild(this.lifeCounter);
        }

        private void addShields()
        {
            var y = MainPage.ApplicationHeight - 160;
            for (var shieldIndex = 1; shieldIndex <= ShieldCount; shieldIndex++)
            {
                var x = MainPage.ApplicationWidth / (ShieldCount + 1) * shieldIndex;
                var currentShield = new LevelShield(5, 3) {
                    Center = new Vector2(x, y)
                };

                AttachChild(currentShield);
            }
        }

        private void updateScoreDisplay()
        {
            this.scoreLabel.Text = $"Score: {Score}";
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

                //Pads spaces to the left of the row
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

                //Pads spaces to the right of the row
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
            this.handleInput();
            base.Update(delta * this.gameSpeed);
        }

        private void handleInput()
        {
            if (Input.IsKeyPressed(ToggleStarsKey) && !this.togglePressedLastFrame)
            {
                this.toggleStarVisibility();
            }

            if (Input.IsKeyPressed(SpeedUpKey))
            {
                this.gameSpeed = Math.Min(this.gameSpeed + SpeedChangeAmount, 2);
            }

            if (Input.IsKeyPressed(SpeedDownKey))
            {
                this.gameSpeed = Math.Max(this.gameSpeed - SpeedChangeAmount, .5);
            }

            this.togglePressedLastFrame = Input.IsKeyPressed(ToggleStarsKey);
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