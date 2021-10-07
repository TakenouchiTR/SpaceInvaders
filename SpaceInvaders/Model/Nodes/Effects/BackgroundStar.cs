using System;
using System.Numerics;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    public class BackgroundStar : SpriteNode
    {
        #region Data members

        private static readonly Random StarRandom = new Random();
        private const double MaxSpeed = 250;
        private const double MinSpeed = 100;

        private double velocity;

        #endregion

        #region Constructors

        public BackgroundStar() : base(new StarSprite())
        {
            this.setVelocityAndScale();
            this.setStartingPosition();
        }

        #endregion

        #region Methods

        private void setVelocityAndScale()
        {
            this.velocity = StarRandom.NextDouble() * (MaxSpeed - MinSpeed) + MinSpeed;
            Sprite.Scale = new Vector3((float) (this.velocity / MaxSpeed));
        }

        private void setStartingPosition()
        {
            Y = -Height;
            X = StarRandom.Next((int) (MainPage.ApplicationWidth - Width));
        }

        public override void Update(double delta)
        {
            Y += this.velocity * delta;
            if (IsOffScreen())
            {
                this.setVelocityAndScale();
                this.setStartingPosition();
            }

            base.Update(delta);
        }

        #endregion
    }
}