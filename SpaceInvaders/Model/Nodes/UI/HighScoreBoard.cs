using Windows.UI.Xaml.Controls;
using SpaceInvaders.View.UI;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Displays the high score board
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.RenderableNode" />
    public class HighScoreBoard : RenderableNode
    {
        #region Data members

        private readonly HighScoreBoardSprite highScoreBoardSprite;

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
            get => this.highScoreBoardSprite.Width;
            set => this.highScoreBoardSprite.Width = value;
        }

        /// <summary>
        ///     Gets the height.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public override double Height
        {
            get => this.highScoreBoardSprite.Height;
            set => this.highScoreBoardSprite.Height = value;
        }

        /// <summary>
        ///     Gets the score board ListView.
        /// </summary>
        /// <value>
        ///     The score board ListView.
        /// </value>
        public ListView ScoreBoardListView => this.highScoreBoardSprite.ListView;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScoreBoard" /> class.
        /// </summary>
        public HighScoreBoard() : this(DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScoreBoard" /> class at a specified render layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public HighScoreBoard(RenderLayer layer) : base(new HighScoreBoardSprite(), layer)
        {
            this.highScoreBoardSprite = (HighScoreBoardSprite) Sprite;
        }

        #endregion
    }
}