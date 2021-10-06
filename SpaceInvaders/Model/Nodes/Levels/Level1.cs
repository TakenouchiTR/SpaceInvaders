using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Levels
{
    public class Level1 : LevelBase
    {
        public override void Initialize(Canvas background)
        {
            Entity sprite = new PlayerShip();
            sprite.X = 200;
            sprite.Y = MainPage.ApplicationHeight - 64;
            AttachChild(sprite);
        }
    }
}
