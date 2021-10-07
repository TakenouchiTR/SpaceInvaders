using System;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class TestBoss : Enemy
    {
        #region Data members

        private Vector2 velocity;
        private int health;

        #endregion

        #region Constructors

        public TestBoss() : base(new TestBossSprite())
        {
            Collision.Monitoring = false;
            this.velocity = new Vector2(100, 0);
            this.health = 3;

            this.createTargets();
        }

        #endregion

        #region Methods

        private void createTargets()
        {
            AttachChild(new TestBossTarget {
                Center = new Vector2(Left, Bottom)
            });
            AttachChild(new TestBossTarget {
                Center = new Vector2(Center.X, Bottom)
            });
            AttachChild(new TestBossTarget {
                Center = new Vector2(Right, Bottom)
            });

            foreach (var child in children)
            {
                child.Removed += this.onTargetRemoved;
            }
        }

        private void onTargetRemoved(object sender, EventArgs e)
        {
            this.health--;
            if (this.health <= 0)
            {
                QueueForRemoval();
            }
        }

        public override void Update(double delta)
        {
            if (Left <= 0)
            {
                this.velocity.X = 100;
            }
            else if (Right >= MainPage.ApplicationWidth)
            {
                this.velocity.X = -100;
            }

            Move(this.velocity * delta);
            base.Update(delta);
        }

        #endregion
    }
}