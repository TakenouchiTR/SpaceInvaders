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
        ///     Initializes a new instance of the <see cref="AnimationFrame" /> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="duration">The duration.</param>
        public AnimationFrame(BaseSprite sprite, double duration)
        {
            this.Sprite = sprite;
            this.Duration = duration;
        }

        #endregion
    }
}