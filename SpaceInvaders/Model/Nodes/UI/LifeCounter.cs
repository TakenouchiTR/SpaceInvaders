using System;
using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Draws a life counter
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node2D" />
    public class LifeCounter: Node2D
    {
        #region Data members

        private const double SpritePadding = 8;
        private static readonly Type BaseSpriteType = typeof(BaseSprite);

        private readonly List<AnimatedSprite> lifeSprites;
        private readonly Type healthySprite;
        private readonly Type hurtSprite;
        private readonly RenderLayer layer;

        private int maxLives;
        private int currentLives;

        #endregion

        #region Properties
        /// <summary>
        ///     Gets or sets the maximum lives.<br />
        ///     MaxLives will not go below 0.
        /// </summary>
        /// <value>
        ///     The maximum lives.
        /// </value>
        public int MaxLives
        {
            get => this.maxLives; 
            set => this.maxLives = Math.Max(value, 0);
        }

        /// <summary>
        ///     Gets or sets the current lives.<br />
        ///     Current lives is clamped between 0 and this.MaxLives.
        /// </summary>
        /// <value>
        ///     The current lives.
        /// </value>
        public int CurrentLives
        {
            get => this.currentLives;
            set
            {
                value = Math.Clamp(value, 0, this.MaxLives);
                this.currentLives = value;

                for (var i = 0; i < this.CurrentLives; ++i)
                {
                    this.lifeSprites[i].Frame = 0;
                }

                for (var i = this.CurrentLives; i < this.MaxLives; ++i)
                {
                    this.lifeSprites[i].Frame = 1;
                }
            }
        }

        /// <summary>
        ///     Gets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public double Width => this.lifeSprites.Last().Right - this.lifeSprites[0].Left;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LifeCounter" /> class.<br />
        ///     Precondition: healthySprite must be the type of a class derived from BaseSprite&amp;&amp;<br />
        ///     hurtSprite must be the type of a class derived from BaseSprite&amp;&amp;<br />
        ///     lives &gt; 0<br />
        ///     Postcondition: this.MaxLives == lives &amp;&amp;<br />
        ///     this.CurrentLives == lives
        /// </summary>
        /// <param name="healthySprite">The type of the healthy sprite.</param>
        /// <param name="hurtSprite">The type of the hurt sprite.</param>
        /// <param name="lives">The number of lives.</param>
        /// <param name="layer">The layer.</param>
        /// <exception cref="System.ArgumentException">
        ///     healthySprite must either be the type of a class derived from BaseSprite.
        ///     or
        ///     hurtSprite must either be the type of a class derived from BaseSprite.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">lives</exception>
        public LifeCounter(Type healthySprite, Type hurtSprite, int lives, RenderLayer layer)
        {
            if (!healthySprite.IsSubclassOf(BaseSpriteType))
            {
                throw new ArgumentException("healthySprite must either be the type of a class derived from BaseSprite.");
            }
            if (!hurtSprite.IsSubclassOf(BaseSpriteType))
            {
                throw new ArgumentException("hurtSprite must either be the type of a class derived from BaseSprite.");
            }

            if (lives <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lives));
            }

            this.healthySprite = healthySprite;
            this.hurtSprite = hurtSprite;
            this.maxLives = lives;
            this.currentLives = lives;
            this.layer = layer;

            this.lifeSprites = new List<AnimatedSprite>();

            this.createInitialSprites();
        }

        #endregion

        #region Methods

        private void createInitialSprites()
        {
            var currentX = 0.0;

            for (var i = 0; i < this.MaxLives; ++i)
            {
                var sprite = this.createSprite();
                sprite.Stop();

                sprite.X = currentX;
                currentX += sprite.Width + SpritePadding;

                this.lifeSprites.Add(sprite);
                this.AttachChild(sprite);
            }
        }

        private AnimatedSprite createSprite()
        {
            var frames = new List<AnimationFrame>() 
            {
                createFrame(this.healthySprite),
                createFrame(this.hurtSprite)
            };

            var sprite = new AnimatedSprite(frames) 
            {
                Layer = this.layer
            };

            return sprite;
        }

        private static AnimationFrame createFrame(Type frameType)
        {
            var constructor = frameType.GetConstructors()[0];
            var sprite = (BaseSprite) constructor.Invoke(new object[] {});

            var frame = new AnimationFrame(sprite, 1);
            return frame;
        }

        #endregion
    }
}
