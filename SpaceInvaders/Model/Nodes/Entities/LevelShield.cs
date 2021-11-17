using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///   An arrangement of shield segments for use in a level
    /// </summary>
    class LevelShield : Area
    {
        #region Data members

        private const int ShieldSegmentBorderThickness = 2;
        private const int ShieldSegmentVerticalPadding = 5;
        private int ShieldSegments;
        private int Rows;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelShield" /> class.
        /// </summary>
        /// <param name="numberShieldSegments">the maximum number of shield segments in a row</param>
        /// <param name="numberRows">the number of shield segment rows</param>
        public LevelShield(int numberShieldSegments, int numberRows) : base(numberShieldSegments * (new ShieldSegment()).Width, numberRows * (new ShieldSegment()).Height)
        {
            this.ShieldSegments = numberShieldSegments;
            this.Rows = numberRows;

            
            this.addShieldSegments();
        }

        #endregion

        #region Methods

        private void addShieldSegments()
        {
            for (int row = 0; row < Rows; row++)
            {
                int numberSegmentsThisRow = this.isRowEven(row) ? this.ShieldSegments : this.ShieldSegments - 1;
                double xpos = this.isRowEven(row) ? 0 : ((new ShieldSegment()).Width - ShieldSegmentBorderThickness) / 2;
                double ypos = row * (new ShieldSegment()).Height;

                if (row > 0)
                {
                    ypos -= row * ShieldSegmentVerticalPadding;
                }

                for (int shieldSegmentIndex = 0; shieldSegmentIndex < numberSegmentsThisRow; shieldSegmentIndex++)
                {
                    var shieldSegment = new ShieldSegment();
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

        #endregion
    }
}
