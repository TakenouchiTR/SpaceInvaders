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

        private readonly Vector2 velocity;
        private Timer refreshTimer;
        private bool active;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundPlanet"/> class.
        /// </summary>
        public BackgroundPlanet() : base(new Planet1Sprite())
        {
            this.Y = MainPage.ApplicationHeight + 1;
            this.velocity = new Vector2(0, 30);
            this.setupTimer();
            this.active = false;
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
            this.active = true;
        }

        private void moveToNewPosition()
        {
            this.Bottom = 0;
            this.X = (RefreshRandom.NextDouble() * (MainPage.ApplicationWidth + this.Width * 2)) - this.Width;
        }

        /// <summary>
        /// The update loop for the GameObject.<br />
        /// Precondition: None<br />
        /// Postcondition: GameObject completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (this.active)
            {
                this.Move(this.velocity * delta);

                if (this.IsOffScreen())
                {
                    this.refreshTimer.Duration = this.getNextRefreshTime();
                    this.refreshTimer.Start();
                    this.active = false;
                }

            }

            base.Update(delta);
        }

        private double getNextRefreshTime()
        {
            return RefreshRandom.NextDouble() * (maxTime - minTime) + minTime;
        }
    }
}
