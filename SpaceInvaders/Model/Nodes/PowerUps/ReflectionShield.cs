using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.View.Sprites.PowerUps;

namespace SpaceInvaders.Model.Nodes.PowerUps
{
    /// <summary>
    ///     A protective shield that reflect enemy bullets
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.CollisionArea" />
    public class ReflectionShield : Entity
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReflectionShield" /> class.
        /// </summary>
        public ReflectionShield() : base(new ReflectiveShieldSprite())
        {
            Collision.CollisionMasks = PhysicsLayer.EnemyHitbox;
            Collision.Monitoring = true;
            Collision.Collided += this.onCollided;
            AttachedToParent += this.onAttachedToParent;
        }

        #endregion

        #region Methods

        private void onAttachedToParent(object sender, Node e)
        {
            if (e is Area area)
            {
                Center = area.Center;
            }
        }

        private void onCollided(object sender, CollisionArea e)
        {
            if (e.Parent is Bullet bullet)
            {
                bullet.Collision.CollisionLayers = PhysicsLayer.PlayerHitbox;
                bullet.Collision.CollisionMasks = PhysicsLayer.Enemy | PhysicsLayer.World;
                bullet.Velocity *= -1;
            }
        }

        #endregion
    }
}