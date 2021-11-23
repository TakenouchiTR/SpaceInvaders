using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace SpaceInvaders.Model.Nodes.Screens.Levels
{
    /// <summary>
    ///     The basic structure for other levels.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public abstract class LevelBase : Screen
    {
        #region Data members

        private const double MillisecondsInSecond = 1000;
        private const double UpdateSkipThreshold = 1;

        private readonly DispatcherTimer updateTimer;
        private long prevUpdateTime;
        private int score;
        private bool levelComplete;
        private Type nextScreen;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score
        {
            get => this.score;
            protected set
            {
                this.score = value;
                this.ScoreChanged?.Invoke(this, this.Score);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelBase" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        protected LevelBase()
        {
            this.score = 0;

            this.updateTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(.1)
            };
            this.updateTimer.Tick += this.onUpdateTimerTick;
            this.updateTimer.Start();

            this.prevUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when the score is changed.
        /// </summary>
        public event EventHandler<int> ScoreChanged;

        /// <summary>
        ///     Readies the level to be ended after the current update tick.<br />
        ///     Precondition: Type != null<br />
        ///     Postcondition: The level is unloaded after the current update tick
        /// </summary>
        /// <param name="screen">The next screen.</param>
        protected void EndLevel(Type screen)
        {
            this.nextScreen = screen;
            this.levelComplete = true;
        }

        private void testForCollisions(IEnumerable<CollisionArea> sourceAreas, IList<CollisionArea> targetAreas)
        {
            foreach (var sourceArea in sourceAreas)
            {
                foreach (var targetArea in targetAreas)
                {
                    sourceArea.DetectCollision(targetArea);
                }
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
            base.Update(delta);
            if (this.levelComplete)
            {
                CompleteScreen(this.nextScreen);
            }
        }

        /// <summary>
        ///     Attaches a new child to the node.<br />
        ///     Precondition: child != null<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="child">The child.</param>
        public override void AttachChild(Node child)
        {
            base.AttachChild(child);
            if (child is Node2D node2D)
            {
                node2D.Moved += this.onChildMoved;
            }
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked if emitRemovedEvent == true &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        /// <param name="emitRemovedEvent">Whether to emit the Removed event</param>
        public override void CompleteRemoval(bool emitRemovedEvent = true)
        {
            base.CompleteRemoval(false);
            this.updateTimer.Stop();
            this.updateTimer.Tick -= this.onUpdateTimerTick;

            if (this.ScoreChanged != null)
            {
                foreach (var subscriber in this.ScoreChanged.GetInvocationList())
                {
                    this.ScoreChanged -= subscriber as EventHandler<int>;
                }
            }
        }

        private void onUpdateTimerTick(object sender, object e)
        {
            var curTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var timeSinceLastTick = curTime - this.prevUpdateTime;
            var delta = timeSinceLastTick / MillisecondsInSecond;
            this.prevUpdateTime = curTime;

            if (delta < UpdateSkipThreshold)
            {
                this.Update(delta);
            }
        }

        private void onChildMoved(object sender, Vector2 e)
        {
            var senderNode = sender as Node;
            if (senderNode == null)
            {
                throw new ArgumentException("sender must be a Node");
            }

            var sourceCollisionAreas = senderNode.GetCollisionAreas();

            foreach (var child in Children)
            {
                if (child == sender)
                {
                    continue;
                }

                var targetCollisionAreas = child.GetCollisionAreas();
                this.testForCollisions(sourceCollisionAreas, targetCollisionAreas);
            }
        }

        #endregion
    }
}