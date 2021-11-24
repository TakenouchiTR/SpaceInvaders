﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Screens.Levels
{
    /// <summary>
    ///     The first level of the game.<br />
    ///     Has three layers of four enemies in descending difficulty.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screens.Levels.LevelBase" />
    public class Level1 : LevelBase
    {
        #region Data members

        private const int TotalMovementSteps = 20;
        private const int XMoveAmount = 20;

        private int curMovementStep;
        private int movementFactor;

        private EnemyGroup enemyGroup;

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
                RegisterEnemy(enemy);
            }
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