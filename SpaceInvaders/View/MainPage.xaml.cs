using System;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceInvaders.View
{
    /// <summary>
    /// The main page for the game.
    /// </summary>
    public sealed partial class MainPage
    {
        /// <summary>
        ///     The application height
        /// </summary>
        public const double ApplicationHeight = 480;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 640;

        private readonly GameManager gameManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size { Width = ApplicationWidth, Height = ApplicationHeight };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
            
            Window.Current.CoreWindow.KeyDown += Input.OnKeyDown;
            Window.Current.CoreWindow.KeyUp += Input.OnKeyUp;

            this.gameManager = new GameManager(ApplicationHeight, ApplicationWidth);
            this.gameManager.InitializeGame(this.theCanvas);
            this.gameManager.ScoreUpdated += onGameManagerScoreUpdated;
            this.gameManager.GameFinished += onGameManagerGameFinished;
        }

        private async void onGameManagerGameFinished(object sender, string e)
        {
            this.gameManager.GameFinished -= this.onGameManagerGameFinished;

            var gameOverDialog = new ContentDialog
            {
                Title = "Game Over",
                Content = e,
                PrimaryButtonText = "Exit to Desktop"
            };

            await gameOverDialog.ShowAsync();

            CoreApplication.Exit();
        }

        private void onGameManagerScoreUpdated(object sender, EventArgs e)
        {
            this.scoreText.Text = $"Score: {this.gameManager.Score}";
        }
    }
}
