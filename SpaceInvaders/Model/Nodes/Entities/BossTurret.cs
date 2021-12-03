using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     Controls a turret mounted on the boss
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public abstract class BossTurret : Entity
    {
        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BossTurret" /> is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossTurret" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        protected BossTurret() : base(new BossTurretSprite())
        {
        }

        #endregion
    }
}