using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class PlayerBullet : Bullet
    {
        public PlayerBullet(GameManager parent) : base(parent)
        {
            this.Speed.Y = -1000;
            this.CollisionMasks = (int) PhysicsLayer.Enemy;
            this.CollisionLayers = (int) PhysicsLayer.PlayerHitbox;
        }
    }
}
