using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A boilerplate class for extending to make basic entities, such as enemies or players
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Area" />
    public class Entity : Area
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the sprite.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public SpriteNode Sprite { get; protected set; }

        /// <summary>
        ///     Gets or sets the collision.
        /// </summary>
        /// <value>
        ///     The collision.
        /// </value>
        public CollisionArea Collision { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Entity" /> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
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