using System;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     A boss used for testing the tree structure on moving nodes
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class TestBoss : Enemy
    {
        #region Data members

        private Vector2 velocity;
        private int health;
        private double speed;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestBoss" /> class.
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 3 &amp;&amp;<br />
        ///     this.Score == 1000 &amp;&amp;<br />
        ///     this.Sprite is TestBossSprite &amp;&amp;
        ///     this.Collision.Monitoring == false
        /// </summary>
        public TestBoss() : base(new TestBossSprite())
        {
            Collision.Monitoring = false;
            this.speed = 100;
            this.velocity = new Vector2(this.speed, 0);
            this.health = 3;

            this.createTargets();
        }

        #endregion

        #region Methods

        private void createTargets()
        {
            AttachChild(new TestBossTarget {
                Center = new Vector2(Left + Width / 6, Bottom)
            });
            AttachChild(new TestBossTarget {
                Center = new Vector2(Right - Width / 6, Bottom)
            });
            AttachChild(new TestBossTarget {
                Center = new Vector2(Center.X, Bottom),
                MovementFactor = -1
            });

            foreach (var child in Children)
            {
                child.Removed += this.onTargetRemoved;
            }
        }

        private void onTargetRemoved(object sender, EventArgs e)
        {
            this.health--;
            this.speed += 50;
            this.velocity.X = Math.Sign(this.velocity.X) * this.speed;
            if (this.health <= 0)
            {
                QueueForRemoval();
            }
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (Left <= 0)
            {
                this.velocity.X = this.speed;
            }
            else if (Right >= MainPage.ApplicationWidth)
            {
                this.velocity.X = -this.speed;
            }

            Move(this.velocity * delta);
            base.Update(delta);
        }

        #endregion
    }
}