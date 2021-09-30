﻿using System;
using System.Drawing;
using SpaceInvaders.View.Sprites;
using Point = Windows.Foundation.Point;

namespace SpaceInvaders.Model.Entities
{
    /// <summary>
    ///     Defines basics of every game object.
    /// </summary>
    public abstract class GameObject
    {
        #region Data members

        protected GameManager parent;
        private Rectangle collisionBox;
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
                this.collisionBox.X = (int) value;
                this.render();
                this.Moved?.Invoke(this, EventArgs.Empty);
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
                this.collisionBox.Y = (int) value;
                this.render();
                this.Moved?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the position of the top-left corner.
        /// </summary>
        /// <value>
        ///     The position of the top-left corner.
        /// </value>
        public Vector2 Position
        {
            get => new Vector2(this.X, this.Y);
            set
            {
                this.X = value.X;
                this.Y = value.Y;
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
        ///     Gets or sets the center of the GameObject.
        /// </summary>
        /// <value>
        ///     The center of the GameObject.
        /// </value>
        public Vector2 Center
        {
            get
            {
                var center = new Vector2 {
                    X = this.X + this.Width / 2,
                    Y = this.Y + this.Height / 2
                };
                return center;
            }
            set
            {
                this.X = value.X - this.Width / 2;
                this.Y = value.Y - this.Height / 2;
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

        /// <summary>
        ///     Gets or sets the collision layers.
        ///     Each bit of CollisionLayers represents a different layer, for a total of 32 layers.
        ///     If Monitorable is set to true, other GameObjects will check if any of their flagged
        ///     CollisionMask bits match this object's CollisionLayer when their bounding boxes overlap.
        /// </summary>
        /// <value>
        ///     The collision layers.
        /// </value>
        public int CollisionLayers { get; set; }

        /// <summary>
        ///     Gets or sets the collision masks.
        ///     Each bit of CollisionMasks represents a different layer, for a total of 32 layers.
        /// </summary>
        /// <value>
        ///     The collision masks.
        /// </value>
        public int CollisionMasks { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GameObject" /> is monitorable.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitorable; otherwise, <c>false</c>.
        /// </value>
        public bool Monitorable { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GameObject" /> is monitoring for collisions.
        /// </summary>
        /// <value>
        ///     <c>true</c> if monitoring; otherwise, <c>false</c>.
        /// </value>
        public bool Monitoring { get; set; }

        /// <summary>
        ///     Gets or sets the collision box.
        /// </summary>
        /// <value>
        ///     The collision box.
        /// </value>
        public Rectangle CollisionBox
        {
            get => this.collisionBox;
            protected set => this.collisionBox = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameObject" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="sprite">The sprite.</param>
        protected GameObject(GameManager parent, BaseSprite sprite)
        {
            this.Sprite = sprite;
            this.parent = parent;

            this.collisionBox.Width = (int) this.Width;
            this.collisionBox.Height = (int) this.Height;
        }

        #endregion

        #region Methods

        public event EventHandler Removed;
        public event EventHandler Moved;

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
            this.collisionBox.X = (int) this.X;
            this.collisionBox.Y = (int) this.Y;
        }

        /// <summary>
        ///     Queues the objects removal at the end of the update tick.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public void QueueRemoval()
        {
            this.parent.QueueGameObjectForRemoval(this);
        }

        /// <summary>
        ///     Runs cleanup code and invokes the Removed event when removed from the game.
        ///     Precondition: None
        ///     Postcondition: Removed event is invoked
        /// </summary>
        public void CompleteRemoval()
        {
            this.Removed?.Invoke(this, EventArgs.Empty);
        }

        private void render()
        {
            var render = this.Sprite as ISpriteRenderer;

            render?.RenderAt(this.X, this.Y);
        }

        /// <summary>
        ///     Determines whether the object is colliding with another object.
        ///     The object checks if the target has at least one CollisionLayer that matches the object's CollisionMasks
        ///     Precondition: target != null
        ///     Postcondition: None
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentException">target must not be null</exception>
        /// <returns>
        ///     <c>true</c> if [is colliding with] [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCollidingWith(GameObject target)
        {
            if (target == null)
            {
                throw new ArgumentException("target must not be null");
            }

            if (!this.Monitoring || !target.Monitorable)
            {
                return false;
            }

            if (this.isMaskingTargetCollisionLayers(target))
            {
                return this.collisionBox.IntersectsWith(target.CollisionBox);
            }

            return false;
        }

        private bool isMaskingTargetCollisionLayers(GameObject target)
        {
            return (this.CollisionMasks & target.CollisionLayers) != 0;
        }

        /// <summary>
        ///     Handles when the object collides with the target object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentException">target must not be null</exception>
        public virtual void HandleCollision(GameObject target)
        {
            if (target == null)
            {
                throw new ArgumentException("target must not be null");
            }
        }

        /// <summary>
        ///     Determines whether [is off screen].
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is off screen]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOffScreen()
        {
            return this.X > this.parent.ScreenWidth || this.Right < 0 || this.Y > this.parent.ScreenHeight ||
                   this.Bottom < 0;
        }

        /// <summary>
        ///     Determines whether [is on screen].
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is on screen]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOnScreen()
        {
            return !this.IsOffScreen();
        }

        #endregion
    }
}