using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes
{
    public class SpriteNode : Area
    {
        #region Data members

        private bool visible;

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
            this.visible = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public SpriteNode(BaseSprite sprite)
        {
            this.Sprite = sprite;
            this.visible = true;

            SpriteShown?.Invoke(this, this.Sprite);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            SpriteHidden?.Invoke(null, this.Sprite);
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