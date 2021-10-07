using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    public class Entity : Area
    {
        #region Properties

        public SpriteNode Sprite { get; protected set; }
        public CollisionArea Collision { get; protected set; }

        #endregion

        #region Constructors

        public Entity(BaseSprite sprite)
        {
            this.Sprite = new SpriteNode(sprite);
            this.Collision = new CollisionArea {
                Width = this.Sprite.Width,
                Height = this.Sprite.Height
            };
            Width = this.Sprite.Width;
            Height = this.Sprite.Height;

            AttachChild(this.Sprite);
            AttachChild(this.Collision);
        }

        #endregion
    }
}