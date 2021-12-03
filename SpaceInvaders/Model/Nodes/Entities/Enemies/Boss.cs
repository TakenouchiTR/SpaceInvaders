using System;
using System.Collections.Generic;
using SpaceInvaders.Model.Nodes.Screens.Levels;
using SpaceInvaders.Model.Nodes.UI;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities.Enemies;

namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     The final boss of the game
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Entities.Enemies.Enemy" />
    public class Boss : Enemy
    {
        #region Data members

        private const double BaseMoveSpeed = 75;
        private const double MoveSpeedBonusAmount = 50;
        private const double VibrationDecay = 1.25;
        private const int MaxHealth = 50;
        private const int WeakPointCount = 3;
        private const double Phase3SetupDuration = 6;
        private const RenderLayer PartRenderLayer = RenderLayer.MainTop;
        private static readonly Random BossRandom = new Random();

        private double speed;
        private int moveFactor;
        private int remainingWeakPoints;
        private int health;
        private double vibrationStrength;
        private Vector2 truePosition;
        private readonly HashSet<BossWeakPoint> weakPoints;
        private readonly HashSet<BossTurret> turrets;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the health.
        /// </summary>
        /// <value>
        ///     The health.
        /// </value>
        public int Health
        {
            get => this.health;
            private set
            {
                this.health = value;
                this.HealthChanged?.Invoke(this, (double) this.health / MaxHealth);
            }
        }

        /// <summary>
        ///     Gets the state.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        public BossState State { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Boss" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: None
        /// </summary>
        public Boss() : base(createSprite())
        {
            Score = 1500;

            Collision.Height /= 2;
            Collision.Monitoring = false;

            this.health = MaxHealth;
            this.speed = BaseMoveSpeed;
            this.moveFactor = 1;

            this.weakPoints = new HashSet<BossWeakPoint>();
            this.turrets = new HashSet<BossTurret>();

            Sprite.Layer = RenderLayer.MainUpperMiddle;

            Collision.Collided += this.onCollision;

            this.setupWeakPoints();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [phase transition ended].
        /// </summary>
        public event EventHandler PhaseTransitionEnded;

        /// <summary>
        ///     Occurs when [health changed].
        /// </summary>
        public event EventHandler<double> HealthChanged;

        private static AnimatedSprite createSprite()
        {
            var frames = new List<BaseSprite> {
                new BossSprite()
            };

            return new AnimatedSprite(1, frames);
        }

        private void setupWeakPoints()
        {
            for (var i = 0; i < WeakPointCount; i++)
            {
                var weakPoint = new BossWeakPoint(this) {
                    Center = new Vector2(Left + Width / 2 * i, Center.Y + 16),
                    Active = true
                };

                this.remainingWeakPoints++;
                this.weakPoints.Add(weakPoint);

                weakPoint.Removed += this.onWeakPointRemoved;
                AttachChild(weakPoint);
            }
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            switch (this.State)
            {
                case BossState.Phase1:
                case BossState.Phase2:
                case BossState.Phase3:
                    this.updateMainPhase(delta);
                    break;
                case BossState.Phase2Transition:
                case BossState.Phase3Transition:
                    this.updateTransition(delta);
                    break;
                case BossState.Phase2Setup:
                case BossState.Phase3Setup:
                    this.updatePhaseSetup(delta);
                    break;
                case BossState.Destroyed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Update(delta);
        }

        private void updateMainPhase(double delta)
        {
            var moveVector = Vector2.Right * this.speed * this.moveFactor;
            Move(moveVector * delta);

            if (Right > MainPage.ApplicationWidth)
            {
                this.moveFactor = -1;
            }
            else if (Left < 0)
            {
                this.moveFactor = 1;
            }
        }

        private void updateTransition(double delta)
        {
            this.vibrate();

            var distanceFromCenter = Center.X - MainPage.ApplicationWidth / 2;

            this.moveFactor = distanceFromCenter > 0 ? -1 : 1;

            if (Math.Abs(distanceFromCenter) > this.speed * delta)
            {
                var moveVector = Vector2.Right * this.speed * this.moveFactor;
                Move(moveVector * delta);
                this.truePosition += moveVector * delta;
            }
            else
            {
                Center = new Vector2(MainPage.ApplicationWidth / 2, Center.Y);
                this.truePosition = Position;
                this.State++;

                this.PhaseTransitionEnded?.Invoke(this, EventArgs.Empty);
            }
        }

        private void updatePhaseSetup(double delta)
        {
            this.vibrate();
            this.vibrationStrength = Math.Max(this.vibrationStrength - VibrationDecay * delta, 0);
        }

        private void vibrate()
        {
            var vibrationAmount = new Vector2(BossRandom.NextDouble() * this.vibrationStrength,
                BossRandom.NextDouble() * this.vibrationStrength);
            Position = this.truePosition + vibrationAmount;
        }

        private void onWeakPointRemoved(object sender, EventArgs e)
        {
            this.remainingWeakPoints -= 1;
            this.weakPoints.Remove(sender as BossWeakPoint);

            if (this.remainingWeakPoints > 0)
            {
                this.speed += MoveSpeedBonusAmount;
            }
            else
            {
                if (Center.X > MainPage.ApplicationWidth / 2)
                {
                    this.moveFactor = -1;
                }
                else
                {
                    this.moveFactor = 1;
                }

                if (this.State == BossState.Phase1)
                {
                    this.PhaseTransitionEnded += this.onPhase2TransitionEnded;
                }
                else
                {
                    this.PhaseTransitionEnded += this.onPhase3TransitionEnded;
                }

                this.State += 1;
                this.vibrationStrength = 4;
                this.speed = BaseMoveSpeed;
                this.truePosition = Position;
            }
        }

        private void onPhase2TransitionEnded(object sender, EventArgs e)
        {
            this.PhaseTransitionEnded -= this.onPhase2TransitionEnded;

            if (GetRoot() is LevelBase level)
            {
                level.RemainingBonusEnemies = 0;
            }

            var weakPointTimer = new Timer();
            weakPointTimer.Start();
            weakPointTimer.Tick += this.onWeakPointTimerTick;

            QueueNodeForAddition(weakPointTimer);
        }

        private void onPhase3TransitionEnded(object sender, EventArgs e)
        {
            this.PhaseTransitionEnded -= this.onPhase3TransitionEnded;

            var healthBar = new BossHealthBar(this);
            healthBar.X = MainPage.ApplicationWidth / 2 - healthBar.Width / 2;

            var nuke = new BossNuke {
                Center = Center
            };

            var phase3SetupTimer = new Timer(Phase3SetupDuration);
            var outerTurretTimer = new Timer(Phase3SetupDuration / 4);
            var innerTurretTimer = new Timer(2 * Phase3SetupDuration / 4);
            var centerTurretTimer = new Timer(3 * Phase3SetupDuration / 4);

            phase3SetupTimer.Start();
            outerTurretTimer.Start();
            innerTurretTimer.Start();
            centerTurretTimer.Start();

            phase3SetupTimer.Tick += this.onPhase3SetupTimerTick;
            outerTurretTimer.Tick += this.onOuterTurretTimerTick;
            innerTurretTimer.Tick += this.onInnerTurretTimerTick;
            centerTurretTimer.Tick += this.onCenterTurretTimerTick;

            QueueNodeForAddition(phase3SetupTimer);
            QueueNodeForAddition(outerTurretTimer);
            QueueNodeForAddition(innerTurretTimer);
            QueueNodeForAddition(centerTurretTimer);

            GetRoot().QueueNodeForAddition(healthBar);
            GetRoot().QueueNodeForAddition(nuke);
        }

        private void onPhase3SetupTimerTick(object sender, EventArgs e)
        {
            (sender as Timer)?.QueueForRemoval();

            Collision.Monitoring = true;

            foreach (var bossTurret in this.turrets)
            {
                bossTurret.Active = true;
            }

            this.State = BossState.Phase3;
        }

        private void onWeakPointTimerTick(object sender, EventArgs e)
        {
            if (this.remainingWeakPoints >= WeakPointCount)
            {
                this.State += 1;
                (sender as Timer)?.QueueForRemoval();

                foreach (var bossWeakPoint in this.weakPoints)
                {
                    bossWeakPoint.Active = true;
                }

                return;
            }

            var weakPoint = new BossWeakPoint(this) {
                Center = new Vector2(Left + Width / 2 * this.remainingWeakPoints, Center.Y + 16),
                RotationProgress = this.remainingWeakPoints * (1 / 3.0) + .167 / 2,
                Mobile = true
            };

            this.remainingWeakPoints++;
            this.weakPoints.Add(weakPoint);

            weakPoint.Removed += this.onWeakPointRemoved;

            QueueNodeForAddition(weakPoint);
            GetRoot().QueueNodeForAddition(new OneShotSoundPlayer("boss_part_added.wav"));
        }

        private void onOuterTurretTimerTick(object sender, EventArgs e)
        {
            (sender as Timer)?.QueueForRemoval();

            var leftTurret = new BossMachineGunTurret(new Vector2(-1, 5).ToAngle()) {
                Position = new Vector2(Left + 16, Center.Y + 8),
                Sprite = {
                    Layer = PartRenderLayer
                }
            };
            var rightTurret = new BossMachineGunTurret(new Vector2(1, 5).ToAngle()) {
                Position = new Vector2(Right - 16, Center.Y + 8),
                Sprite = {
                    Layer = PartRenderLayer
                }
            };

            QueueNodeForAddition(leftTurret);
            QueueNodeForAddition(rightTurret);

            this.turrets.Add(leftTurret);
            this.turrets.Add(rightTurret);

            GetRoot().QueueNodeForAddition(new OneShotSoundPlayer("boss_part_added.wav"));
        }

        private void onInnerTurretTimerTick(object sender, EventArgs e)
        {
            (sender as Timer)?.QueueForRemoval();

            var player = (PlayerShip) GetRoot().GetChildByName("PlayerShip");

            var leftTurret = new BossTrackingTurret(player) {
                Position = new Vector2(Width / 4 + X, Center.Y + 8),
                Sprite = {
                    Layer = PartRenderLayer
                }
            };
            var rightTurret = new BossTrackingTurret(player) {
                Position = new Vector2(3 * Width / 4 + X, Center.Y + 8),
                Sprite = {
                    Layer = PartRenderLayer
                }
            };

            QueueNodeForAddition(leftTurret);
            QueueNodeForAddition(rightTurret);

            this.turrets.Add(leftTurret);
            this.turrets.Add(rightTurret);

            GetRoot().QueueNodeForAddition(new OneShotSoundPlayer("boss_part_added.wav"));
        }

        private void onCenterTurretTimerTick(object sender, EventArgs e)
        {
            (sender as Timer)?.QueueForRemoval();

            var turret = new BossScatterTurret {
                Position = new Vector2(Center.X, Center.Y + 8),
                Sprite = {
                    Layer = PartRenderLayer
                }
            };

            QueueNodeForAddition(turret);
            this.turrets.Add(turret);

            GetRoot().QueueNodeForAddition(new OneShotSoundPlayer("boss_part_added.wav"));
        }

        private void onCollision(object sender, CollisionArea e)
        {
            this.Health--;
            if (this.health <= 0)
            {
                this.State = BossState.Destroyed;
                foreach (var bossTurret in this.turrets)
                {
                    bossTurret.Active = false;
                }

                QueueForRemoval();
            }
        }

        #endregion
    }
}