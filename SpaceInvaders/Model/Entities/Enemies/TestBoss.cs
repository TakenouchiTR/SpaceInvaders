using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    public class TestBoss : Enemy
    {
        private int health = 3;
        private Vector2 velocity = new Vector2(75, 0);
        private int direction = 1;

        public TestBoss(GameManager gameManager) : base(gameManager, new TestBossSprite())
        {
            this.CollisionMasks = 0;
            this.CollisionLayers = (int) PhysicsLayer.Enemy;
            this.Score = 1000;
            
            var leftTarget = new TestBossTarget(gameManager) 
            {
                Center = new Vector2(this.X, this.Bottom)
            };
            var rightTarget = new TestBossTarget(gameManager)
            {
                Center = new Vector2(this.Right, this.Bottom)
            };
            var centerTarget = new TestBossTarget(gameManager)
            {
                Center = new Vector2(this.Center.X, this.Bottom)
            };

            this.AddChild(leftTarget);
            this.AddChild(centerTarget);
            this.AddChild(rightTarget);

            gameManager.QueueGameObjectForAddition(leftTarget);
            gameManager.QueueGameObjectForAddition(centerTarget);
            gameManager.QueueGameObjectForAddition(rightTarget);

            leftTarget.Removed += onTargetRemoved;
            centerTarget.Removed += onTargetRemoved;
            rightTarget.Removed += onTargetRemoved;
        }

        private void onTargetRemoved(object sender, EventArgs e)
        {
            this.health -= 1;
            this.velocity.X += Math.Sign(this.velocity.X) * 75;
            if (this.health <= 0)
            {
                this.QueueRemoval();
            }

            if (sender is GameObject child)
            {
                child.Removed -= this.onTargetRemoved;
            }
        }

        public override void Update(double delta)
        {
            Move(this.velocity * delta);
            if (this.X < 0 || this.Right > gameManager.ScreenWidth)
            {
                this.velocity *= -1;
            }
        }

        protected override void OnMovementTick(Vector2 moveDistance)
        {

        }
    }
}
