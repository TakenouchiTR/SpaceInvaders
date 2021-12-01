using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ReflectionShield() : base(new ReflectiveShieldSprite())
        {
            Collision.CollisionMasks = PhysicsLayer.EnemyHitbox;
            Collision.Monitoring = true;
            Collision.Collided += this.onCollided;
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
    }
}
