using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    /// Represents a single frame of animation. Stores the frame and the duration.
    /// </summary>
    public class AnimationFrame
    {
        /// <summary>
        /// Gets the duration for the frame.
        /// </summary>
        /// <value>
        /// The frame's duration.
        /// </value>
        public double Duration { get; private set; }
        /// <summary>
        /// Gets the sprite.
        /// </summary>
        /// <value>
        /// The sprite.
        /// </value>
        public BaseSprite Sprite { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationFrame" /> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="duration">The duration.</param>
        public AnimationFrame(BaseSprite sprite, double duration)
        {
            this.Sprite = sprite;
            this.Duration = duration;
        }
    }
}
