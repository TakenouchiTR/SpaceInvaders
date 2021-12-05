using System;
using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Screens.Levels
{
    /// <summary>
    ///     The third level of the game.<br />
    ///     Four layers of enemies that slowly approach the player.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screens.Levels.LevelBase" />
    public class Level3 : LevelBase
    {
        #region Data members

        private const int MoveAmount = 20;
        private const double EdgeDetectorWidth = 48;
        private const int KillsPerSpeedUp = 10;
        private const double SpeedUpFactor = .5;

        private int movementFactor;
        private bool downStep;
        private int enemyDeaths;
        private EnemyGroup enemyGroup;
        private CollisionArea leftEdgeDetector;
        private CollisionArea rightEdgeDetector;
        private Timer enemyMoveTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level1" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 4
        /// </summary>
        public Level3() : base(typeof(Level4))
        {
            this.movementFactor = -1;

            this.addEnemyHelperNodes();
            this.addEnemies();
            this.setupEdgeDetection();
        }

        #endregion

        #region Methods

        private void addEnemyHelperNodes()
        {
            this.enemyMoveTimer = new Timer();
            this.enemyMoveTimer.Start();

            this.enemyGroup = new EnemyGroup(new Vector2(80, 80), 8);

            this.enemyMoveTimer.Tick += this.onEnemyMoveTimerTick;

            AttachChild(this.enemyMoveTimer);
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
                enemy.Removed += this.onEnemyRemoved;
            }
        }

        private void setupEdgeDetection()
        {
            this.leftEdgeDetector = new CollisionArea(EdgeDetectorWidth, MainPage.ApplicationHeight) {
                Monitoring = true,
                CollisionMasks = PhysicsLayer.Enemy
            };
            this.rightEdgeDetector = new CollisionArea(EdgeDetectorWidth, MainPage.ApplicationHeight) {
                CollisionMasks = PhysicsLayer.Enemy,
                Right = MainPage.ApplicationWidth
            };

            this.leftEdgeDetector.Collided += this.onLeftEdgeCollision;
            this.rightEdgeDetector.Collided += this.onRightEdgeCollision;

            AttachChild(this.leftEdgeDetector);
            AttachChild(this.rightEdgeDetector);
        }

        private IEnumerable<Enemy> createEnemyOrder()
        {
            EnemyType[] classOrder = {
                EnemyType.MasterEnemy,
                EnemyType.AggressiveEnemy,
                EnemyType.IntermediateEnemy,
                EnemyType.BasicEnemy
            };
            var enemyOrder = new List<Enemy>();
            var enemiesPerRow = this.enemyGroup.EnemiesPerRow;

            for (var i = 0; i < classOrder.Length && i * 2 < enemiesPerRow; i++)
            {
                for (var j = 0; j < enemiesPerRow; j++)
                {
                    enemyOrder.Add(Enemy.CreateEnemy(classOrder[i]));
                }
            }

            return enemyOrder;
        }

        private void onEnemyMoveTimerTick(object sender, EventArgs e)
        {
            var moveDistance = new Vector2();
            if (this.downStep)
            {
                moveDistance.Y = MoveAmount;
                this.downStep = false;
            }
            else
            {
                moveDistance.X = MoveAmount * this.movementFactor;
            }

            this.enemyGroup.MoveEnemies(moveDistance);
        }

        private void onLeftEdgeCollision(object sender, CollisionArea e)
        {
            if (e.Parent is BonusEnemy)
            {
                return;
            }

            if (e.Parent is MasterEnemy masterEnemy)
            {
                if (masterEnemy.State != MasterEnemyState.InFormation &&
                    !this.leftEdgeDetector.ContainsPoint(masterEnemy.FormationLocation))
                {
                    return;
                }
            }

            this.movementFactor = 1;
            this.leftEdgeDetector.Monitoring = false;
            this.rightEdgeDetector.Monitoring = true;
            this.downStep = true;
        }

        private void onRightEdgeCollision(object sender, CollisionArea e)
        {
            if (e.Parent is BonusEnemy)
            {
                return;
            }

            if (e.Parent is MasterEnemy masterEnemy)
            {
                if (masterEnemy.State != MasterEnemyState.InFormation &&
                    !this.leftEdgeDetector.ContainsPoint(masterEnemy.FormationLocation))
                {
                    return;
                }
            }

            this.movementFactor = -1;
            this.leftEdgeDetector.Monitoring = true;
            this.rightEdgeDetector.Monitoring = false;
            this.downStep = true;
        }

        private void onEnemyRemoved(object sender, EventArgs e)
        {
            this.enemyDeaths++;
            if (this.enemyDeaths % KillsPerSpeedUp == 0)
            {
                this.enemyMoveTimer.Duration *= SpeedUpFactor;
            }
        }

        #endregion
    }
}