using System.Collections.Generic;
using SpaceInvaders.Model.Nodes.Effects;
using SpaceInvaders.View;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.Entities;

namespace SpaceInvaders.Model.Nodes.Entities
{
    /// <summary>
    ///     A nuke that the boss launches at the start of the third phase
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.AnimatedSprite" />
    public class BossNuke : Entity
    {
        #region Data members

        private const double MoveSpeed = 75;

        private Vector2 target;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BossNuke" /> class.
        /// </summary>
        public BossNuke() : base(createSprite())
        {
            this.target = new Vector2(MainPage.ApplicationWidth / 2, MainPage.ApplicationHeight - 160);

            var launchSound = new OneShotSoundPlayer("nuke_launch.wav");
            GetRoot().QueueNodeForAddition(launchSound);
        }

        #endregion

        #region Methods

        private static AnimatedSprite createSprite()
        {
            var frames = new List<BaseSprite> {
                new BossNukeSprite1(),
                new BossNukeSprite2()
            };

            return new AnimatedSprite(.25, frames);
        }

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            Move(Vector2.Down * MoveSpeed * delta);
            if (Center.Y > this.target.Y)
            {
                Center = this.target;
                var waveSpawnPoint = Center + new Vector2(0, 16);

                var leftWave = new NukeWave(-1) {
                    Center = waveSpawnPoint
                };
                var rightWave = new NukeWave(1) {
                    Center = waveSpawnPoint
                };

                var launchSound = new OneShotSoundPlayer("nuke_explosion.wav");
                var explosion = new Explosion {
                    Center = Center
                };

                GetRoot().QueueNodeForAddition(leftWave);
                GetRoot().QueueNodeForAddition(rightWave);
                GetRoot().QueueNodeForAddition(launchSound);
                GetRoot().QueueNodeForAddition(explosion);
                QueueForRemoval();
            }

            base.Update(delta);
        }

        #endregion
    }
}