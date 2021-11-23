using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model.Nodes.Screens.Levels;
using SpaceInvaders.Model.Nodes.UI;

namespace SpaceInvaders.Model.Nodes.Screens
{
    /// <summary>
    ///     The main menu for the game.<br />
    ///     Provides different options for the player to pick from.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screen" />
    public class MainMenu : Screen
    {
        private const double ButtonWidth = 256;

        private Button playButton;
        private Button scoreboardButton;
        private Button optionsButton;
        private Button quitButton;
        private List<Button> buttons;
        private SoundPlayer buttonSound;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        {
            this.setupButtons();
            this.setupSoundPlayer();
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
                this.buttons[i].Y = i * 48;
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
            this.AttachChild(this.buttonSound);
        }

        private void onPlayClick(object sender, EventArgs e)
        {
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

        private void onQuitClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onButtonMouseEntered(object sender, Vector2 e)
        {
            this.buttonSound.Play();
        }

    }
}
