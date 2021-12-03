using System;
using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Screens.Levels
{
    /// <summary>
    ///     The first level of the game.<br />
    ///     Has two blocks of enemies that move back and forth.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screens.Levels.LevelBase" />
    public class Level1 : LevelBase
    {
        #region Data members

        private const int ColumnsPerBlock = 3;
        private const double EnemyGroupYLocation = 32;

        private const double CellSize = 70;
        private const double XMoveAmount = 32;

        private int movementFactor;

        private EnemyGroup leftEnemyGroup;
        private EnemyGroup rightEnemyGroup;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level1" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 4
        /// </summary>
        public Level1() : base(typeof(Level2))
        {
            this.movementFactor = -1;

            this.addEnemyHelperNodes();
            this.addEnemies(this.leftEnemyGroup);
            this.addEnemies(this.rightEnemyGroup);
        }

        #endregion

        #region Methods

        private void addEnemyHelperNodes()
        {
            var enemyMoveTimer = new Timer();
            enemyMoveTimer.Start();
            enemyMoveTimer.Tick += this.onEnemyMoveTimerTick;

            this.leftEnemyGroup = new EnemyGroup(new Vector2(CellSize, CellSize), ColumnsPerBlock);
            this.rightEnemyGroup = new EnemyGroup(new Vector2(CellSize, CellSize), ColumnsPerBlock);

            this.leftEnemyGroup.X = MainPage.ApplicationWidth / 3 - this.leftEnemyGroup.Width / 2 + CellSize / 1.5;
            this.rightEnemyGroup.X = MainPage.ApplicationWidth * 2 / 3 - this.rightEnemyGroup.Width / 2 - CellSize / 1.5;

            this.leftEnemyGroup.Y = EnemyGroupYLocation;
            this.rightEnemyGroup.Y = EnemyGroupYLocation;

            AttachChild(enemyMoveTimer);
            AttachChild(this.leftEnemyGroup);
            AttachChild(this.rightEnemyGroup);
        }

        private void addEnemies(EnemyGroup enemyGroup)
        {
            var enemyOrder = this.createEnemyOrder().ToArray();
            var enemies = enemyOrder.Where(enemy => enemy != null).ToArray();

            enemyGroup.AddEnemies(enemyOrder);

            foreach (var enemy in enemies)
            {
                RegisterEnemy(enemy);
            }
        }

        private IEnumerable<Enemy> createEnemyOrder()
        {
            EnemyType[] classOrder = {
                EnemyType.AggressiveEnemy,
                EnemyType.IntermediateEnemy,
                EnemyType.BasicEnemy
            };
            var enemyOrder = new List<Enemy>();

            foreach (var type in classOrder)
            {
                for (var i = 0; i < ColumnsPerBlock; i++)
                {
                    enemyOrder.Add(Enemy.CreateEnemy(type));
                }
            }

            return enemyOrder;
        }

        private void onEnemyMoveTimerTick(object sender, EventArgs e)
        {
            var moveDistance = Vector2.Right * this.movementFactor * XMoveAmount;

            this.leftEnemyGroup.MoveEnemies(moveDistance);
            this.rightEnemyGroup.MoveEnemies(moveDistance * -1);

            this.movementFactor *= -1;
        }

        #endregion
    }
}