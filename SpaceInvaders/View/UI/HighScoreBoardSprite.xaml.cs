using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.UI
{
    /// <summary>
    ///     Displays a High score table
    /// </summary>
    /// <seealso cref="SpaceInvaders.View.Sprites.BaseSprite" />
    public sealed partial class HighScoreBoardSprite
    {
        #region Properties

        /// <summary>
        ///     Gets the ListView.
        /// </summary>
        /// <value>
        ///     The ListView.
        /// </value>
        public ListView ListView => this.listView;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScoreBoardSprite" /> class.
        /// </summary>
        public HighScoreBoardSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}