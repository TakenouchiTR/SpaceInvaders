using System;
using System.Collections.Generic;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     Manages the positions for a group of enemies by uniformly placing them into the center of a grid of cells.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node2D" />
    public class EnemyGroup : Node2D
    {
        #region Data members

        private Vector2 cellSize;
        private int totalEnemiesAdded;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public double Width => this.EnemiesPerRow * this.cellSize.X;

        /// <summary>
        ///     Gets or sets the enemies per row.
        /// </summary>
        /// <value>
        ///     The enemies per row.
        /// </value>
        public int EnemiesPerRow { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyGroup" /> class.<br />
        ///     Precondition: enemiesPerRow $gt; 0
        ///     Postcondition: this.EnemiesPerRow == enemiesPerRow &amp;&amp;<br />
        ///     this.Width == enemiesPerRow * cellSize.X
        /// </summary>
        /// <param name="cellSize">Size of each cell.</param>
        /// <param name="enemiesPerRow">The enemies per row.</param>
        /// <exception cref="System.ArgumentException">enemiesPerRow must be positive</exception>
        public EnemyGroup(Vector2 cellSize, int enemiesPerRow)
        {
            if (enemiesPerRow <= 0)
            {
                throw new ArgumentException("enemiesPerRow must be positive");
            }

            this.cellSize = cellSize;
            this.EnemiesPerRow = enemiesPerRow;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds an enemy.<br />
        ///     Null objects represent a blank space and cause gaps in enemy placement.<br />
        ///     Precondition: None<br />
        ///     PostCondition: if (enemy != null), this.Children.Contains(enemy)
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        public void AddEnemy(Enemy enemy)
        {
            if (enemy != null)
            {
                QueueNodeForAddition(enemy);

                var xPos = this.totalEnemiesAdded % this.EnemiesPerRow * this.cellSize.X;
                // ReSharper disable once PossibleLossOfFraction
                var yPos = this.totalEnemiesAdded / this.EnemiesPerRow * this.cellSize.Y;

                enemy.Center = new Vector2(xPos, yPos) + this.cellSize / 2 + Position;
            }

            this.totalEnemiesAdded++;
        }

        /// <summary>
        ///     Adds a collection of enemies.<br />
        ///     Null objects represent a blank space and cause gaps in enemy placement.<br />
        ///     Precondition: enemies != null<br />
        ///     Postcondition: All non-null items in enemies are in this.Children
        /// </summary>
        /// <param name="enemies">The enemies.</param>
        public void AddEnemies(IEnumerable<Enemy> enemies)
        {
            if (enemies == null)
            {
                throw new ArgumentNullException(nameof(enemies));
            }

            foreach (var enemy in enemies)
            {
                this.AddEnemy(enemy);
            }
        }

        /// <summary>
        ///     Moves the enemies.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="distance">The distance.</param>
        public void MoveEnemies(Vector2 distance)
        {
            foreach (var node in Children)
            {
                if (node is Enemy enemy)
                {
                    enemy.MoveWithGroup(distance);
                }
            }
        }

        #endregion
    }
}