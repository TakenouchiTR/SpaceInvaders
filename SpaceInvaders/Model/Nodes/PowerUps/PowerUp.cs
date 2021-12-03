using SpaceInvaders.Model.Nodes.Entities;

namespace SpaceInvaders.Model.Nodes.PowerUps
{
    /// <summary>
    ///     A limited-time bonus that can be given to the player
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public abstract class PowerUp : Node
    {
        #region Methods

        /// <summary>
        ///     Runs the required code to attach and apply the power up to the player<br />
        ///     Precondition: player != null<br />
        ///     Postcondition: player has the power up applied
        /// </summary>
        /// <param name="player">The player.</param>
        public abstract void AttachToPlayer(PlayerShip player);

        /// <summary>
        ///     Removes the power up from player.<br />
        ///     Precondition: None<br />
        ///     Postcondition: The power up is removed
        /// </summary>
        public abstract void RemoveFromPlayer();

        #endregion
    }
}