using System;
using System.Collections.Generic;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     The weak points for the test boss
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class TestBossTarget : Entity
    {
        #region Data members

        private int health;
        private AnimationStateMachine stateMachine;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestBossTarget" /> class.
        /// </summary>
        public TestBossTarget() : base(new BasicEnemySprite())
        {
            this.health = 3;

            Collision.CollisionLayers = PhysicsLayer.Enemy;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            Collision.Collided += this.onCollided;
            Collision.Monitoring = true;
            Collision.Monitorable = true;

            Sprite.Visible = false;

            this.setupTimer();
            this.addAnimations();
        }

        #endregion

        #region Methods

        private void setupTimer()
        {
            var timer = new Timer {
                Duration = 2
            };
            timer.Start();
            timer.Tick += this.onShootTimerTick;
            AttachChild(timer);
        }

        private void addAnimations()
        {
            var animation1 = new AnimatedSprite(1, new List<BaseSprite> {
                new BasicEnemySprite(),
                new IntermediateEnemySprite()
            });
            var animation2 = new AnimatedSprite(new List<AnimationFrame> {
                new AnimationFrame(new AggresiveEnemySprite(), .2),
                new AnimationFrame(new PlayerShipSprite(), .8)
            });
            
            this.stateMachine = new AnimationStateMachine();
            this.stateMachine.AddAnimation("normal", animation1);
            this.stateMachine.AddAnimation("damaged", animation2);

            AttachChild(animation1);
            AttachChild(animation2);
            AttachChild(this.stateMachine);

            this.stateMachine.ChangeAnimation("normal");
        }

        private void onShootTimerTick(object sender, EventArgs e)
        {
            var bullet = new EnemyBullet {
                Position = Center
            };

            GetRoot().QueueGameObjectForAddition(bullet);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            this.health--;

            this.stateMachine.ChangeAnimation("damaged");

            if (this.health <= 0)
            {
                QueueForRemoval();
            }
        }

        #endregion
    }
}