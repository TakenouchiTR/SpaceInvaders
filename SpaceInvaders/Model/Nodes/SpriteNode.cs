using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A node that handles the display and movement of Sprites.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Area" />
    public class SpriteNode : Area
    {
        #region Data members

        private const RenderLayer DefaultRenderLayer = RenderLayer.MainUpperMiddle;

        private bool visible;
        private RenderLayer layer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the sprite.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite { get; private set; }

        /// <summary>
        ///     Gets or sets the render layer.
        /// </summary>
        /// <value>
        ///     The render layer.
        /// </value>
        public RenderLayer Layer
        {
            get => this.layer;
            set
            {
                if (this.Visible)
                {
                    SpriteHidden?.Invoke(this, this.Sprite);
                }

                this.layer = value;
                if (this.Visible)
                {
                    SpriteShown?.Invoke(this, this.Sprite);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the x coordinate.
        /// </summary>
        /// <value>
        ///     The x coordinate.
        /// </value>
        public override double X
        {
            get => base.X;
            set
            {
                base.X = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        ///     The y coordinate.
        /// </value>
        public override double Y
        {
            get => base.Y;
            set
            {
                base.Y = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public override double Width => this.Sprite.Width;

        /// <summary>
        ///     Gets the height.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public override double Height => this.Sprite.Height;

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SpriteNode" /> is visible.
        /// </summary>
        /// <value>
        ///     <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public bool Visible
        {
            get => this.visible;
            set
            {
                if (this.visible == value)
                {
                    return;
                }

                this.visible = value;

                if (this.Sprite != null)
                {
                    if (value)
                    {
                        SpriteShown?.Invoke(this, this.Sprite);
                    }
                    else
                    {
                        SpriteHidden?.Invoke(this, this.Sprite);
                    }
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class without a BaseSprite.
        /// </summary>
        public SpriteNode()
        {
            this.visible = false;
            this.layer = DefaultRenderLayer;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class with a specified Sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public SpriteNode(BaseSprite sprite) : this(sprite, DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class with a specified Sprite and RenderLayer.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="layer">The render layer.</param>
        public SpriteNode(BaseSprite sprite, RenderLayer layer)
        {
            this.Sprite = sprite;
            this.visible = true;
            this.layer = layer;

            SpriteShown?.Invoke(this, this.Sprite);
        }

        #endregion

        #region Methods

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

            SpriteHidden?.Invoke(this, this.Sprite);
        }

        /// <summary>
        ///     Occurs when a sprite is Shown.
        /// </summary>
        public static event EventHandler<BaseSprite> SpriteShown;

        /// <summary>
        ///     Occurs when a sprite is Hidden.
        /// </summary>
        public static event EventHandler<BaseSprite> SpriteHidden;

        private void render()
        {
            var render = this.Sprite as ISpriteRenderer;

            render?.RenderAt(this.X, this.Y);
        }

        /// <summary>
        ///     Changes the sprite to the specified sprite.
        /// </summary>
        /// <param name="newSprite">The new sprite.</param>
        public void ChangeSprite(BaseSprite newSprite)
        {
            var oldSprite = this.Sprite;
            this.Sprite = newSprite;

            if (!this.Visible)
            {
                return;
            }

            if (oldSprite != null)
            {
                SpriteHidden?.Invoke(this, oldSprite);
            }

            if (newSprite != null)
            {
                SpriteShown?.Invoke(this, newSprite);
                this.render();
            }
        }

        #endregion
    }
}