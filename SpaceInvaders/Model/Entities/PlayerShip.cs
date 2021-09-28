using Windows.System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     Manages the player ship.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Entities.GameObject" />
    public class PlayerShip : GameObject
    {
        #region Data members

        private int moveSpeed = 100;
        private Vector2 velocity;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip()
        {
            Sprite = new PlayerShipSprite();
            this.velocity = new Vector2();
            this.CollisionLayer = (int) PhysicsLayer.Player;
            this.CollisionMask = (int) PhysicsLayer.EnemyHitbox;
        }

        #endregion


        #region Methods

        public override void Update(double delta)
        {
            handleMovement(delta);
        }

        private void handleMovement(double delta)
        {
            double moveDistance = 0;

            if (Input.IsKeyPressed(VirtualKey.Left))
            {
                moveDistance -= 1;
            }

            if (Input.IsKeyPressed(VirtualKey.Right))
            {
                moveDistance += 1;
            }

            if (moveDistance != 0)
            {
                moveDistance *= this.moveSpeed * delta;
                this.velocity.X = moveDistance;

                this.Move(velocity);
            }
        }

        #endregion
    }
}
