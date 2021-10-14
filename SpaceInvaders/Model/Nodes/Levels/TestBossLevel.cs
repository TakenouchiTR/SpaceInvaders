using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Levels
{
    /// <summary>
    ///     A test level for fighting a test boss.<br />
    ///     Not intended for actual play.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Levels.LevelBase" />
    public class TestBossLevel : LevelBase
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestBossLevel" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 2
        /// </summary>
        public TestBossLevel()
        {
            this.addEnemies();
            this.addPlayer();
        }

        #endregion

        #region Methods

        private void addPlayer()
        {
            var player = new PlayerShip();
            player.X = MainPage.ApplicationWidth / 2 - player.Collision.Width / 2;
            player.Y = MainPage.ApplicationHeight - 64;
            AttachChild(player);
        }

        private void addEnemies()
        {
            AttachChild(new TestBoss());
        }

        #endregion
    }
}