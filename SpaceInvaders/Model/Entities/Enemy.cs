using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     Defines the basic properties of all Enemy objects
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.GameObject" />
    public abstract class Enemy : GameObject
    {
        public delegate void MovementTickHandler(Vector2 moveDistance);

        public static event MovementTickHandler MovementTick;
        
        private const int TotalMovementSteps = 20;
        private const int XMoveAmount = 15;
        private const int YMoveAmount = 32;

        private static readonly DispatcherTimer moveTimer;

        private static int curMovementStep = 10;
        private static int movementDirection = 1;

        /// <summary>
        ///     Gets or sets how many points is awarded when the enemy is killed.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; protected set; }

        static Enemy()
        {
            moveTimer = new DispatcherTimer();
            moveTimer.Interval = TimeSpan.FromSeconds(1);
            moveTimer.Tick += onMoveTimerTick;
            moveTimer.Start();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Enemy" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="sprite">The sprite.</param>
        protected Enemy(GameManager parent, BaseSprite sprite) : base(parent, sprite)
        {
            Monitorable = true;
            CollisionLayers = (int) PhysicsLayer.Enemy;
            MovementTick += this.OnMovementTick;
        }

        #endregion

        #region Methods

        public static event MovementTickHandler MovementTick;

        protected virtual void OnMovementTick(Vector2 moveDistance)
        {
            Move(moveDistance);
        }

        private static void onMoveTimerTick(object sender, object e)
        {
            Vector2 moveDistance = new Vector2();

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
            
            MovementTick?.Invoke(moveDistance);
        }

    }
}
