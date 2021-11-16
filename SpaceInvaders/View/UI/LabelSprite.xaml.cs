// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using SpaceInvaders.Model;

namespace SpaceInvaders.View.UI
{
    /// <summary>
    ///     Draws text at a given location
    /// </summary>
    /// <seealso cref="SpaceInvaders.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class LabelSprite
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        public string Text
        {
            get => this.textBlock.Text;
            set => this.textBlock.Text = value;
        }

        /// <summary>
        ///     Gets or sets the width of the render area of the text.
        /// </summary>
        /// <value>
        ///     The width of the render area of the text.
        /// </value>
        public double TextWidth
        {
            get => this.textBlock.Width;
            set => this.textBlock.Width = value;
        }

        /// <summary>
        ///     Gets or sets the height of the render area of the text.
        /// </summary>
        /// <value>
        ///     The height of the render area of the text.
        /// </value>
        public double TextHeight
        {
            get => this.textBlock.Height;
            set => this.textBlock.Height = value;
        }

        /// <summary>
        ///     Gets the size of the render area of the text.
        /// </summary>
        /// <value>
        ///     The size of the render area of the text.
        /// </value>
        public Vector2 TextSize => new Vector2(this.TextWidth, this.TextHeight);

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LabelSprite" /> class.
        /// </summary>
        public LabelSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}