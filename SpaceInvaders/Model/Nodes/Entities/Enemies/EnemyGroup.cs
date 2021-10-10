using System.Collections.Generic;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class EnemyGroup : Node2D
    {
        #region Data members

        private Vector2 cellSize;
        private int totalEnemiesAdded;

        #endregion

        #region Properties

        public double Width => this.EnemiesPerRow * this.cellSize.X;
        public int EnemiesPerRow { get; protected set; }

        #endregion

        #region Constructors

        public EnemyGroup(Vector2 cellSize, int enemiesPerRow)
        {
            this.cellSize = cellSize;
            this.EnemiesPerRow = enemiesPerRow;
        }

        #endregion

        #region Methods

        public void AddEnemy(Enemy enemy)
        {
            QueueGameObjectForAddition(enemy);

            var xPos = this.totalEnemiesAdded % this.EnemiesPerRow * this.cellSize.X;
            // ReSharper disable once PossibleLossOfFraction
            var yPos = this.totalEnemiesAdded / this.EnemiesPerRow * this.cellSize.Y;

            enemy.Center = new Vector2(xPos, yPos) + this.cellSize / 2 + Position;

            this.totalEnemiesAdded++;
        }

        public void AddEnemies(IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                this.AddEnemy(enemy);
            }
        }

        #endregion
    }
}