using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace SpaceInvaders.Model.Nodes.Levels
{
    /// <summary>
    ///     The basic structure for other levels.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public abstract class LevelBase : Node
    {
        #region Data members

        private const double MillisecondsInSecond = 1000;

        private readonly DispatcherTimer updateTimer;
        private long prevUpdateTime;
        private int score;
        private bool gameActive;

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
        ///     Initializes a new instance of the <see cref="LevelBase" /> class.
        /// </summary>
        protected LevelBase()
        {
            this.score = 0;
            this.gameActive = true;

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
        ///     Occurs when the game is finished.
        /// </summary>
        public event EventHandler<string> GameFinished;

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            this.updateTimer.Tick -= this.onUpdateTimerTick;

            if (this.ScoreChanged != null)
            {
                foreach (var subscriber in this.ScoreChanged.GetInvocationList())
                {
                    this.ScoreChanged -= subscriber as EventHandler<int>;
                }
            }

            if (this.GameFinished != null)
            {
                foreach (var subscriber in this.GameFinished.GetInvocationList())
                {
                    this.GameFinished -= subscriber as EventHandler<string>;
                }
            }
        }

        /// <summary>
        ///     Fires the GameFinished event with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void CompleteGame(string message)
        {
            if (!this.gameActive)
            {
                return;
            }

            this.GameFinished?.Invoke(this, message);
            this.gameActive = false;
        }

        private void onUpdateTimerTick(object sender, object e)
        {
            var curTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var timeSinceLastTick = curTime - this.prevUpdateTime;
            var delta = timeSinceLastTick / MillisecondsInSecond;
            this.prevUpdateTime = curTime;

            Update(delta);
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

        private void testForCollisions(List<CollisionArea> sourceAreas, List<CollisionArea> targetAreas)
        {
            foreach (var sourceArea in sourceAreas)
            {
                foreach (var targetArea in targetAreas)
                {
                    sourceArea.DetectCollision(targetArea);
                }
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