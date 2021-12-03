// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI.Xaml.Shapes;

namespace SpaceInvaders.View.Sprites.PowerUps
{
    /// <summary>
    ///     Draws a reflective shield
    /// </summary>
    /// <seealso cref="SpaceInvaders.View.Sprites.BaseSprite" />
    public sealed partial class ReflectiveShieldSprite
    {
        #region Properties

        /// <summary>
        ///     Gets the shield shape.
        /// </summary>
        /// <value>
        ///     The shield shape.
        /// </value>
        public Polygon ShieldShape => this.shieldShape;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReflectiveShieldSprite" /> class.
        /// </summary>
        public ReflectiveShieldSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}