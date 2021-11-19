// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites.Entities.Enemies
{
    public sealed partial class BonusEnemySprite
    {
        #region Data members

        private readonly string ImageSource;

        #endregion

        #region Constructors

        public BonusEnemySprite()
        {
            this.InitializeComponent();
            this.ImageSource = @"https://cdn.betterttv.net/emote/60a9075967644f1d67e8ae60/1x";
        }

        #endregion
    }
}