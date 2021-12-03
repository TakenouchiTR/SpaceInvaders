using System;
using System.Collections.Generic;
using Windows.UI.Notifications;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     The weak point for the boss
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class BossWeakPoint : Entity
    {
        #region Data members
        
        private const double SecondsPerRotation = 4;
        private const int BaseHealth = 3;

        private int health;
        private double rotationProgress;
        private readonly Boss boss;
        private Gun gun;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BossWeakPoint" /> is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BossWeakPoint" /> is mobile.
        /// </summary>
        /// <value>
        ///     <c>true</c> if mobile; otherwise, <c>false</c>.
        /// </value>
        public bool Mobile { get; set; }

        /// <summary>
        ///     Gets or sets the rotation progress.
        /// </summary>
        /// <value>
        ///     The rotation progress.
        /// </value>
        public double RotationProgress
        {
            get => this.rotationProgress;
            set
            {
                this.rotationProgress = value - (int) value;
                var xOffset = Math.Cos(Math.PI * 2 * this.rotationProgress) * this.boss.Width / 2;
                Center = new Vector2(this.boss.Center.X + xOffset, Center.Y);

                if (this.RotationProgress < .5)
                {
                    Sprite.Layer = RenderLayer.MainLowerMiddle;
                }
                else
                {
                    Sprite.Layer = RenderLayer.MainTop;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossWeakPoint" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        public BossWeakPoint(Boss boss) : base(createSprite())
        {
            this.health = BaseHealth;
            this.boss = boss;

            Sprite.Layer = RenderLayer.MainTop;
            Collision.CollisionLayers = PhysicsLayer.Enemy;
            Collision.CollisionMasks = PhysicsLayer.PlayerHitbox;
            Collision.Monitoring = true;
            Collision.Monitorable = true;

            Collision.Collided += this.onCollided;
            Removed += this.onRemoved;

            this.setupGun();
        }

        #endregion

        #region Methods

        private static AnimatedSprite createSprite()
        {
            var frames = new List<BaseSprite> {
                new BossWeakPointSprite1(),
                new BossWeakPointSprite2(),
                new BossWeakPointSprite3(),
            };
            var animation = new AnimatedSprite(1, frames);
            animation.Stop();

            return animation;
        }

        private void setupGun()
        {
            this.gun = new EnemyGun {
                CooldownDuration = 1,
                Position = new Vector2(Center.X, Bottom)
            };

            AttachChild(this.gun);
        }
        
        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (!this.Active)
            {
                return;
            }

            if (this.Mobile)
            {
                this.RotationProgress += delta / SecondsPerRotation;
            }

            this.gun.Shoot();

            base.Update(delta);
        }

        private void onCollided(object sender, CollisionArea e)
        {
            if (!this.Active)
            {
                return;
            }

            this.health--;
            if (this.Sprite is AnimatedSprite animatedSprite && this.health > 0)
            {
                animatedSprite.Frame++;
            }

            if (this.health <= 0)
            {
                QueueForRemoval();
            }
        }

        private void onRemoved(object sender, EventArgs e)
        {
            var explosion = new Explosion
            {
                Center = Center
            };

            var explosionSound = new OneShotSoundPlayer("enemy_explosion.wav");

            GetRoot().QueueNodeForAddition(explosion);
            GetRoot().QueueNodeForAddition(explosionSound);
        }

        #endregion
    }
}