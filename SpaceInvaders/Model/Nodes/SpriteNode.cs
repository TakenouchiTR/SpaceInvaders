using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes
{
    public class SpriteNode : Area
    {
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

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public SpriteNode(BaseSprite sprite)
        {
            this.Sprite = sprite;

            SpriteAdded?.Invoke(this, this.Sprite);
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
            SpriteRemoved?.Invoke(null, this.Sprite);
        }

        /// <summary>
        ///     Occurs when a sprite is added.
        /// </summary>
        public static event EventHandler<BaseSprite> SpriteAdded;

        /// <summary>
        ///     Occurs when a sprite is removed.
        /// </summary>
        public static event EventHandler<BaseSprite> SpriteRemoved;

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
            SpriteRemoved?.Invoke(this, this.Sprite);
            this.Sprite = newSprite;
            SpriteAdded?.Invoke(this, this.Sprite);
            this.render();
        }

        #endregion
    }
}