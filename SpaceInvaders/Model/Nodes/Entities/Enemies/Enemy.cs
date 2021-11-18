using System;
using SpaceInvaders.Model.Nodes.Effects;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     The base class to derive enemies from
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public abstract class Enemy : Entity
    {
        #region Data members

        private static readonly Random EnemyRandom = new Random();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the score earned from killing the enemy.
        /// </summary>
        /// <value>
        ///     The score earned.
        /// </value>
        public int Score { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Enemy" /> class.<br />
        ///     Precondition: None
        ///     Postcondition: this.Collision.CollisionLayers == PhysicsLayer.Enemy &amp;&amp;<br />
        ///     this.Collision.CollisionMasks == PhysicsLayer.PlayerHitbox &amp;&amp;<br />
        ///     this.Collision.Monitorable == true &amp;&amp;<br />
        ///     this.Collision.Monitoring == true &amp;&amp;<br />
        ///     this.Sprite == sprite
        /// </summary>
        /// <param name="sprite">The enemy sprite.</param>
        protected Enemy(AnimatedSprite sprite) : base(sprite)
        {
            Collision.CollisionLayers = PhysicsLayer.Enemy;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            Collision.Monitoring = true;
            Collision.Monitorable = true;

            sprite.Stop();
            sprite.Visible = true;
            sprite.Frame = EnemyRandom.Next(sprite.FrameCount);
            Removed += this.onRemoved;
            Moved += this.onMoved;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the enemy of the specified type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An instance of the specified enemy.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">type - null</exception>
        public static Enemy CreateEnemy(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.BasicEnemy:
                    return new BasicEnemy();
                case EnemyType.IntermediateEnemy:
                    return new IntermediateEnemy();
                case EnemyType.AggressiveEnemy:
                    return new AggresiveEnemy();
                case EnemyType.MasterEnemy:
                    return new MasterEnemy();
                case EnemyType.BonusEnemy:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void onRemoved(object sender, EventArgs e)
        {
            var explosion = new Explosion {
                Center = Center
            };
            GetRoot().QueueNodeForAddition(explosion);
        }

        private void onMoved(object sender, Vector2 e)
        {
            if (Sprite is AnimatedSprite animatedSprite)
            {
                animatedSprite.Frame++;
            }
        }

        #endregion
    }
}