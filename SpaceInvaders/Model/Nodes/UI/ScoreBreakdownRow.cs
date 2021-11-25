using Windows.UI;
using Windows.UI.Xaml;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Displays a row in the level end display
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Area" />
    public class ScoreBreakdownRow : Area
    {
        #region Data members

        /// <summary>
        ///     The source column width
        /// </summary>
        public const double SourceColumnWidth = 128;

        /// <summary>
        ///     The score column width
        /// </summary>
        public const double ScoreColumnWidth = 64;

        /// <summary>
        ///     The row height
        /// </summary>
        public const double RowHeight = 25;

        private const double FontSize = 20;
        private static readonly Color FontColor = Color.FromArgb(255, 0, 0, 0);

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScoreBreakdownRow" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="score">The score.</param>
        public ScoreBreakdownRow(string source, int score)
        {
            this.setupSize();
            this.setupLabels(source, score);
        }

        #endregion

        #region Methods

        private void setupSize()
        {
            Width = SourceColumnWidth + ScoreColumnWidth;
            Height = RowHeight;
        }

        private void setupLabels(string source, int score)
        {
            var sourceLabel = new Label(source, RenderLayer.UiMiddle) {
                FontColor = FontColor,
                FontSize = FontSize
            };
            var scoreLabel = new Label(score.ToString(), RenderLayer.UiMiddle) {
                X = SourceColumnWidth,
                Alignment = TextAlignment.Right,
                FontColor = FontColor,
                Width = ScoreColumnWidth,
                FontSize = FontSize
            };

            AttachChild(sourceLabel);
            AttachChild(scoreLabel);
        }

        #endregion
    }
}