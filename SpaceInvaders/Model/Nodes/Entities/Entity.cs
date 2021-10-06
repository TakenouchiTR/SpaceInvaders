using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    public class Entity : Node2D
    {
        public SpriteNode Sprite { get; protected set; }
        public CollisionArea Collision { get; protected set; }
        
        public Entity(BaseSprite sprite)
        {
            this.Sprite = new SpriteNode(sprite);
            this.Collision = new CollisionArea()
            {
                Width = this.Sprite.Width,
                Height = this.Sprite.Height
            };

            this.AttachChild(this.Sprite);
            this.AttachChild(this.Collision);
        }
    }
}
