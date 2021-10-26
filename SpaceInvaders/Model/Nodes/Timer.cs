using System;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A node that fires an event after a specified amount of time has passed.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public class Timer : Node
    {
        #region Data members

        private double currentTime;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether the Timer is actively counting down.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; private set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Timer" /> will repeat.
        /// </summary>
        /// <value>
        ///     <c>true</c> if repeating; otherwise, <c>false</c>.
        /// </value>
        public bool Repeat { get; set; }

        /// <summary>
        ///     Gets or sets the duration in seconds.
        /// </summary>
        /// <value>
        ///     The duration in seconds.
        /// </value>
        public double Duration { get; set; }

        /// <summary>
        ///     Gets the time remaining in seconds.
        /// </summary>
        /// <value>
        ///     The time remaining in seconds.
        /// </value>
        public double TimeRemaining => this.Duration - this.currentTime;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Timer" /> class.<br />
        ///     By default, the timer is set to 1 second and will auto repeat.<br />
        ///     Precondition: None
        ///     Postcondition: this.Duration == 1 &amp;&amp;<br />
        ///     this.Repeat == repeat &amp;&amp;<br />
        ///     this.TimeRemaining == 1
        /// </summary>
        public Timer() : this(1)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Timer" /> class with a specified duration in seconds.<br />
        ///     By default, the timer will auto repeat.<br />
        ///     Precondition: Duration &gt; 0<br />
        ///     Postcondition: this.Duration == duration &amp;&amp;<br />
        ///     this.Repeat == true &amp;&amp;<br />
        ///     this.TimeRemaining == duration
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <exception cref="System.ArgumentException">duration must be a positive number</exception>
        public Timer(double duration) : this(duration, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Timer" /> class with a specified duration and repeat option.<br />
        ///     Precondition: Duration &gt; 0<br />
        ///     Postcondition: this.Duration == duration &amp;&amp;<br />
        ///     this.Repeat == repeat &amp;&amp;<br />
        ///     this.TimeRemaining == duration
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="repeat">if set to <c>true</c> [repeat].</param>
        /// <exception cref="System.ArgumentException">duration must be a positive number</exception>
        public Timer(double duration, bool repeat)
        {
            if (duration <= 0)
            {
                throw new ArgumentException("duration must be a positive number");
            }

            this.currentTime = 0;
            this.Duration = duration;
            this.Repeat = repeat;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when the timer is complete.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        ///     Starts the timer.<br />
        ///     Timers that are already active will remain active.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.IsActive == true
        /// </summary>
        public void Start()
        {
            this.IsActive = true;
        }

        /// <summary>
        ///     Starts the timer from the beginning.<br />
        ///     Timers that are already active will remain active, but will have their TimeRemaining reset.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.IsActive == true &amp;&amp;<br />
        ///     this.TimeRemaining == this.Duration
        /// </summary>
        public void Restart()
        {
            this.IsActive = true;
            this.currentTime = 0;
        }

        /// <summary>
        ///     Pauses the timer without resetting the time.<br />
        ///     Timers that are already inactive will remain inactive.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.IsActive == false
        /// </summary>
        public void Pause()
        {
            this.IsActive = false;
        }

        /// <summary>
        ///     Stops the timer and resets the time.<br />
        ///     Timers that are paused will remain inactive and still have their time reset.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.IsActive == false &amp;&amp;<br />
        ///     this.TimeRemaining == this.Duration
        /// </summary>
        public void Stop()
        {
            this.IsActive = false;
            this.currentTime = 0;
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
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

                var timeQuotient = (int)(this.currentTime / this.Duration);
                this.currentTime -= (this.Duration * timeQuotient);

                if (!this.Repeat)
                {
                    this.Stop();
                }
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
            base.CompleteRemoval(emitRemovedEvent);
            if (this.Tick != null)
            {
                foreach (var subscriber in this.Tick?.GetInvocationList())
                {
                    this.Tick -= subscriber as EventHandler;
                }
            }
        }

        #endregion
    }
}