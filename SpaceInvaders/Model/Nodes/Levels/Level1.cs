using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Levels
{
    public class Level1 : LevelBase
    {
        public override void Initialize(Canvas background)
        {
            Timer enemyMoveTimer = new Timer();
            enemyMoveTimer.Start();
            enemyMoveTimer.Tick += this.onEnemyMoveTimerTick;
            AttachChild(enemyMoveTimer);

            Entity sprite = new PlayerShip();
            sprite.X = 200;
            sprite.Y = MainPage.ApplicationHeight - 64;
            AttachChild(sprite);

        private void onEnemyMoveTimerTick(object sender, EventArgs e)
        {
            foreach (var child in this.enemies.Children)
            {
                if (child is Enemy enemy)
                {
                    enemy.X += 10;
                }
            }
        }
    }
}
