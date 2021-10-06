using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Levels
{
    public class Level1 : LevelBase
    {
        private const int EnemiesPerRow = 4;
        private const double EnemySpawnAreaWidth = 250;
        private const double EnemyVerticalGap = 64;

        private const int TotalMovementSteps = 19;
        private const int XMoveAmount = 15;
        private const int YMoveAmount = 32;
        
        private static int curMovementStep = 9;
        private static int movementDirection = 1;

        private int enemiesRemaining;

        private Node2D enemies;

        public Level1() : base()
        {
            this.addPlayer();
            this.addEnemyHelperNodes();
            this.addEnemies();
        }

        private void addPlayer()
        {
            PlayerShip player = new PlayerShip();
            player.X = MainPage.ApplicationWidth / 2 - player.Collision.Width / 2;
            player.Y = MainPage.ApplicationHeight - 64;
            AttachChild(player);

            player.Removed += onPlayerRemoved;
        }
        
        private void addEnemyHelperNodes()
        {
            Timer enemyMoveTimer = new Timer();
            enemyMoveTimer.Start();
            enemyMoveTimer.Tick += this.onEnemyMoveTimerTick;
            AttachChild(enemyMoveTimer);

            this.enemies = new Node2D();
            this.AttachChild(enemies);
        }

        private void addEnemies()
        {
            var spawnCellWidth = EnemySpawnAreaWidth / EnemiesPerRow;
            var startX = spawnCellWidth / 2;
            var startY = 64;

            List<Enemy> enemyList = new List<Enemy> 
            {
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
                new BasicEnemy(),
            };

            for (var i = 0; i < enemyList.Count; i++)
            {
                var enemy = enemyList[i];
                var enemyCenter = new Vector2 {
                    X = startX + (i % EnemiesPerRow) * spawnCellWidth,
                    // ReSharper disable once PossibleLossOfFraction
                    Y = startY + i / EnemiesPerRow * EnemyVerticalGap
                };
                enemy.Center = enemyCenter;
                enemy.Removed += this.onEnemyRemoved;
                this.enemies.AttachChild(enemy);
            }

            this.enemies.X = MainPage.ApplicationWidth / 2 - EnemySpawnAreaWidth / 2;

            this.enemiesRemaining += enemyList.Count;
        }

        private void onPlayerRemoved(object sender, EventArgs e)
        {
            CompleteGame("You have been destroyed!");
        }

        private void onEnemyRemoved(object sender, EventArgs e)
        {
            if (sender is Enemy enemy)
            {
                this.Score += enemy.Score;
                this.enemiesRemaining--;
                if (this.enemiesRemaining <= 0)
                {
                    this.CompleteGame("All enemies have been defeated!");
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

            this.enemies.Move(moveDistance);
        }
    }
}
