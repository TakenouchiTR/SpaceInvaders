using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Represents a single frame of animation. Stores the frame and the duration.
    /// </summary>
    public class AnimationFrame
    {
        #region Properties

        /// <summary>
        ///     Gets the duration for the frame.
        /// </summary>
        /// <value>
        ///     The frame's duration.
        /// </value>
        public double Duration { get; }

        /// <summary>
        ///     Gets the sprite.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnimationFrame" /> class.<br />
        ///     Precondition: duration &gt; 0<br />
        ///     Postcondition: this.Sprite == sprite &amp;&amp;<br />
        ///     this.Duration == duration
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="duration">The duration.</param>
        /// <exception cref="System.ArgumentException">duration must be a positive number</exception>
        public AnimationFrame(BaseSprite sprite, double duration)
        {
            if (duration <= 0)
            {
                throw new ArgumentException("duration must be a positive number");
            }

            this.Sprite = sprite;
            this.Duration = duration;
        }

        #endregion
    }
}