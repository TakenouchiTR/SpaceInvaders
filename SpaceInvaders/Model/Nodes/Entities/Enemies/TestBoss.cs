using System;
using Windows.Devices.PointOfService;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public class TestBoss : Enemy
    {
        private Vector2 velocity;
        private int health;
        public TestBoss() : base(new TestBossSprite())
        {
            Collision.Monitoring = false;
            this.velocity = new Vector2(100, 0);
            this.health = 3;

            this.createTargets();
        }

        private void createTargets()
        {
            AttachChild(new TestBossTarget() {
                Center = new Vector2(this.Left, this.Bottom)
            });
            AttachChild(new TestBossTarget() {
                Center = new Vector2(this.Center.X, this.Bottom)
            });
            AttachChild(new TestBossTarget()
            {
                Center = new Vector2(this.Right, this.Bottom)
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
                this.QueueForRemoval();
            }
        }

        public override void Update(double delta)
        {
            if (this.Left <= 0)
            {
                this.velocity.X = 100;
            }
            else if (this.Right >= MainPage.ApplicationWidth)
            {
                this.velocity.X = -100;
            }

            this.Move(this.velocity * delta);
            base.Update(delta);
        }
    }
}
