using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    /// <summary>
    ///     An intermediate enemy which has the same behavior as a basic enemy, but is worth more points.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.Enemies.Enemy" />
    public class IntermediateEnemy : Enemy
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="IntermediateEnemy" /> class.
        /// </summary>
        /// <param name="manager">The Manager.</param>
        public IntermediateEnemy(GameManager manager) : base(manager, new IntermediateEnemySprite())
        {
            Score = 20;
        }

        #endregion

        #region Methods

        public override void Update(double delta)
        {
        }

        #endregion
    }
}