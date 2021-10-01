using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    public class TestBoss : Enemy
    {
        #region Data members

        private int health = 3;
        private Vector2 velocity = new Vector2(75, 0);
        private int direction = 1;

        #endregion

        #region Constructors

        public TestBoss(GameManager gameManager) : base(gameManager, new TestBossSprite())
        {
            CollisionMasks = 0;
            CollisionLayers = (int) PhysicsLayer.Enemy;
            Score = 1000;

            var leftTarget = new TestBossTarget(gameManager) {
                Center = new Vector2(X, Bottom)
            };
            var rightTarget = new TestBossTarget(gameManager) {
                Center = new Vector2(Right, Bottom)
            };
            var centerTarget = new TestBossTarget(gameManager) {
                Center = new Vector2(Center.X, Bottom)
            };

            AddChild(leftTarget);
            AddChild(centerTarget);
            AddChild(rightTarget);

            gameManager.QueueGameObjectForAddition(leftTarget);
            gameManager.QueueGameObjectForAddition(centerTarget);
            gameManager.QueueGameObjectForAddition(rightTarget);

            leftTarget.Removed += this.onTargetRemoved;
            centerTarget.Removed += this.onTargetRemoved;
            rightTarget.Removed += this.onTargetRemoved;
        }

        #endregion

        #region Methods

        private void onTargetRemoved(object sender, EventArgs e)
        {
            this.health -= 1;
            this.velocity.X += Math.Sign(this.velocity.X) * 75;
            if (this.health <= 0)
            {
                QueueRemoval();
            }

            if (sender is GameObject child)
            {
                child.Removed -= this.onTargetRemoved;
            }
        }

        public override void Update(double delta)
        {
            Move(this.velocity * delta);
            if (X < 0 || Right > gameManager.ScreenWidth)
            {
                this.velocity *= -1;
            }
        }

        protected override void OnMovementTick(Vector2 moveDistance)
        {
        }

        #endregion
    }
}