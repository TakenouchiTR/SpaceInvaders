using System;
using Windows.Foundation;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     Defines basics of every game object.
    /// </summary>
    public abstract class GameObject
    {
        #region Data members

        private Point location;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the x location of the game object.
        /// </summary>
        /// <value>
        ///     The x.
        /// </value>
        public double X
        {
            get => this.location.X;
            set
            {
                this.location.X = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets or sets the y location of the game object.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public double Y
        {
            get => this.location.Y;
            set
            {
                this.location.Y = value;
                this.render();
            }
        }
        
        /// <summary>
        ///     Gets the width of the game object.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public double Width => this.Sprite.Width;

        /// <summary>
        ///     Gets the height of the game object.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public double Height => this.Sprite.Height;

        /// <summary>
        ///     Gets or sets the sprite associated with the game object.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite { get; protected set; }

        public byte CollisionLayers { get; set; }

        public byte CollisionMasks { get; set; }

        public bool Monitorable { get; set; }

        public bool Monitoring { get; set; }

        #endregion

        #region Methods

        public abstract void Update(double delta);

        public void Move(Vector2 distance)
        {
            this.X += distance.X;
            this.Y += distance.Y;
        }

        private void render()
        {
            var render = this.Sprite as ISpriteRenderer;

            render?.RenderAt(this.X, this.Y);
        }
        
        #endregion
    }
}
