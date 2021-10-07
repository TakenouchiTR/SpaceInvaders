using System;
using System.Numerics;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    public class BackgroundStar : SpriteNode
    {
        private static readonly Random StarRandom = new Random();
        private const double MaxSpeed = 250;
        private const double MinSpeed = 100;

        private double velocity;

        public BackgroundStar() : base(new StarSprite())
        {
            this.setVelocityAndScale();
            this.setStartingPosition();
        }

        private void setVelocityAndScale()
        {
            this.velocity = StarRandom.NextDouble() * (MaxSpeed - MinSpeed) + MinSpeed;
            this.Sprite.Scale = new Vector3((float) (this.velocity / MaxSpeed));
        }

        private void setStartingPosition()
        {
            this.Y = -this.Height;
            this.X = StarRandom.Next((int)(MainPage.ApplicationWidth - this.Width));
        }

        public override void Update(double delta)
        {
            this.Y += this.velocity * delta;
            if (this.IsOffScreen())
            {
                this.setVelocityAndScale();
                this.setStartingPosition();
            }
            base.Update(delta);
        }
    }
}
