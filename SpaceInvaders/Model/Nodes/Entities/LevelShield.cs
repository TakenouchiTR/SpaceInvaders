using System;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     An arrangement of shield segments for use in a level
    /// </summary>
    internal class LevelShield : Area
    {
        #region Data members

        private const int ShieldSegmentBorderThickness = 2;
        private const int ShieldSegmentVerticalPadding = 5;

        private readonly int totalShieldSegments;
        private readonly int rows;
        private double shieldSegmentWidth;
        private double shieldSegmentHeight;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelShield" /> class.
        ///     Precondition: numberShieldSegments > 0 AND numberRows > 0
        /// </summary>
        /// <param name="numberShieldSegments">the maximum number of shield segments in a row</param>
        /// <param name="numberRows">the number of shield segment rows</param>
        public LevelShield(int numberShieldSegments, int numberRows)
        {
            if (numberShieldSegments <= 0)
            {
                throw new ArgumentException("Must be at least 1 shield segment per row");
            }

            if (numberRows <= 0)
            {
                throw new ArgumentException("Must be at least 1 row");
            }

            this.totalShieldSegments = numberShieldSegments;
            this.rows = numberRows;

            this.setupShieldSegments();
            this.setAreaSize();
        }

        #endregion

        #region Methods

        private void setupShieldSegments()
        {
            var shieldSegments = this.createShieldSegments();
            this.shieldSegmentWidth = shieldSegments[0].Width;
            this.shieldSegmentHeight = shieldSegments[0].Height;

            this.positionAndAttachShieldSegments(shieldSegments);
        }

        private ShieldSegment[] createShieldSegments()
        {
            var totalSegments = this.totalShieldSegments * this.rows - this.rows / 2;
            var shieldSegments = new ShieldSegment[totalSegments];

            for (var index = 0; index < shieldSegments.Length; index++)
            {
                shieldSegments[index] = new ShieldSegment();
            }

            return shieldSegments;
        }

        private void positionAndAttachShieldSegments(ShieldSegment[] shieldSegments)
        {
            for (var row = 0; row < this.rows; row++)
            {
                var numberSegmentsThisRow =
                    this.isRowEven(row) ? this.totalShieldSegments : this.totalShieldSegments - 1;
                var xpos = this.isRowEven(row) ? 0 : (this.shieldSegmentWidth - ShieldSegmentBorderThickness) / 2;
                var ypos = row * this.shieldSegmentHeight;

                if (row > 0)
                {
                    ypos -= row * ShieldSegmentVerticalPadding;
                }

                for (var shieldSegmentIndex = 0; shieldSegmentIndex < numberSegmentsThisRow; shieldSegmentIndex++)
                {
                    var shieldSegment = shieldSegments[shieldSegmentIndex + row * this.totalShieldSegments - row / 2];
                    shieldSegment.Removed += this.onSegmentDestroyed;
                    AttachChild(shieldSegment);

                    shieldSegment.X = xpos;
                    shieldSegment.Y = ypos;

                    xpos += shieldSegment.Width - ShieldSegmentBorderThickness;
                }
            }
        }

        private bool isRowEven(int row)
        {
            return row % 2 == 0;
        }

        private void onSegmentDestroyed(object sender, EventArgs e)
        {
            if (Children.Count == 0)
            {
                QueueForRemoval();
            }
        }

        private void setAreaSize()
        {
            Width = this.totalShieldSegments * this.shieldSegmentWidth;
            Height = this.rows * this.shieldSegmentHeight;
        }

        #endregion
    }
}