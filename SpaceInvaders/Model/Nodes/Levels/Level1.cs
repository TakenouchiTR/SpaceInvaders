using System;
using System.Collections.Generic;
using Windows.System;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

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

        private const int EnemiesPerRow = 4;
        private const double EnemySpawnAreaWidth = 250;
        private const double EnemyVerticalGap = 64;
        private const int StarCount = 50;

        private const int TotalMovementSteps = 19;
        private const int XMoveAmount = 15;
        private const int YMoveAmount = 32;

        private const VirtualKey ToggleStarsKey = VirtualKey.S;

        private static int curMovementStep = 9;
        private static int movementDirection = 1;

        private int enemiesRemaining;
        private bool togglePressedLastFrame;

        private Node2D enemyGroup;
        private Node starNode;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level1" /> class.
        /// </summary>
        public Level1()
        {
            this.addBackground();
            this.addPlayer();
            this.addEnemyHelperNodes();
            this.addEnemies();
        }

        #endregion

        #region Methods

        private void addPlayer()
        {
            var player = new PlayerShip();
            player.X = MainPage.ApplicationWidth / 2 - player.Collision.Width / 2;
            player.Y = MainPage.ApplicationHeight - 64;
            AttachChild(player);

            player.Removed += this.onPlayerRemoved;
        }

        private void addEnemyHelperNodes()
        {
            var enemyMoveTimer = new Timer();
            enemyMoveTimer.Start();
            enemyMoveTimer.Tick += this.onEnemyMoveTimerTick;
            AttachChild(enemyMoveTimer);

            this.enemyGroup = new Node2D();
            AttachChild(this.enemyGroup);
        }

        private void addEnemies()
        {
            var spawnCellWidth = EnemySpawnAreaWidth / EnemiesPerRow;
            var startX = spawnCellWidth / 2;
            var startY = 64;

            var enemyList = new List<Enemy> {
                new AggresiveEnemy(),
                new AggresiveEnemy(),
                new AggresiveEnemy(),
                new AggresiveEnemy(),
                new IntermediateEnemy(),
                new IntermediateEnemy(),
                new IntermediateEnemy(),
                new IntermediateEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy()
            };

            for (var i = 0; i < enemyList.Count; i++)
            {
                var enemy = enemyList[i];
                var enemyCenter = new Vector2 {
                    X = startX + i % EnemiesPerRow * spawnCellWidth,
                    // ReSharper disable once PossibleLossOfFraction
                    Y = startY + i / EnemiesPerRow * EnemyVerticalGap
                };
                enemy.Center = enemyCenter;
                enemy.Removed += this.onEnemyRemoved;
                this.enemyGroup.AttachChild(enemy);
            }

            this.enemyGroup.X = MainPage.ApplicationWidth / 2 - EnemySpawnAreaWidth / 2;

            this.enemiesRemaining += enemyList.Count;
        }

        private void addBackground()
        {
            this.starNode = new Node();
            var starRandom = new Random();

            for (var i = 0; i < StarCount / 2; ++i)
            {
                var star = new BackgroundStar {
                    Y = starRandom.NextDouble() * MainPage.ApplicationHeight
                };
                this.starNode.AttachChild(star);
            }

            //Must create planet between stars so that stars will go both behind and in front of the planet.
            //This must be done since ZIndex is not a property of UserControls, so the draw order cannot be easily set.
            this.starNode.AttachChild(new BackgroundPlanet());

            for (var i = StarCount / 2; i < StarCount; ++i)
            {
                var star = new BackgroundStar {
                    Y = starRandom.NextDouble() * MainPage.ApplicationHeight
                };
                this.starNode.AttachChild(star);
            }

            AttachChild(this.starNode);
        }

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
            foreach (var child in this.starNode.Children)
            {
                if (child is SpriteNode star)
                {
                    star.Visible = !star.Visible;
                }
            }
        }

        private void onPlayerRemoved(object sender, EventArgs e)
        {
            CompleteGame("You have been destroyed!");
        }

        private void onEnemyRemoved(object sender, EventArgs e)
        {
            if (sender is Enemy enemy)
            {
                Score += enemy.Score;
                this.enemiesRemaining--;
                if (this.enemiesRemaining <= 0)
                {
                    CompleteGame("All enemies have been defeated!");
                }
            }
        }

        private void onEnemyMoveTimerTick(object sender, EventArgs e)
        {
            var moveDistance = new Vector2();

            curMovementStep += movementDirection;

            if (curMovementStep > TotalMovementSteps || curMovementStep < 0)
            {
                movementDirection *= -1;
                moveDistance.Y = YMoveAmount;
            }
            else
            {
                moveDistance.X = XMoveAmount * movementDirection;
            }

            this.enemyGroup.Move(moveDistance);
        }

        #endregion
    }
}