using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model;
using SpaceInvaders.Model.Nodes;
using SpaceInvaders.Model.Nodes.Screens;
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
        public const double ApplicationHeight = 620;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 960;

        private Screen currentScreen;
        private Canvas[] canvasLayers;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            Application.Current.RequestedTheme = ApplicationTheme.Dark;

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            Window.Current.CoreWindow.KeyDown += Input.OnKeyDown;
            Window.Current.CoreWindow.KeyUp += Input.OnKeyUp;

            RenderableNode.SpriteHidden += this.onSpriteNodeHidden;
            RenderableNode.SpriteShown += this.onSpriteNodeShown;

            this.createAndPopulateCanvasLayers();

            this.setupScreen(new MainMenu());
        }

        #endregion

        #region Methods

        private void createAndPopulateCanvasLayers()
        {
            this.canvasLayers = new Canvas[Enum.GetValues(typeof(RenderLayer)).Length];

            for (var i = 0; i < this.canvasLayers.Length; i++)
            {
                var canvas = new Canvas {
                    Width = ApplicationWidth,
                    Height = ApplicationHeight
                };

                this.theCanvas.Children.Add(canvas);
                this.canvasLayers[i] = canvas;
            }
        }

        private void setupScreen(Screen screen)
        {
            this.currentScreen = screen;
            this.currentScreen.Complete += this.onCurrentScreenComplete;
        }

        private void cleanupScreen()
        {
            if (this.currentScreen == null)
            {
                return;
            }

            this.currentScreen.CleanupScreen();

            this.currentScreen.Complete -= this.onCurrentScreenComplete;
            this.currentScreen = null;
            GC.Collect();
        }

        private void onSpriteNodeHidden(object sender, BaseSprite e)
        {
            if (sender is RenderableNode node && e != null)
            {
                var layerIndex = (int) node.Layer;
                this.canvasLayers[layerIndex].Children.Remove(e);
            }
        }

        private void onSpriteNodeShown(object sender, BaseSprite e)
        {
            if (sender is RenderableNode node && e != null)
            {
                var layerIndex = (int) node.Layer;
                this.canvasLayers[layerIndex].Children.Add(e);
            }
        }

        private void onCurrentScreenComplete(object sender, Type e)
        {
            this.cleanupScreen();

            var constructor = e.GetConstructors()[0];
            var screen = (Screen) constructor.Invoke(new object[] { });
            this.setupScreen(screen);
        }

        #endregion
    }
}