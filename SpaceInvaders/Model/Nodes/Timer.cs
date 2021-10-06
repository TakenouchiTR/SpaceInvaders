using System;
using SpaceInvaders.Model.Entities;

namespace SpaceInvaders.Model.Nodes
{
    public class Timer : GameObject
    {
        private double currentTime;

        public bool IsActive { get; private set; }
        public bool Repeat { get; set; }
        public double Duration { get; set; }
        public double TimeRemaining => Duration - this.currentTime;

        public Timer(GameManager manager) : base(manager, null)
        {
            this.currentTime = 0;
            this.Duration = 1;
            this.Repeat = true;
        }

        public event EventHandler Tick;

        public void Start()
        {
            this.IsActive = true;
        }

        public void Pause()
        {
            this.IsActive = false;
        }

        public void Stop()
        {
            this.IsActive = false;
            this.currentTime = 0;
        }

        public override void Update(double delta)
        {
            if (!this.IsActive)
            {
                return;
            }

            this.currentTime += delta;

            if (this.currentTime >= this.Duration)
            {
                this.Tick?.Invoke(this, EventArgs.Empty);
                this.currentTime = Math.IEEERemainder(this.currentTime, this.Duration);

                if (!this.Repeat)
                {
                    this.Stop();
                }
            }
        }

        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            foreach (var subscriber in this.Tick?.GetInvocationList())
            {
                this.Tick -= subscriber as EventHandler;
            }
        }
    }
}
