using System;
using System.Collections.Generic;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     Handles playing animated sprites.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.SpriteNode" />
    public class AnimatedSprite : SpriteNode
    {
        #region Data members

        private int currentFrame;
        private readonly List<AnimationFrame> frames;
        private Timer frameTimer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the current frame of the animation.<br/>
        ///     If the new frame is greater than the total number of frames or below 0, it will loop around.
        /// </summary>
        /// <value>
        ///     The current frame.
        /// </value>
        public int Frame
        {
            get => this.currentFrame;
            set
            {
                if (this.currentFrame == value)
                {
                    return;
                }
                
                this.currentFrame = value % this.frames.Count;
                if (this.currentFrame < 0)
                {
                    this.currentFrame += this.frames.Count;
                }
                ChangeSprite(this.frames[this.currentFrame].Sprite);
                this.frameTimer.Duration = this.frames[this.currentFrame].Duration;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the animation is playing
        /// </summary>
        /// <value>
        ///     <c>true</c> if the animation is playing; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaying => this.frameTimer.IsActive;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnimatedSprite" /> class.
        /// </summary>
        /// <param name="frames">The frames.</param>
        /// <exception cref="System.ArgumentException">frames must not be empty</exception>
        public AnimatedSprite(List<AnimationFrame> frames)
        {
            if (frames.Count == 0)
            {
                throw new ArgumentException("frames must not be empty");
            }

            this.setupTimer(frames[0].Duration);

            this.frames = frames;
            ChangeSprite(this.frames[0].Sprite);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnimatedSprite" /> class.<br/>
        ///     Converts the collection of BaseSprites into AnimationFrames, each with the specified frame duration. 
        /// </summary>
        /// <param name="frameDuration">Duration of each frame, in seconds.</param>
        /// <param name="frames">The frames.</param>
        /// <exception cref="System.ArgumentException">frames must not be empty</exception>
        public AnimatedSprite(double frameDuration, ICollection<BaseSprite> frames)
        {
            if (frames.Count == 0)
            {
                throw new ArgumentException("frames must not be empty");
            }

            this.frames = new List<AnimationFrame>();
            foreach (var baseSprite in frames)
            {
                this.frames.Add(new AnimationFrame(baseSprite, frameDuration));
            }

            this.setupTimer(frameDuration);
            ChangeSprite(this.frames[0].Sprite);
        }

        #endregion

        #region Methods

        private void setupTimer(double frameDuration)
        {
            this.frameTimer = new Timer(frameDuration);
            this.frameTimer.Tick += this.onFrameTimerTick;
            this.frameTimer.Start();
            AttachChild(this.frameTimer);
        }

        /// <summary>
        ///     Pauses the animation on the current frame.<br />
        ///     Precondition: None<br />
        ///     Postcondition: The animation is stopped on the current frame.
        /// </summary>
        public void Pause()
        {
            this.frameTimer.Stop();
        }

        /// <summary>
        ///     Starts the animation. Has no effect if the animation is already playing.<br />
        ///     Precondition: None<br />
        ///     Postcondition: The animation is playing
        /// </summary>
        public void Start()
        {
            this.frameTimer.Start();
        }

        /// <summary>
        ///     Stops the animation and returns it to the first frame, even if it was already paused.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Frame == 0 &amp;&amp;<br />
        ///     The animation is stopped
        /// </summary>
        public void Stop()
        {
            this.frameTimer.Stop();
            this.Frame = 0;
        }

        private void onFrameTimerTick(object sender, EventArgs e)
        {
            ++this.Frame;
        }

        #endregion
    }
}