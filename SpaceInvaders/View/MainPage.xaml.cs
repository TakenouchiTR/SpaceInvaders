﻿using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
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

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;

            this.gameManager = new GameManager(ApplicationHeight, ApplicationWidth);
            this.gameManager.InitializeGame(this.theCanvas);
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.MovePlayerShipLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.MovePlayerShipRight();
                    break;
            }
        }
    }
}
