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

        public int Score
        {
            get => this.score;
            private set
            {
                this.score = value;
                this.ScoreChanged?.Invoke(this, this.Score);
            }
        }

        protected LevelBase()
        {
            this.score = 0;

            this.updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(.1)
            };
            this.updateTimer.Tick += this.onUpdateTimerTick;
            this.updateTimer.Start();

            this.prevUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public event EventHandler<int> ScoreChanged;
        
        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            this.updateTimer.Tick -= this.onUpdateTimerTick;
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

            if (child is Enemy enemy)
            {
                enemy.Removed += this.onEnemyRemoved;
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

        private void onEnemyRemoved(object sender, EventArgs e)
        {
            if (sender is Enemy enemy)
            {
                this.Score += enemy.Score;
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
