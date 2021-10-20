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

        public int CurrentLives
        {
            get => this.currentLives;
            set => this.currentLives = Math.Clamp(value, 0, this.maxLives);
        }

        public int MaxShots { get; set; }

        public double ShotCooldownDuration
        {
            get => this.shotCooldownTimer.Duration;
            set => this.shotCooldownTimer.Duration = value;
        }

        private bool CanShoot => this.activeShots < this.MaxShots && !this.shotCooldownTimer.IsActive;

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
            this.respawnTimer.Tick += onRespawnTimerTick;
            this.invulnerabilityTimer.Tick += onInvulnerabilityTimerTick;
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
            var explosion = new Explosion
            {
                Center = Center
            };
            GetRoot().QueueNodeForAddition(explosion);

            this.Collision.Monitoring = false;
            this.Collision.Monitorable = false;

            this.Sprite.Visible = false;

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
                this.QueueForRemoval();
                return;
            }

            this.Sprite.Sprite.Opacity = .5;

            this.X = MainPage.ApplicationWidth / 2 - this.Width / 2;
            this.Collision.Monitorable = true;
            this.Sprite.Visible = true;
            this.isAlive = true;

            this.invulnerabilityTimer.Restart();
        }

        private void onInvulnerabilityTimerTick(object sender, EventArgs e)
        {
            this.Sprite.Sprite.Opacity = 1;
            this.Collision.Monitoring = true;
        }

        #endregion
    }
}