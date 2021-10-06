using System;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes
{
    public class SpriteNode : Area
    {
        private BaseSprite sprite;
        public BaseSprite Sprite => this.sprite;
        public override double X
        {
            get => base.X;
            set
            {
                base.X = value;
                this.render();
            }
        }
        public override double Y
        {
            get => base.Y;
            set
            {
                base.Y = value;
                this.render();
            }
        }
        public override double Width => this.sprite.Width;
        public override double Height => this.sprite.Height;

        public SpriteNode(BaseSprite sprite)
        {
            this.sprite = sprite;

            SpriteAdded?.Invoke(this, this.sprite);
        }

        public override void CompleteRemoval()
        {
            base.CompleteRemoval();
            SpriteRemoved?.Invoke(null, this.sprite);
        }

        public static event EventHandler<BaseSprite> SpriteAdded;
        public static event EventHandler<BaseSprite> SpriteRemoved;

        private void render()
        {
            var render = this.sprite as ISpriteRenderer;

            render?.RenderAt(this.X, this.Y);
        }
        public void ChangeSprite(BaseSprite newSprite)
        {
            SpriteRemoved?.Invoke(this, this.sprite);
            this.sprite = newSprite;
            SpriteAdded?.Invoke(this, this.sprite);
            this.render();
        }

    }
}
