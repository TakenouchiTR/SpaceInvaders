using SpaceInvaders.View.Sprites.Entities;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A wave from the nuke that the boss uses to destroy shields
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class NukeWave : Entity
    {
        #region Data members

        private const double MoveSpeed = 360;

        private readonly int moveFactor;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NukeWave" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        /// <param name="moveFactor">The move factor.</param>
        public NukeWave(int moveFactor) : base(new NukeWaveSprite())
        {
            this.moveFactor = moveFactor;
            if (this.moveFactor > 0)
            {
                var nukeWaveSprite = (NukeWaveSprite) Sprite.Sprite;

                nukeWaveSprite.Rotation = 180;
            }

            Collision.CollisionLayers = PhysicsLayer.PlayerHitbox;
            Collision.Monitorable = true;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            Move(Vector2.Right * MoveSpeed * this.moveFactor * delta);

            if (IsOffScreen())
            {
                QueueForRemoval();
            }

            base.Update(delta);
        }

        #endregion
    }
}