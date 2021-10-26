using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A node that handles the display and movement of Sprites.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Area" />
    public class SpriteNode : RenderableNode
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class without a BaseSprite.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer
        /// </summary>
        public SpriteNode() : this(null, DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class with a specified Sprite.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer &amp;&amp;<br />
        ///     this.Sprite == sprite &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public SpriteNode(BaseSprite sprite) : this(sprite, DefaultRenderLayer)
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
        public SpriteNode(BaseSprite sprite, RenderLayer layer) : base(sprite, layer)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Changes the sprite to the specified sprite.
        /// </summary>
        /// <param name="newSprite">The new sprite.</param>
        public void ChangeSprite(BaseSprite newSprite)
        {
            var oldSprite = Sprite;
            Sprite = newSprite;

            if (!Visible)
            {
                return;
            }

            if (oldSprite != null)
            {
                OnSpriteHidden(oldSprite);
            }

            if (newSprite != null)
            {
                OnSpriteShown(newSprite);
                Render();
            }
        }

        #endregion
    }
}