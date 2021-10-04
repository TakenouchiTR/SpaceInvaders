﻿using System;
using Windows.UI.Xaml;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities.Enemies
{
    public class TestBossTarget : GameObject
    {
        #region Data members

        private int health = 3;
        private bool canShoot;
        private readonly DispatcherTimer shootToggleTimer;
        private readonly DispatcherTimer shootTimer;

        #endregion

        #region Constructors

        public TestBossTarget(GameManager manager) : base(manager, new BasicEnemySprite())
        {
            Monitoring = true;
            Monitorable = true;
            CollisionLayers = PhysicsLayer.Enemy;
            CollisionMasks = PhysicsLayer.PlayerHitbox;

            this.canShoot = false;
            this.shootToggleTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            this.shootToggleTimer.Tick += this.onShootToggleTimerTick;
            this.shootToggleTimer.Start();

            this.shootTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(.5)
            };
            this.shootTimer.Tick += this.onShooTimerTick;
            this.shootTimer.Start();

            Removed += this.onRemoved;
        }

        #endregion

        #region Methods

        private void onRemoved(object sender, EventArgs e)
        {
            this.shootTimer.Stop();
            this.shootToggleTimer.Stop();
            this.shootTimer.Tick -= this.onShooTimerTick;
            this.shootToggleTimer.Tick -= this.onShootToggleTimerTick;
        }

        private void onShooTimerTick(object sender, object e)
        {
            if (this.canShoot)
            {
                var bullet = new EnemyBullet(Manager) {
                    Position = Position
                };

                Manager.QueueGameObjectForAddition(bullet);
            }
        }

        private void onShootToggleTimerTick(object sender, object e)
        {
            this.canShoot = !this.canShoot;
        }

        public override void Update(double delta)
        {
        }

        public override void HandleCollision(GameObject target)
        {
            this.health--;
            if (this.health <= 0)
            {
                QueueRemoval();
            }
        }

        #endregion
    }
}