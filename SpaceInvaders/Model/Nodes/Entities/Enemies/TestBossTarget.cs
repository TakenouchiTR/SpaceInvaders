using System;
using System.Collections.Generic;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     The weak points for the test boss
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class TestBossTarget : Entity
    {
        #region Data members

        private const double Speed = 50;

        private int health;
        private double movementFactor;
        private AnimationStateMachine stateMachine;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the direction and speed of movement.<br />
        ///     The sprite's render layer is updated to reflect the movement.
        /// </summary>
        /// <value>
        ///     The movement factor.
        /// </value>
        public double MovementFactor
        {
            get => this.movementFactor;
            set
            {
                this.movementFactor = value;
                if (this.movementFactor >= 0)
                {
                    this.stateMachine.SetRenderLayer(RenderLayer.MainUpperMiddle);
                }
                else
                {
                    this.stateMachine.SetRenderLayer(RenderLayer.MainLowerMiddle);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestBossTarget" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Collision.CollisionLayers == PhysicsLayer.Enemy &amp;&amp;<br />
        ///     this.Collision.CollisionMasks == this.PhysicsLayer.PlayerHitbox &amp;&amp;<br />
        ///     this.Collision.Monitoring == true &amp;&amp;<br />
        ///     this.Collision.Monitorable == true &amp;&amp;<br />
        ///     this.Sprite is BasicEnemySprite &amp;&amp;<br />
        ///     this.Sprite.Visible == false &amp;&amp;<br />
        ///     this.Children.Count == 5
        /// </summary>
        public TestBossTarget() : base(new BasicEnemySpriteBase())
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

            this.MovementFactor = 1;
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
                new BasicEnemySpriteBase(),
                null
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

        public override void Update(double delta)
        {
            X += Speed * delta * this.MovementFactor;

            if (Parent is TestBoss boss)
            {
                if (Center.X > boss.Right)
                {
                    this.MovementFactor *= -1;
                    X -= 2 * (Center.X - boss.Right);
                }
                else if (Center.X < boss.Left)
                {
                    this.MovementFactor *= -1;
                    X += 2 * (boss.Left - Center.X);
                }
            }

            base.Update(delta);
        }

        private void onShootTimerTick(object sender, EventArgs e)
        {
            var bullet = new EnemyBullet {
                Position = Center
            };

            GetRoot().QueueNodeForAddition(bullet);
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