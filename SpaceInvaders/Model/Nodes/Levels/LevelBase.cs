using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace SpaceInvaders.Model.Nodes.Levels
{
    public abstract class LevelBase : Node
    {
        private const double MillisecondsInSecond = 1000;
        
        private readonly DispatcherTimer updateTimer;
        private long prevUpdateTime;
        private int score;
        private bool gameActive;

        public int Score
        {
            get => this.score;
            protected set
            {
                this.score = value;
                this.ScoreChanged?.Invoke(this, this.Score);
            }
        }

        protected LevelBase()
        {
            this.score = 0;
            this.gameActive = true;

            this.updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(.1)
            };
            this.updateTimer.Tick += this.onUpdateTimerTick;
            this.updateTimer.Start();

            this.prevUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public event EventHandler<int> ScoreChanged;
        public event EventHandler<string> GameFinished;
        
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

            this.Update(delta);
        }

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

            List<CollisionArea> sourceCollisionAreas = senderNode.GetCollisionAreas();

            foreach (var child in children)
            {
                if (child == sender)
                {
                    continue;
                }

                List<CollisionArea> targetCollisionAreas = child.GetCollisionAreas();
                testForCollisions(sourceCollisionAreas, targetCollisionAreas);
            }
        }

    }

}
