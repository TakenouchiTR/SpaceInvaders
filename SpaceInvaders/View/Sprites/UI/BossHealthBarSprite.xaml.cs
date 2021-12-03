// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI.Xaml.Shapes;

namespace SpaceInvaders.View.Sprites.UI
{
    /// <summary>
    ///     Draws the boss' health bar
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public sealed partial class BossHealthBarSprite
    {
        #region Properties

        /// <summary>
        ///     Gets the health bar.
        /// </summary>
        /// <value>
        ///     The health bar.
        /// </value>
        public Rectangle HealthBar => this.healthBar;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossHealthBarSprite" /> class.
        /// </summary>
        public BossHealthBarSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}