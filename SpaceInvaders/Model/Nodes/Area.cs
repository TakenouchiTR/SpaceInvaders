using System.Reflection.Metadata.Ecma335;

namespace SpaceInvaders.Model.Nodes
{
    public class Area : Node2D
    {
        public double Left => this.X;
        public double Right => this.X + this.Width;
        public double Top => this.Y;
        public double Bottom => this.Y + this.Height;
        public Vector2 Center
        {
            get => new Vector2(this.X + this.Width / 2, this.Y + this.Width / 2);
            set
            {
                this.X = value.X - this.Width / 2;
                this.Y = value.Y - this.Height / 2;
            }
        }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
