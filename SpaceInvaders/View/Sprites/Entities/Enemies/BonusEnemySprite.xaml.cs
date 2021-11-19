using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites.Entities.Enemies
{
    public sealed partial class BonusEnemySprite
    {
        private readonly String ImageSource;

        public BonusEnemySprite()
        {
            this.InitializeComponent();
            this.ImageSource = @"https://cdn.betterttv.net/emote/60a9075967644f1d67e8ae60/1x";
        }
    }
}
