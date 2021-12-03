using System;
using Windows.System;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.Model.Nodes.Screens.Levels;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites.Entities;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     The player character
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Entity" />
    public class PlayerShip : Entity
    {
        #region Data members

        private const int MoveSpeed = 200;
        private const int PointsPerGraze = 10;
        private const double GrazeMeterPerBullet = .1;
        private const double GrazeLostOnDeath = .5;
        private const double SlowdownDuration = 3;
        private const double CollisionSizeMultiplier = .8;

        private const VirtualKey LeftKey = VirtualKey.Left;
        private const VirtualKey RightKey = VirtualKey.Right;
        private const VirtualKey ShootKey = VirtualKey.Space;
        private const VirtualKey SlowdownKey = VirtualKey.X;

        private bool isAlive;
        private bool isSlowingTime;
        private bool slowdownPrevPressed;
        private int maxLives;
        private int currentLives;
        private double grazeMeter;
        private Vector2 velocity;
        private Gun gun;
        private Timer invulnerabilityTimer;
        private Timer respawnTimer;
        private SoundPlayer explosionSound;
        private SoundPlayer grazeSound;

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
            set
            {
                if (value == this.currentLives)
                {
                    return;
                }

                this.currentLives = Math.Clamp(value, 0, this.maxLives);
                this.CurrentLivesChanged?.Invoke(this, this.currentLives);
            }
        }

        /// <summary>
        ///     Gets or sets the graze meter.
        /// </summary>
        /// <value>
        ///     The graze meter.
        /// </value>
        public double GrazeMeter
        {
            get => this.grazeMeter;
            set
            {
                this.grazeMeter = Math.Clamp(value, 0, 1);
                this.GrazeMeterChanged?.Invoke(this, this.grazeMeter);
            }
        }

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

            this.setupCollision();
            this.setupTimers();
            this.setupGun();
            this.setupExplosion();
            this.setupGrazeHitbox();

            Collision.Collided += this.onCollision;
            this.respawnTimer.Tick += this.onRespawnTimerTick;
            this.invulnerabilityTimer.Tick += this.onInvulnerabilityTimerTick;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [current lives changed].
        /// </summary>
        public event EventHandler<int> CurrentLivesChanged;

        /// <summary>
        ///     Occurs when [graze meter changed].
        /// </summary>
        public event EventHandler<double> GrazeMeterChanged;

        /// <summary>
        ///     Occurs when [killed].
        /// </summary>
        public event EventHandler Killed;
        private void setupCollision()
        {
            Collision.Width *= CollisionSizeMultiplier;
            Collision.Height *= CollisionSizeMultiplier;
            Collision.Center = Center;

            Collision.Monitorable = true;
            Collision.Monitoring = true;

            Collision.CollisionLayers = PhysicsLayer.Player;
            Collision.CollisionMasks = PhysicsLayer.EnemyHitbox | PhysicsLayer.Enemy;
        }

        private void setupTimers()
        {
            this.respawnTimer = new Timer(1.5, false);
            this.invulnerabilityTimer = new Timer(1.5, false);

            AttachChild(this.respawnTimer);
            AttachChild(this.invulnerabilityTimer);
        }

        private void setupGun()
        {
            const PhysicsLayer collisionLayer = PhysicsLayer.PlayerHitbox;
            const PhysicsLayer collisionMasks = PhysicsLayer.Enemy | PhysicsLayer.World;
            const double bulletSpeed = 700;

            this.gun = new Gun(collisionLayer, collisionMasks, "player_shot.wav") {
                Rotation = Vector2.Up.ToAngle(),
                BulletSpeed = bulletSpeed,
                Position = Center,
                CooldownDuration = .25
            };

            AttachChild(this.gun);
        }

        private void setupExplosion()
        {
            this.explosionSound = new SoundPlayer("player_explosion.wav");
        }

        private void setupGrazeHitbox()
        {
            var grazeHitbox = new GrazeHitbox {
                Width = Width + 32,
                Height = Height + 12,
                Center = Center
            };
            grazeHitbox.BulletGrazed += this.onBulletGrazed;

            this.grazeSound = new SoundPlayer("graze.wav");
            AttachChild(grazeHitbox);
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
                this.handleSlowdown(delta);
            }

            base.Update(delta);
        }

        private void handleMovement(double delta)
        {
            double moveDistance = Input.GetInputStrength(RightKey) - Input.GetInputStrength(LeftKey);

            if (moveDistance != 0)
            {
                moveDistance *= MoveSpeed * delta;

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
            if (Input.IsKeyPressed(ShootKey))
            {
                this.gun.Shoot();
            }
        }

        private void handleSlowdown(double delta)
        {
            if (this.isSlowingTime)
            {
                this.GrazeMeter -= delta / SlowdownDuration;

                if (this.grazeMeter == 0)
                {
                    this.unSlowTime();
                }
            }

            if (!this.slowdownPrevPressed && Input.IsKeyPressed(SlowdownKey))
            {
                if (this.isSlowingTime)
                {
                    this.unSlowTime();
                }
                else if (this.grazeMeter > 0)
                {
                    this.slowTime();
                }
            }

            this.slowdownPrevPressed = Input.IsKeyPressed(SlowdownKey);
        }

        private void slowTime()
        {
            var level = (LevelBase) GetRoot();
            level.SpeedModifier = .5;
            this.isSlowingTime = true;
        }

        private void unSlowTime()
        {
            var level = (LevelBase) GetRoot();
            level.SpeedModifier = 1;
            this.isSlowingTime = false;
        }

        /// <summary>
        ///     Runs cleanup and invokes the Removed event when removed from the game.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Removed event is invoked if emitRemovedEvent == true &amp;&amp;<br />
        ///     All event subscribers are removed
        /// </summary>
        /// <param name="emitRemovedEvent">Whether to emit the Removed event</param>
        public override void CompleteRemoval(bool emitRemovedEvent = true)
        {
            base.CompleteRemoval(emitRemovedEvent);

            if (this.CurrentLivesChanged != null)
            {
                foreach (var subscriber in this.CurrentLivesChanged.GetInvocationList())
                {
                    this.CurrentLivesChanged -= subscriber as EventHandler<int>;
                }
            }
        }

        private void onCollision(object sender, CollisionArea e)
        {
            this.explosionSound.Play();

            var explosion = new Explosion {
                Center = Center
            };
            GetRoot().QueueNodeForAddition(explosion);

            Collision.Monitoring = false;
            Collision.Monitorable = false;

            Sprite.Visible = false;

            this.unSlowTime();
            this.isAlive = false;
            this.respawnTimer.Restart();
            this.CurrentLives--;
            this.GrazeMeter -= GrazeLostOnDeath;

            this.Killed?.Invoke(this, EventArgs.Empty);
        }

        private void onRespawnTimerTick(object sender, EventArgs e)
        {
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

        private void onBulletGrazed(object sender, EventArgs e)
        {
            if (this.isAlive && !this.isSlowingTime)
            {
                var root = (LevelBase) GetRoot();
                root.AddPoints(PointSource.Graze, PointsPerGraze);

                this.GrazeMeter += GrazeMeterPerBullet;
                this.grazeSound.Play();
            }
        }

        #endregion
    }
}