using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Nodes.UI;
using SpaceInvaders.View;
using Button = SpaceInvaders.Model.Nodes.UI.Button;

namespace SpaceInvaders.Model.Nodes.Screens
{
    /// <summary>
    ///     Displays a sortable high score table
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screen" />
    public class HighScoresMenu : Screen
    {
        #region Data members

        private const string HighScoreFilePath = "Scores.csv";
        private const string DefaultScoreFilePath = "Data/DefaultScores.csv";
        private const int TopPadding = 48;
        private const int ButtonPadding = 15;
        private const int ButtonWidth = 90;
        private const int ButtonHeight = 32;

        private readonly List<ScoreEntry> entries;
        private HighScoreBoard scoreBoard;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScoresMenu" /> class.
        /// </summary>
        public HighScoresMenu()
        {
            this.entries = new List<ScoreEntry>();
            this.scoreBoard = new HighScoreBoard();

            this.initialize();
        }

        #endregion

        #region Methods

        private async void initialize()
        {
            this.fillEntriesWithDummies();
            await this.loadHighScores();

            if (SessionStats.Level > 0 && SessionStats.Score > this.entries.Last().Score)
            {
                this.handleNewHighScore();
            }
            else
            {
                this.setupUi();
            }
        }

        private async Task loadHighScores()
        {
            try
            {
                var storageFolder = ApplicationData.Current.LocalFolder;
                var file = await storageFolder.GetFileAsync(HighScoreFilePath);

                var text = await FileIO.ReadTextAsync(file);
                var lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

                this.entries.Clear();

                foreach (var line in lines)
                {
                    var data = line.Split(",");
                    var name = data[0];
                    var score = int.Parse(data[1]);
                    var level = int.Parse(data[2]);

                    var scoreEntry = new ScoreEntry(name, score, level);
                    this.entries.Add(scoreEntry);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading high scores:");
                Debug.WriteLine(e.Message);
                Debug.WriteLine("Loading default scores");
                this.loadDefaultScores();
            }

            this.fillEntriesWithDummies();
            this.sortEntries();
        }

        private void loadDefaultScores()
        {
            this.entries.Clear();

            try
            {
                using (var reader = new StreamReader(DefaultScoreFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var data = line.Split(",");
                        var name = data[0];
                        var score = int.Parse(data[1]);
                        var level = int.Parse(data[2]);

                        var scoreEntry = new ScoreEntry(name, score, level);
                        this.entries.Add(scoreEntry);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading default scores:");
                Debug.WriteLine(e.Message);
            }
        }

        private void fillEntriesWithDummies()
        {
            while (this.entries.Count < 10)
            {
                this.entries.Add(new ScoreEntry("dummy", 0, 0));
            }
        }

        private async void handleNewHighScore()
        {
            var name = await promptForName();

            var entry = new ScoreEntry(name, SessionStats.Score, SessionStats.Level);

            this.entries.Add(entry);
            this.sortEntries();
            this.entries.RemoveAt(this.entries.Count - 1);

            await this.saveScores();

            this.setupUi();
        }

        private static async Task<string> promptForName()
        {
            var name = "";
            var nameTextBox = new TextBox();
            var nameDialog = new ContentDialog {
                Title = "Enter Name",
                PrimaryButtonText = "Enter",
                Content = nameTextBox
            };

            while (string.IsNullOrWhiteSpace(name))
            {
                await nameDialog.ShowAsync();
                name = nameTextBox.Text;
            }

            return name;
        }

        private async Task saveScores()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = await storageFolder.CreateFileAsync(HighScoreFilePath,
                CreationCollisionOption.ReplaceExisting);
            try
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var output = stream.GetOutputStreamAt(0))
                    {
                        using (var dataWriter = new DataWriter(output))
                        {
                            foreach (var entry in this.entries)
                            {
                                dataWriter.WriteString($"{entry.Name},{entry.Score},{entry.Level}\n");
                            }

                            await dataWriter.StoreAsync();
                            await output.FlushAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error reading high scores:");
                Debug.WriteLine(e.Message);
            }
        }

        private void setupUi()
        {
            this.setupScoreBoard();
            this.setupButtons();
            this.orderScoreBoardByScore();
        }

        private void setupScoreBoard()
        {
            this.scoreBoard = new HighScoreBoard(RenderLayer.UiMiddle) {
                Y = TopPadding
            };
            this.scoreBoard.Left = MainPage.ApplicationWidth / 2 - this.scoreBoard.Width / 2;

            var background = new ColorRectangle(RenderLayer.UiBottom) {
                Width = this.scoreBoard.Width,
                Height = this.scoreBoard.Height,
                Position = this.scoreBoard.Position,
                Color = Color.FromArgb(128, 80, 80, 80)
            };

            AttachChild(this.scoreBoard);
            AttachChild(background);
        }

        private void setupButtons()
        {
            var scoreButton = new Button("Score");
            var nameButton = new Button("Name");
            var levelButton = new Button("Level");
            var returnButton = new Button("Return to Menu") {
                Width = ButtonWidth * 3 + ButtonPadding * 2,
                Height = ButtonHeight
            };

            var sortButtons = new[] {
                scoreButton,
                nameButton,
                levelButton
            };

            var position = new Vector2(this.scoreBoard.Left, this.scoreBoard.Bottom + ButtonPadding);
            foreach (var button in sortButtons)
            {
                button.Position = position;
                button.Width = ButtonWidth;
                button.Height = ButtonHeight;
                position.X += ButtonWidth + ButtonPadding;
                AttachChild(button);
            }

            returnButton.Position = new Vector2(scoreButton.Left, scoreButton.Bottom + ButtonPadding);

            scoreButton.Click += this.onScoreButtonClick;
            nameButton.Click += this.onNameButtonClick;
            levelButton.Click += this.onLevelButtonClick;
            returnButton.Click += this.onReturnButtonClick;

            AttachChild(returnButton);
        }

        private void sortEntries()
        {
            this.entries.Sort((entry1, entry2) => entry2.Score - entry1.Score);
        }

        private void orderScoreBoardByScore()
        {
            var listView = this.scoreBoard.ScoreBoardListView;
            listView.ItemsSource = this.entries.OrderByDescending(entry => entry.Score)
                                       .ThenBy(entry => entry.Name)
                                       .ThenByDescending(entry => entry.Level);
        }

        private void orderScoreBoardByName()
        {
            var listView = this.scoreBoard.ScoreBoardListView;
            listView.ItemsSource = this.entries.OrderBy(entry => entry.Name)
                                       .ThenByDescending(entry => entry.Score)
                                       .ThenByDescending(entry => entry.Level);
        }

        private void orderScoreBoardByLevel()
        {
            var listView = this.scoreBoard.ScoreBoardListView;
            listView.ItemsSource = this.entries.OrderByDescending(entry => entry.Level)
                                       .ThenByDescending(entry => entry.Score)
                                       .ThenBy(entry => entry.Name);
        }

        private void onScoreButtonClick(object sender, EventArgs e)
        {
            this.orderScoreBoardByScore();
        }

        private void onNameButtonClick(object sender, EventArgs e)
        {
            this.orderScoreBoardByName();
        }

        private void onLevelButtonClick(object sender, EventArgs e)
        {
            this.orderScoreBoardByLevel();
        }

        private void onReturnButtonClick(object sender, EventArgs e)
        {
            CompleteScreen(typeof(MainMenu));
        }

        #endregion
    }
}