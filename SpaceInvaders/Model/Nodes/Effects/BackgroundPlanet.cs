using System;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Effects
{
    /// <summary>
    ///     Represents a slow moving planet that occasionally appears in the background
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.SpriteNode" />
    public class BackgroundPlanet : SpriteNode
    {
        #region Data members

        private static readonly Random BackgroundPlanetRandom = new Random();
        private const double MinRefreshTime = 5;
        private const double MaxRefreshTime = 10;

        private readonly Vector2 velocity;
        private Timer refreshTimer;
        private bool active;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BackgroundPlanet" /> class.
        /// </summary>
        public BackgroundPlanet() : base(new Planet1Sprite(), RenderLayer.BackgroundMiddle)
        {
            this.velocity = new Vector2(0, 30);
            this.setupTimer();
            this.active = false;

            this.placeOffScreen();
        }

        #endregion

        #region Methods

        private void placeOffScreen()
        {
            Y = MainPage.ApplicationHeight + 1;
        }

        private void setupTimer()
        {
            this.refreshTimer = new Timer {
                Duration = getNextRefreshTime(),
                Repeat = false
            };
            AttachChild(this.refreshTimer);
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
            Bottom = 0;
            X = BackgroundPlanetRandom.NextDouble() * (MainPage.ApplicationWidth + Width * 2) - Width;
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (this.active)
            {
                Move(this.velocity * delta);

                if (IsOffScreen())
                {
                    this.refreshTimer.Duration = getNextRefreshTime();
                    this.refreshTimer.Start();
                    this.active = false;
                }
            }

            base.Update(delta);
        }

        private static double getNextRefreshTime()
        {
            return BackgroundPlanetRandom.NextDouble() * (MaxRefreshTime - MinRefreshTime) + MinRefreshTime;
        }

        #endregion
    }
}