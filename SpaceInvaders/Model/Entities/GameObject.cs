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
        ///     Gets or sets the right edge of the GameObject.
        /// </summary>
        /// <value>
        ///     The right.
        /// </value>
        public double Right
        {
            get => this.X + this.Width;
            set => this.X = value - this.Width;
        }

        /// <summary>
        ///     Gets or sets the bottom edge of the GameObject.
        /// </summary>
        /// <value>
        ///     The bottom edge.
        /// </value>
        public double Bottom
        {
            get => this.Y + this.Height;
            set => this.Y = value - this.Height;
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

        /// <summary>
        ///     Gets or sets the collision layers.
        ///     Each bit of CollisionLayers represents a different layer, for a total of eight layers.
        /// 
        ///     If Monitorable is set to true, other GameObjects will check if any of their flagged
        ///     CollisionMask bits match this object's CollisionLayer when their bounding boxes overlap.
        /// </summary>
        /// <value>
        ///     The collision layers.
        /// </value>
        public byte CollisionLayers { get; set; }

        /// <summary>
        ///     Gets or sets the collision masks.
        ///     Each bit of CollisionMasks represents a different layer, for a total of eight layers.
        /// 
        /// </summary>
        /// <value>
        ///     The collision masks.
        /// </value>
        public byte CollisionMasks { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GameObject"/> is monitorable.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitorable; otherwise, <c>false</c>.
        /// </value>
        public bool Monitorable { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GameObject"/> is monitoring for collisions.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitoring; otherwise, <c>false</c>.
        /// </value>
        public bool Monitoring { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     The update loop for the GameObject.
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public abstract void Update(double delta);

        /// <summary>
        ///     Moves the specified distance.
        /// </summary>
        /// <param name="distance">The distance to move.</param>
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
