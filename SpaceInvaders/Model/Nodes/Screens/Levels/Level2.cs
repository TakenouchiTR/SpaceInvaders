using System;
using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Screens.Levels
{
    /// <summary>
    ///     The second level of the game.<br />
    ///     Has three layers of four enemies in descending difficulty.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screens.Levels.LevelBase" />
    public class Level2 : LevelBase
    {
        #region Data members

        private const int TotalMovementSteps = 20;
        private const double CellSize = 60;
        private const double XMoveAmount = CellSize / 3;

        private int curMovementStep;
        private int movementFactor;

        private EnemyGroup topEnemyGroup;
        private EnemyGroup bottomEnemyGroup;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level1" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 4
        /// </summary>
        public Level2() : base(typeof(Level3))
        {
            this.curMovementStep = 9;
            this.movementFactor = 1;

            this.addEnemyHelperNodes();
            this.addEnemies();
        }

        #endregion

        #region Methods

        private void addEnemyHelperNodes()
        {
            var enemyMoveTimer = new Timer();
            enemyMoveTimer.Start();
            enemyMoveTimer.Tick += this.onEnemyMoveTimerTick;
            AttachChild(enemyMoveTimer);

            this.topEnemyGroup = new EnemyGroup(new Vector2(CellSize, CellSize), 8) 
            {
                Y = 24
            };
            this.topEnemyGroup.X = MainPage.ApplicationWidth / 2 - this.topEnemyGroup.Width / 2;
            this.bottomEnemyGroup = new EnemyGroup(new Vector2(CellSize, CellSize), 8)
            {
                X = this.topEnemyGroup.X,
                Y = this.topEnemyGroup.Y + CellSize
            };
            AttachChild(this.topEnemyGroup);
            AttachChild(this.bottomEnemyGroup);
        }

        private void addEnemies()
        {
            var topEnemyTypes = new [] {
                EnemyType.MasterEnemy,
                EnemyType.IntermediateEnemy
            };
            var bottomEnemyTypes = new [] {
                EnemyType.AggressiveEnemy,
                EnemyType.BasicEnemy
            };

            var topEnemies = this.createEnemyOrder(topEnemyTypes).ToArray();
            var bottomEnemies = this.createEnemyOrder(bottomEnemyTypes).ToArray();

            this.topEnemyGroup.AddEnemies(topEnemies);
            this.bottomEnemyGroup.AddEnemies(bottomEnemies);

            foreach (var enemy in topEnemies.Where(enemy => enemy != null))
            {
                RegisterEnemy(enemy);
            }
            foreach (var enemy in bottomEnemies.Where(enemy => enemy != null))
            {
                RegisterEnemy(enemy);
            }
        }

        private IEnumerable<Enemy> createEnemyOrder(IReadOnlyList<EnemyType> classOrder)
        {
            var enemyOrder = new List<Enemy>();
            var enemiesPerRow = this.topEnemyGroup.EnemiesPerRow;

            foreach (var enemyType in classOrder)
            {
                for (var j = 0; j < enemiesPerRow; j++)
                {
                    enemyOrder.Add(Enemy.CreateEnemy(enemyType));
                }
                for (var j = 0; j < enemiesPerRow; j++)
                {
                    enemyOrder.Add(null);
                }
            }

            return enemyOrder;
        }
        
        private void onEnemyMoveTimerTick(object sender, EventArgs e)
        {
            this.curMovementStep += this.movementFactor;

            if (this.curMovementStep >= TotalMovementSteps || this.curMovementStep <= 0)
            {
                this.movementFactor *= -1;
            }

            this.topEnemyGroup.MoveEnemies(new Vector2(XMoveAmount * this.movementFactor, 0));
            this.bottomEnemyGroup.MoveEnemies(new Vector2(XMoveAmount * this.movementFactor * -1, 0));
        }

        #endregion
    }
}