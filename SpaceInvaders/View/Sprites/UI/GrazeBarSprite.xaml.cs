// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites.UI
{
    /// <summary>
    ///     Draws a green bar to the screen
    /// </summary>
    /// <seealso cref="SpaceInvaders.View.Sprites.BaseSprite" />
    public sealed partial class GrazeBarSprite
    {
        /// <summary>
        ///     Sets the height of the bar as a percent of the max height.
        /// </summary>
        /// <value>
        ///     The height of the bar.
        /// </value>
        public double BarHeight
        {
            set => this.bar.Height = this.Height * value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrazeBarSprite"/> class.
        /// </summary>
        public GrazeBarSprite()
        {
            this.InitializeComponent();
        }
    }
}
