using SpaceInvaders.View.Sprites.UI;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Draws a bar that displays the player's current graze meter
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.RenderableNode" />
    public class GrazeBar : RenderableNode
    {
        #region Data members

        private readonly GrazeBarFrameSprite barSprite;

        #endregion

        #region Properties

        /// <summary>
        ///     Sets the height of the bar as a percent of the max height.
        /// </summary>
        /// <value>
        ///     The height of the bar.
        /// </value>
        public double BarHeight
        {
            set => this.barSprite.BarHeight = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrazeBar" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == RenderableNode.DefaultRenderLayer
        /// </summary>
        public GrazeBar() : this(DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrazeBar" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == layer
        /// </summary>
        /// <param name="layer">The layer.</param>
        public GrazeBar(RenderLayer layer) : base(new GrazeBarFrameSprite(), layer)
        {
            this.barSprite = (GrazeBarFrameSprite) Sprite;
        }

        #endregion
    }
}