using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class EnemyGroup : Node2D
    {
        private Vector2 cellSize;
        private int totalEnemiesAdded;

        public double Width => this.EnemiesPerRow * this.cellSize.X;
        public int EnemiesPerRow { get; protected set; }

        public EnemyGroup(Vector2 cellSize, int enemiesPerRow)
        {
            this.cellSize = cellSize;
            this.EnemiesPerRow = enemiesPerRow;
        }

        public void AddEnemy(Enemy enemy)
        {
            QueueGameObjectForAddition(enemy);

            double xPos = (this.totalEnemiesAdded % this.EnemiesPerRow) * this.cellSize.X;
            // ReSharper disable once PossibleLossOfFraction
            double yPos = (this.totalEnemiesAdded / this.EnemiesPerRow) * this.cellSize.Y;

            enemy.Center = new Vector2(xPos, yPos) + this.cellSize / 2 + this.Position;

            this.totalEnemiesAdded++;
        }

        public void AddEnemies(IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                this.AddEnemy(enemy);
            }
        }
    }
}
