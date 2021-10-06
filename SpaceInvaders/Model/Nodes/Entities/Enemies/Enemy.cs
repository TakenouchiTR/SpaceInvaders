using System;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    public abstract class Enemy : Entity
    {
        public int Score { get; protected set; }

        protected Enemy(BaseSprite sprite) : base(sprite)
        {
            this.Collision.CollisionLayers = PhysicsLayer.Enemy;
            this.Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            this.Collision.Monitoring = true;
            this.Collision.Monitorable = true;
            this.Score = 0;
        }

        public override void CompleteRemoval()
        {
            Explosion explosion = new Explosion() 
            {
                Center = this.Center
            };
            this.GetRoot().QueueGameObjectForAddition(explosion);
            base.CompleteRemoval();
        }
    }
}
