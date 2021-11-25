using Windows.UI;
using Windows.UI.Xaml;
using SpaceInvaders.View.UI;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Draws a label to display some specified text.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.RenderableNode" />
    public class Label : RenderableNode
    {
        #region Data members

        private readonly LabelSprite labelSprite;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public override double Width
        {
            get => this.labelSprite.TextWidth;
            set => this.labelSprite.TextWidth = value;
        }

        /// <summary>
        ///     Gets the height.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public override double Height
        {
            get => this.labelSprite.TextHeight;
            set => this.labelSprite.TextHeight = value;
        }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        public string Text
        {
            get => this.labelSprite.Text;
            set => this.labelSprite.Text = value;
        }

        /// <summary>
        ///     Gets or sets the text alignment.
        /// </summary>
        /// <value>
        ///     The alignment.
        /// </value>
        public TextAlignment Alignment
        {
            get => this.labelSprite.Alignment;
            set => this.labelSprite.Alignment = value;
        }

        /// <summary>
        ///     Sets the color of the font.
        /// </summary>
        /// <value>
        ///     The color of the font.
        /// </value>
        public Color FontColor
        {
            set => this.labelSprite.FontColor = value;
        }

        /// <summary>
        ///     Gets or sets the size of the font.
        /// </summary>
        /// <value>
        ///     The size of the font.
        /// </value>
        public double FontSize
        {
            get => this.labelSprite.FontSize;
            set => this.labelSprite.FontSize = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Label" /> class with blank text.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == RenderableNode.DefaultRenderLayer
        /// </summary>
        public Label() : this(string.Empty, DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Label" /> class with specified text.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer &amp;&amp;<br />
        ///     this.Text == text &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="text">The default text.</param>
        public Label(string text) : this(text, DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Label" /> class with specified text and RenderLayer.
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == layer &amp;&amp;<br />
        ///     this.Text == text &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="text">The default text.</param>
        /// <param name="layer">The Render layer.</param>
        public Label(string text, RenderLayer layer) : base(new LabelSprite(), layer)
        {
            this.labelSprite = (LabelSprite) Sprite;
            this.labelSprite.Text = text;
        }

        #endregion
    }
}