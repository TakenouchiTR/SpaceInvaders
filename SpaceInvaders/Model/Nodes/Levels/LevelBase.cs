using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Nodes.Entities;

namespace SpaceInvaders.Model.Nodes.Levels
{
    public abstract class LevelBase : Node
    {
        private const double MillisecondsInSecond = 1000;
        
        private readonly DispatcherTimer updateTimer;
        private long prevUpdateTime;

        protected LevelBase()
        {
            this.updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(.1)
            };
            this.updateTimer.Tick += this.onUpdateTimerTick;
            this.updateTimer.Start();

            this.prevUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public abstract void Initialize(Canvas background);

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
    }
}
