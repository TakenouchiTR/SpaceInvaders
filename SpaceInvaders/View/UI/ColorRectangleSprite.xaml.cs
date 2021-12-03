using Windows.UI;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.UI
{
    /// <summary>
    ///     Draws a colored rectangle
    /// </summary>
    /// <seealso cref="SpaceInvaders.View.Sprites.BaseSprite" />
    public sealed partial class ColorRectangleSprite
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the width of the rectangle.
        /// </summary>
        /// <value>
        ///     The width of the rectangle.
        /// </value>
        public double RectangleWidth
        {
            get => this.rectangle.Width;
            set => this.rectangle.Width = value;
        }

        /// <summary>
        ///     Gets or sets the height of the rectangle.
        /// </summary>
        /// <value>
        ///     The height of the rectangle.
        /// </value>
        public double RectangleHeight
        {
            get => this.rectangle.Height;
            set => this.rectangle.Height = value;
        }

        /// <summary>
        ///     Sets the fill color.
        /// </summary>
        /// <value>
        ///     The color.
        /// </value>
        public Color Color
        {
            set => this.rectangle.Fill = new SolidColorBrush(value);
        }

        /// <summary>
        ///     Sets the border color.
        /// </summary>
        /// <value>
        ///     The border color.
        /// </value>
        public Color BorderColor
        {
            set => this.rectangle.Stroke = new SolidColorBrush(value);
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorRectangleSprite" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.RectangleWidth == 100 &amp;&amp;<br />
        ///     this.RectangleHeight == 100
        /// </summary>
        public ColorRectangleSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}