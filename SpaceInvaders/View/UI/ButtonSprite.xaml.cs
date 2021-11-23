// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.View.UI
{
    /// <summary>
    ///     Displays a button
    /// </summary>
    /// <seealso cref="SpaceInvaders.View.Sprites.BaseSprite" />
    public sealed partial class ButtonSprite
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the text displayed in the button.
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        public string Text
        {
            get => this.button.Content?.ToString();
            set => this.button.Content = value;
        }

        /// <summary>
        ///     Gets or sets the button width.
        /// </summary>
        public double ButtonWidth
        {
            get => this.button.Width;
            set => this.button.Width = value;
        }

        /// <summary>
        ///     Gets or sets the button height.
        /// </summary>
        public double ButtonHeight
        {
            get => this.button.Height;
            set => this.button.Height = value;
        }

        /// <summary>
        ///     Gets the button.
        /// </summary>
        /// <value>
        ///     The button.
        /// </value>
        public Button Button => this.button;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonSprite" /> class.
        /// </summary>
        public ButtonSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}