using System;
using Windows.System;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     The player character
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class PlayerShip : Entity
    {
        #region Data members

        private const VirtualKey LeftKey = VirtualKey.Left;
        private const VirtualKey RightKey = VirtualKey.Right;
        private const VirtualKey ShootKey = VirtualKey.Space;

        private readonly Vector2 bulletSpawnLocation = new Vector2(12, -4);

        private readonly int moveSpeed = 200;

        private int maxLives;
        private int currentLives;
        private int activeShots;
        private bool isAlive;
        private Vector2 velocity;
        private Timer invulnerabilityTimer;
        private Timer respawnTimer;
        private Timer shotCooldownTimer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the maximum lives. Values will be clamped to 1 as the lowest value.<br />
        ///     If the value is increased, this.CurrentLives will increase by the same amount.<br />
        ///     If the value is decreased, this.CurrentLives will match this.MaxLives if it falls below it.
        /// </summary>
        /// <value>
        ///     The maximum lives.
        /// </value>
        public int MaxLives
        {
            get => this.maxLives;
            set
            {
                value = Math.Max(value, 1);
                if (value > this.maxLives)
                {
                    this.currentLives += value - this.maxLives;
                    this.maxLives = value;
                }
                else
                {
                    this.maxLives = value;
                    this.currentLives = Math.Min(this.maxLives, this.currentLives);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current lives. Values will be clamped between 0 and this.MaxLives, inclusive.<br />
        /// </summary>
        /// <value>
        ///     The current lives.
        /// </value>
        public int CurrentLives
        {
            get => this.currentLives;
            set => this.currentLives = Math.Clamp(value, 0, this.maxLives);
        }

        /// <summary>
        ///     Gets or sets the maximum shots that can be on the screen at once.
        /// </summary>
        /// <value>
        ///     The maximum shots.
        /// </value>
        public int MaxShots { get; set; }

        /// <summary>
        ///     Gets or sets the delay between each shot fired.
        /// </summary>
        /// <value>
        ///     The duration of the shot cooldown.
        /// </value>
        public double ShotCooldownDuration
        {
            get => this.shotCooldownTimer.Duration;
            set => this.shotCooldownTimer.Duration = value;
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can shoot.<br />
        ///     The shot cooldown must not be active and there must be less than the maximum amount of shots active to be able to
        ///     shoot.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can shoot; otherwise, <c>false</c>.
        /// </value>
        private bool CanShoot => this.activeShots < this.MaxShots && !this.shotCooldownTimer.IsActive;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Collision.Monitoring == true &amp;&amp;<br />
        ///     this.Collision.Monitorable == true &amp;&amp;<br />
        ///     this.Collision.CollisionLayers == PhysicsLayer.Player &amp;&amp;<br />
        ///     this.Collision.CollisionMasks == PhysicsLayer.EnemyHitbox | PhysicsLayer.Enemy &amp;&amp;<br />
        ///     this.Sprite is PlayerShipSprite
        /// </summary>
        public PlayerShip() : base(new PlayerShipSprite())
        {
            this.velocity = new Vector2();
            this.isAlive = true;

            this.MaxLives = 3;
            this.MaxShots = 3;

            this.setupCollision();
            this.setupTimers();

            Collision.Collided += this.onCollision;
            this.respawnTimer.Tick += this.onRespawnTimerTick;
            this.invulnerabilityTimer.Tick += this.onInvulnerabilityTimerTick;
        }

        #endregion

        #region Methods

        private void setupCollision()
        {
            Collision.Monitorable = true;
            Collision.Monitoring = true;

            Collision.CollisionLayers = PhysicsLayer.Player;
            Collision.CollisionMasks = PhysicsLayer.EnemyHitbox | PhysicsLayer.Enemy;
        }

        private void setupTimers()
        {
            this.respawnTimer = new Timer(1.5, false);
            this.invulnerabilityTimer = new Timer(1.5, false);
            this.shotCooldownTimer = new Timer(.33, false);

            AttachChild(this.respawnTimer);
            AttachChild(this.invulnerabilityTimer);
            AttachChild(this.shotCooldownTimer);
        }

        private void onCollision(object sender, CollisionArea e)
        {
            var explosion = new Explosion {
                Center = Center
            };
            GetRoot().QueueNodeForAddition(explosion);

            Collision.Monitoring = false;
            Collision.Monitorable = false;

            Sprite.Visible = false;

            this.respawnTimer.Restart();
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (this.isAlive)
            {
                this.handleMovement(delta);
                this.handleShooting();
            }

            base.Update(delta);
        }

        private void handleMovement(double delta)
        {
            double moveDistance = 0;

            if (Input.IsKeyPressed(LeftKey))
            {
                moveDistance -= 1;
            }

            if (Input.IsKeyPressed(RightKey))
            {
                moveDistance += 1;
            }

            if (moveDistance != 0)
            {
                moveDistance *= this.moveSpeed * delta;

                if (X + moveDistance < 0)
                {
                    moveDistance = -X;
                }
                else if (Collision.Right + moveDistance > MainPage.ApplicationWidth)
                {
                    moveDistance = MainPage.ApplicationWidth - Collision.Right;
                }

                this.velocity.X = moveDistance;

                Move(this.velocity);
            }
        }

        private void handleShooting()
        {
            if (this.CanShoot && Input.IsKeyPressed(ShootKey))
            {
                var bullet = new PlayerBullet {
                    Position = Position + this.bulletSpawnLocation
                };
                bullet.Removed += this.onBulletRemoval;

                Parent.QueueNodeForAddition(bullet);
                this.activeShots++;
                this.shotCooldownTimer.Restart();
            }
        }

        private void onBulletRemoval(object sender, EventArgs e)
        {
            if (sender is Bullet bullet)
            {
                this.activeShots--;
                bullet.Removed -= this.onBulletRemoval;
            }
        }

        private void onRespawnTimerTick(object sender, EventArgs e)
        {
            this.CurrentLives--;
            if (this.CurrentLives == 0)
            {
                QueueForRemoval();
                return;
            }

            Sprite.Sprite.Opacity = .5;

            X = MainPage.ApplicationWidth / 2 - Width / 2;
            Collision.Monitorable = true;
            Sprite.Visible = true;
            this.isAlive = true;

            this.invulnerabilityTimer.Restart();
        }

        private void onInvulnerabilityTimerTick(object sender, EventArgs e)
        {
            Sprite.Sprite.Opacity = 1;
            Collision.Monitoring = true;
        }

        #endregion
    }
}