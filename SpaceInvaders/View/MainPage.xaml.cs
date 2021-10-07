using System;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model;
using SpaceInvaders.Model.Nodes;
using SpaceInvaders.Model.Nodes.Levels;
using SpaceInvaders.View.Sprites;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceInvaders.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const double ApplicationHeight = 480;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 640;

        private readonly LevelBase level;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            Window.Current.CoreWindow.KeyDown += Input.OnKeyDown;
            Window.Current.CoreWindow.KeyUp += Input.OnKeyUp;

            SpriteNode.SpriteHidden += this.onSpriteNodeHidden;
            SpriteNode.SpriteShown += this.onSpriteNodeShown;

            this.level = new Level1();
            this.level.ScoreChanged += this.onLevelScoreChanged;
            this.level.GameFinished += this.onLevelGameFinished;
        }

        #endregion

        #region Methods

        private void onSpriteNodeHidden(object sender, BaseSprite e)
        {
            this.theCanvas.Children.Remove(e);
        }

        private void onSpriteNodeShown(object sender, BaseSprite e)
        {
            this.theCanvas.Children.Add(e);
        }

        private async void onLevelGameFinished(object sender, string e)
        {
            this.level.GameFinished -= this.onLevelGameFinished;

            var gameOverDialog = new ContentDialog {
                Title = "Game Over",
                Content = e,
                PrimaryButtonText = "Exit to Desktop"
            };

            await gameOverDialog.ShowAsync();

            CoreApplication.Exit();
        }

        private void onLevelScoreChanged(object sender, int e)
        {
            this.scoreText.Text = $"Score: {e}";
        }

        #endregion
    }
}