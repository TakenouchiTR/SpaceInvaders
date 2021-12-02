using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Nodes.Screens.Levels;
using SpaceInvaders.Model.Nodes.UI;
using SpaceInvaders.View;
using Button = SpaceInvaders.Model.Nodes.UI.Button;

namespace SpaceInvaders.Model.Nodes.Screens
{
    /// <summary>
    ///     The main menu for the game.<br />
    ///     Provides different options for the player to pick from.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screen" />
    public class MainMenu : Screen
    {
        #region Data members

        private const double ButtonWidth = 256;
        private const double ButtonStartY = 320;
        private const double ButtonSpacing = 48;

        private Button playButton;
        private Button scoreboardButton;
        private Button optionsButton;
        private Button quitButton;
        private List<Button> buttons;
        private SoundPlayer buttonSound;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainMenu" /> class.
        /// </summary>
        public MainMenu()
        {
            this.setupLabels();
            this.setupButtons();
            this.setupSoundPlayer();
        }

        #endregion

        #region Methods

        private void setupLabels()
        {
            var title = new Label("Space Invaders") {
                FontSize = 128,
                Y = 32,
                Width = MainPage.ApplicationWidth,
                Alignment = TextAlignment.Center
            };

            var subtitle = new Label("Team C") {
                FontSize = 64,
                Y = 192,
                Width = MainPage.ApplicationWidth,
                Alignment = TextAlignment.Center
            };

            AttachChild(title);
            AttachChild(subtitle);
        }

        private void setupButtons()
        {
            this.buttons = new List<Button>();

            this.playButton = new Button("Play", RenderLayer.UiMiddle);
            this.scoreboardButton = new Button("Scoreboard", RenderLayer.UiMiddle);
            this.optionsButton = new Button("Options", RenderLayer.UiMiddle);
            this.quitButton = new Button("Quit", RenderLayer.UiMiddle);

            this.buttons.Add(this.playButton);
            this.buttons.Add(this.scoreboardButton);
            this.buttons.Add(this.optionsButton);
            this.buttons.Add(this.quitButton);

            for (var i = 0; i < this.buttons.Count; i++)
            {
                this.buttons[i].Y = i * ButtonSpacing + ButtonStartY;
                this.buttons[i].X = MainPage.ApplicationWidth / 2 - ButtonWidth / 2;
                this.buttons[i].Width = ButtonWidth;
                this.buttons[i].MouseEntered += this.onButtonMouseEntered;
                AttachChild(this.buttons[i]);
            }

            this.playButton.Click += this.onPlayClick;
            this.scoreboardButton.Click += this.onScoreboardClick;
            this.optionsButton.Click += this.onOptionsClick;
            this.quitButton.Click += this.onQuitClick;
        }

        private void setupSoundPlayer()
        {
            this.buttonSound = new SoundPlayer("change_option_high.wav");
            AttachChild(this.buttonSound);
        }

        private void onPlayClick(object sender, EventArgs e)
        {
            SessionStats.Reset();
            CompleteScreen(typeof(Level1));
        }

        private void onScoreboardClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onOptionsClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void onQuitClick(object sender, EventArgs e)
        {
            var quitDialog = new ContentDialog {
                Title = "Quitting game",
                Content = "Are you sure that you want to quit to desktop?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            var dialogResult = await quitDialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                CoreApplication.Exit();
            }
        }

        private void onButtonMouseEntered(object sender, Vector2 e)
        {
            this.buttonSound.Play();
        }

        #endregion
    }
}