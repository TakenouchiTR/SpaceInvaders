using Windows.UI;
using SpaceInvaders.View.UI;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Draws a rectangle
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.RenderableNode" />
    public class ColorRectangle : RenderableNode
    {
        #region Data members

        private readonly ColorRectangleSprite rectangleSprite;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the width of the rectangle.
        /// </summary>
        /// <value>
        ///     The width of the rectangle.
        /// </value>
        public override double Width
        {
            get => this.rectangleSprite.RectangleWidth;
            set => this.rectangleSprite.RectangleWidth = value;
        }

        /// <summary>
        ///     Gets or sets the height of the rectangle.
        /// </summary>
        /// <value>
        ///     The height of the rectangle.
        /// </value>
        public override double Height
        {
            get => this.rectangleSprite.RectangleHeight;
            set => this.rectangleSprite.RectangleHeight = value;
        }

        /// <summary>
        ///     Sets the fill color.
        /// </summary>
        /// <value>
        ///     The color.
        /// </value>
        public Color Color
        {
            set => this.rectangleSprite.Color = value;
        }

        /// <summary>
        ///     Sets the border color.
        /// </summary>
        /// <value>
        ///     The border color.
        /// </value>
        public Color BorderColor
        {
            set => this.rectangleSprite.BorderColor = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorRectangle" /> class.
        ///     Precondition: None<br />
        ///     Postcondition: this.Width == 100 &amp;&amp;<br />
        ///     this.Height == 100 &amp;&amp;<br />
        ///     this.RenderLayer == RenderableNode.DefaultRenderLayer
        /// </summary>
        public ColorRectangle() : this(DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorRectangle" /> class.
        ///     Precondition: None<br />
        ///     Postcondition: this.Width == 100 &amp;&amp;<br />
        ///     this.Height == 100 &amp;&amp;<br />
        ///     this.RenderLayer == layer
        /// </summary>
        public ColorRectangle(RenderLayer layer) : base(new ColorRectangleSprite(), layer)
        {
            this.rectangleSprite = (ColorRectangleSprite) Sprite;
        }

        #endregion
    }
}