using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Displays the score breakdown at the end of a level
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Area" />
    public class LevelOverDisplay : Area
    {
        #region Data members

        /// <summary>
        ///     The number of pixels between the content and the edge of the display.
        /// </summary>
        public const int Padding = 16;

        private const double ButtonWidth = 128;
        private const double ButtonHeight = 32;

        private List<ScoreBreakdownRow> rows;
        private Button continueButton;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelOverDisplay" /> class.
        /// </summary>
        /// <param name="scoreBreakdown">The score breakdown.</param>
        public LevelOverDisplay(Dictionary<PointSource, int> scoreBreakdown)
        {
            this.setupBreakdownRows(scoreBreakdown);
            this.setupButton();
            this.setupSize();
            this.setupBackground();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [continue is clicked].
        /// </summary>
        public event EventHandler ContinueClicked;

        private void setupBreakdownRows(Dictionary<PointSource, int> scoreBreakdown)
        {
            this.rows = new List<ScoreBreakdownRow>();

            double currentY = Padding;

            foreach (var pair in scoreBreakdown)
            {
                var source = pair.Key.ToString();
                var score = pair.Value;

                var row = new ScoreBreakdownRow(source, score) {
                    Y = currentY,
                    X = Padding
                };
                currentY += row.Height;

                AttachChild(row);
                this.rows.Add(row);
            }

            var total = scoreBreakdown.Values.Sum();
            var totalRow = new ScoreBreakdownRow("Total", total) {
                Y = currentY + Padding,
                X = Padding
            };

            AttachChild(totalRow);
            this.rows.Add(totalRow);
        }

        private void setupButton()
        {
            this.continueButton = new Button("Continue", RenderLayer.UiMiddle) {
                Width = ButtonWidth,
                Height = ButtonHeight,
                Y = this.rows.Last().Bottom + Padding * 2,
                X = this.rows[0].Width / 2 - ButtonWidth / 2 + Padding
            };

            this.continueButton.Click += this.onClick;

            AttachChild(this.continueButton);
        }

        private void setupSize()
        {
            Width = ScoreBreakdownRow.ScoreColumnWidth + ScoreBreakdownRow.SourceColumnWidth + 2 * Padding;
            Height = ScoreBreakdownRow.RowHeight * this.rows.Count + ButtonHeight + Padding * 5;
        }

        private void setupBackground()
        {
            var background = new ColorRectangle(RenderLayer.UiBottom) {
                Width = Width,
                Height = ScoreBreakdownRow.RowHeight * this.rows.Count + Padding * 3
            };
            AttachChild(background);
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked if emitRemovedEvent == true &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        /// <param name="emitRemovedEvent">Whether to emit the Removed event</param>
        public override void CompleteRemoval(bool emitRemovedEvent = true)
        {
            base.CompleteRemoval(emitRemovedEvent);

            this.continueButton.Click -= this.onClick;
            if (this.ContinueClicked != null)
            {
                foreach (var subscriber in this.ContinueClicked.GetInvocationList())
                {
                    this.ContinueClicked -= subscriber as EventHandler;
                }
            }
        }

        private void onClick(object sender, EventArgs e)
        {
            this.ContinueClicked?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}