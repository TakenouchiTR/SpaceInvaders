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

        private Node2D enemies;

        public Level1() : base()
        {
            this.addPlayer();
            this.addEnemyHelperNodes();
            this.addEnemies();
        }

        private void addPlayer()
        {
            Entity sprite = new PlayerShip();
            sprite.X = 200;
            sprite.Y = MainPage.ApplicationHeight - 64;
            AttachChild(sprite);
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
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
                new BasicEnemy(),
            };

            for (var i = 0; i < enemyList.Count; i++)
            {
                Enemy enemy = enemyList[i];
                enemy.X = startX + (i % EnemiesPerRow) * spawnCellWidth;
                enemy.Y = startY + (i / EnemiesPerRow) * EnemyVerticalGap;
                enemy.Removed += this.onEnemyRemoved;
                this.enemies.AttachChild(enemy);
            }

            this.enemies.X = MainPage.ApplicationWidth / 2 - EnemySpawnAreaWidth / 2;
        }
        
        private void onEnemyRemoved(object sender, EventArgs e)
        {
            if (sender is Enemy enemy)
            {
                this.Score += enemy.Score;
            }
        }

        private void onEnemyMoveTimerTick(object sender, EventArgs e)
        {
            this.enemies.X += 10;
        }
    }
}
