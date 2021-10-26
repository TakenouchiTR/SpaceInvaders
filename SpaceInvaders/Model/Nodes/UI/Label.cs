using System;
using SpaceInvaders.View.UI;

namespace SpaceInvaders.Model.Nodes.UI
{
    public class Label : SpriteNode
    {
        private LabelSprite labelSprite;

        public override double Width
        {
            get => this.labelSprite.TextWidth;
            set => this.labelSprite.TextWidth = value;
        }

        public override double Height
        {
            get => this.labelSprite.TextHeight;
            set => this.labelSprite.TextHeight = value;
        }

        public string Text
        {
            get => this.labelSprite.Text;
            set => this.labelSprite.Text = value;
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class without a BaseSprite.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer
        /// </summary>
        public Label() : this("", DefaultRenderLayer)
        {

        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class with a specified Sprite.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer &amp;&amp;<br />
        ///     this.Sprite == sprite &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="text">The default text.</param>
        public Label(string text) : this(text, DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpriteNode" /> class with a specified Sprite and RenderLayer.
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == layer &amp;&amp;<br />
        ///     this.Sprite == sprite &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="text">The default text.</param>
        /// <param name="layer">The Render layer.</param>
        public Label(string text, RenderLayer layer) : base(new LabelSprite(), layer)
        {
            this.labelSprite = (LabelSprite) Sprite;
            this.labelSprite.Text = text;
        }

    }
}
