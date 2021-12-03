// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites.UI
{
    /// <summary>
    ///     Draws a frame for the Graze Bar
    /// </summary>
    /// <seealso cref="SpaceInvaders.View.Sprites.BaseSprite" />
    public sealed partial class GrazeBarFrameSprite
    {
        #region Properties

        /// <summary>
        ///     Sets the height of the bar as a percent of the max height.
        /// </summary>
        /// <value>
        ///     The height of the bar.
        /// </value>
        public double BarHeight
        {
            set => this.bar.BarHeight = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrazeBarFrameSprite" /> class.
        /// </summary>
        public GrazeBarFrameSprite()
        {
            this.InitializeComponent();
            this.BarHeight = 0;
        }

        #endregion
    }
}