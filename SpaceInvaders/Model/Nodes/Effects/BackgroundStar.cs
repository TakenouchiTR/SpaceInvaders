using System;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    public class BackgroundStar : SpriteNode
    {
        private static readonly Random StarRandom = new Random();
        private const double MaxSpeed = 250;
        private const double MinSpeed = 100;

        private readonly double velocity;

        public BackgroundStar() : base(new StarSprite())
        {
            this.velocity = StarRandom.NextDouble() * (MaxSpeed - MinSpeed) + MinSpeed;
            this.setStartingPosition();
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
                this.QueueForRemoval();
            }
            base.Update(delta);
        }
    }
}
