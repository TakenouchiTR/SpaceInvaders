using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Screens.Levels
{
    /// <summary>
    ///     The fourth level of the game.<br />
    ///     Has the boss fight.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Screens.Levels.LevelBase" />
    public class Level4 : LevelBase
    {
        #region Data members

        private const double BossStartY = 128;

        private Boss boss;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level1" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Children.Count == 4
        /// </summary>
        public Level4() : base(typeof(HighScoresMenu))
        {
            this.addBoss();
        }

        #endregion

        #region Methods

        private void addBoss()
        {
            this.boss = new Boss {
                Center = new Vector2(MainPage.ApplicationWidth / 2, BossStartY)
            };

            RegisterEnemy(this.boss);
            AttachChild(this.boss);
        }

        #endregion
    }
}