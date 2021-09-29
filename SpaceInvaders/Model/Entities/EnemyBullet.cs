using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class EnemyBullet : Bullet
    {
        public EnemyBullet(GameManager parent) : base(parent)
        {
            this.Speed.Y = 500;
            this.CollisionMasks = (int)PhysicsLayer.Player;
            this.CollisionLayers = (int)PhysicsLayer.EnemyHitbox;
        }
    }
}
