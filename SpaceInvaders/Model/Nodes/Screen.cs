using System;
using Windows.UI.Xaml;
using SpaceInvaders.Model.Nodes.Effects;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     Represents the base of any root node
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public abstract class Screen : Node
    {
        #region Data members

        private const double MillisecondsInSecond = 1000;
        private const double UpdateSkipThreshold = 1;

        private long prevUpdateTime;
        private DispatcherTimer updateTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Screen" /> class.
        /// </summary>
        protected Screen()
        {
            this.setupTimer();
            this.addBackground();
        }

        #endregion

        #region Methods

        private void setupTimer()
        {
            this.prevUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            this.updateTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(.1)
            };
            this.updateTimer.Tick += this.onUpdateTimerTick;
            this.updateTimer.Start();
        }

        private void addBackground()
        {
            AttachChild(new Background());
        }

        /// <summary>
        ///     Occurs when the screen is [complete].
        ///     Passes the Type of the next screen as the eventArgs.
        /// </summary>
        public event EventHandler<Type> Complete;

        /// <summary>
        ///     Safely removes all children from the node without firing their Removed events, then completes the <br />
        ///     removal step on itself.<br />
        ///     Precondition: None <br />
        ///     Postcondition: this.children.Count == 0 &amp;&amp;
        ///     All event subscribers are removed
        /// </summary>
        public void CleanupScreen()
        {
            SilentlyRemoveAllChildren();

            this.CompleteRemoval(false);
        }

        /// <summary>
        ///     Invokes the Complete event with the specified eventArgs.
        /// </summary>
        /// <param name="type">The type.</param>
        protected virtual void CompleteScreen(Type type)
        {
            this.Complete?.Invoke(this, type);
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

            if (this.Complete != null)
            {
                foreach (var subscriber in this.Complete.GetInvocationList())
                {
                    this.Complete -= subscriber as EventHandler<Type>;
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
                Update(delta);
            }
        }

        #endregion
    }
}