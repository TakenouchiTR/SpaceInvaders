using System;
using System.Numerics;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    /// <summary>
    ///     Represents a star that slowly moves down the screen in the background
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.SpriteNode" />
    public class BackgroundStar : SpriteNode
    {
        #region Data members

        private static readonly Random StarRandom = new Random();
        private const double MaxSpeed = 15;
        private const double MinSpeed = 5;

        private double velocity;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BackgroundStar" /> class.
        /// </summary>
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

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
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