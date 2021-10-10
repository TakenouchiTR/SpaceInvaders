using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    /// <summary>
    /// Represents a slow moving planet that occasionally appears in the background
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.SpriteNode" />
    public class BackgroundPlanet : SpriteNode
    {
        private static readonly Random RefreshRandom = new Random();
        private const double minTime = 5;
        private const double maxTime = 10;

        private Vector2 velocity;
        private Timer refreshTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundPlanet"/> class.
        /// </summary>
        public BackgroundPlanet() : base(new Planet1Sprite())
        {
            this.Y = MainPage.ApplicationHeight + 1;
            this.velocity = new Vector2(0, 30);
            this.setupTimer();
        }

        private void setupTimer()
        {
            this.refreshTimer = new Timer()
            {
                Duration = this.getNextRefreshTime(),
                Repeat = false
            };
            this.AttachChild(this.refreshTimer);
            this.refreshTimer.Tick += this.onRefreshTimerTick;
            this.refreshTimer.Start();
        }

        private void onRefreshTimerTick(object sender, EventArgs e)
        {
            this.moveToNewPosition();
        }

        private void moveToNewPosition()
        {
            this.Y = -this.Height;
            this.X = RefreshRandom.NextDouble() * MainPage.ApplicationWidth - this.Width;
        }

        /// <summary>
        /// The update loop for the GameObject.<br />
        /// Precondition: None<br />
        /// Postcondition: GameObject completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            this.Move(this.velocity * delta);

            if (this.IsOffScreen())
            {
                this.refreshTimer.Duration = this.getNextRefreshTime();
                this.refreshTimer.Start();
            }

            base.Update(delta);
        }

        private double getNextRefreshTime()
        {
            return RefreshRandom.NextDouble() * (maxTime - minTime) + minTime;
        }
    }
}
