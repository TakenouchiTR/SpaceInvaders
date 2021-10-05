using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using SpaceInvaders.Model.Entities;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    public class UpdateTimer : GameObject
    {
        private double currentTime;

        public bool IsActive { get; private set; }
        public bool Repeat { get; set; }
        public double Duration { get; set; }
        public double TimeRemaining => Duration - this.currentTime;

        public UpdateTimer(GameManager manager) : base(manager, null)
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
                while (this.currentTime >= this.Duration)
                {
                    this.currentTime -= this.Duration;
                }

                if (!this.Repeat)
                {
                    this.Stop();
                }
            }
        }

        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            foreach (var subscriber in this.Tick.GetInvocationList())
            {
                this.Tick -= subscriber as EventHandler;
            }
        }
    }
}
