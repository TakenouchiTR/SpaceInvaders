using System;
using Windows.UI.Xaml;
using SpaceInvaders.Model.Entities.Effects;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    /// <summary>
    ///     Defines the basic properties of all Enemy objects
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.GameObject" />
    public abstract class Enemy : GameObject
    {
        #region Types and Delegates

        public delegate void MovementTickHandler(Vector2 moveDistance);

        #endregion

        #region Data members

        private const int TotalMovementSteps = 19;
        private const int XMoveAmount = 15;
        private const int YMoveAmount = 32;

        private static readonly DispatcherTimer moveTimer;

        private static int curMovementStep = 10;
        private static int movementDirection = 1;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets how many points is awarded when the enemy is killed.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; protected set; }

        #endregion

        #region Constructors

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
            Monitorable = true;
            CollisionLayers = (int) PhysicsLayer.Enemy;
            CollisionMasks = (int) PhysicsLayer.PlayerHitbox;
            MovementTick += this.OnMovementTick;
            this.Removed += this.onRemoved;
        }

        private void onRemoved(object sender, EventArgs e)
        {
            Explosion explosion = new Explosion(gameManager) 
            {
                Center = this.Center
            };
            gameManager.QueueGameObjectForAddition(explosion);
        }

        #endregion

        #region Methods

        public static event MovementTickHandler MovementTick;

        public override void HandleCollision(GameObject target)
        {
            base.HandleCollision(target);

            this.QueueRemoval();
        }

        protected virtual void OnMovementTick(Vector2 moveDistance)
        {
            Move(moveDistance);
        }

        private static void onMoveTimerTick(object sender, object e)
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

            MovementTick?.Invoke(moveDistance);
        }

        #endregion
    }
}