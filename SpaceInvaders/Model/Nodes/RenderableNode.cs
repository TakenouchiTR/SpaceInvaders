using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     Represents any node that may be rendered.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Area" />
    public abstract class RenderableNode : Area
    {
        #region Data members

        /// <summary>
        ///     The default Render layer
        /// </summary>
        public const RenderLayer DefaultRenderLayer = RenderLayer.MainUpperMiddle;

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
        public BaseSprite Sprite { get; protected set; }

        /// <summary>
        ///     Gets or sets the Render layer.
        /// </summary>
        /// <value>
        ///     The Render layer.
        /// </value>
        public RenderLayer Layer
        {
            get => this.layer;
            set
            {
                if (this.Visible && this.Sprite != null)
                {
                    SpriteHidden?.Invoke(this, this.Sprite);
                }

                this.layer = value;

                if (this.Visible && this.Sprite != null)
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
                this.Render();
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
                this.Render();
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
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class without a BaseSprite.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer
        /// </summary>
        protected RenderableNode()
        {
            this.layer = DefaultRenderLayer;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class with a specified Sprite.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer &amp;&amp;<br />
        ///     this.Sprite == sprite &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        protected RenderableNode(BaseSprite sprite) : this(sprite, DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class with a specified Sprite and RenderLayer.
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == layer &amp;&amp;<br />
        ///     this.Sprite == sprite &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="layer">The Render layer.</param>
        protected RenderableNode(BaseSprite sprite, RenderLayer layer)
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
        ///     Invokes the SpriteShown event.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        protected void OnSpriteShown(BaseSprite sprite)
        {
            SpriteShown?.Invoke(this, sprite);
        }

        /// <summary>
        ///     Invokes the SpriteHidden event.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        protected void OnSpriteHidden(BaseSprite sprite)
        {
            SpriteHidden?.Invoke(this, sprite);
        }

        /// <summary>
        ///     Occurs when a sprite is Shown.
        /// </summary>
        public static event EventHandler<BaseSprite> SpriteShown;

        /// <summary>
        ///     Occurs when a sprite is Hidden.
        /// </summary>
        public static event EventHandler<BaseSprite> SpriteHidden;

        /// <summary>
        ///     Renders this instance.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        protected void Render()
        {
            var render = this.Sprite as ISpriteRenderer;

            render?.RenderAt(this.X, this.Y);
        }

        #endregion
    }
}