// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using SpaceInvaders.Model;

namespace SpaceInvaders.View.UI
{
    public sealed partial class LabelSprite
    {
        public string Text
        {
            get => this.textBlock.Text;
            set => this.textBlock.Text = value;
        }

        public double TextWidth 
        { 
            get => this.textBlock.Width;
            set => this.textBlock.Width = value;
        }

        public double TextHeight
        {
            get => this.textBlock.Height;
            set => this.textBlock.Height = value;
        }

        public Vector2 TextSize => new Vector2(this.TextWidth, this.TextHeight);

        public LabelSprite()
        {
            this.InitializeComponent();
        }
    }
}
