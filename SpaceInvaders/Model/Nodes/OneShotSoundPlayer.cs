using System;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A sound player that removed itself after it finishes playing
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.SoundPlayer" />
    public class OneShotSoundPlayer : SoundPlayer
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OneShotSoundPlayer" /> class.
        /// </summary>
        /// <param name="fileName">Name of the audio file.</param>
        public OneShotSoundPlayer(string fileName) : base(fileName)
        {
            Play();
            Stopped += this.onStopped;
        }

        #endregion

        #region Methods

        private void onStopped(object sender, EventArgs e)
        {
            QueueForRemoval();
        }

        #endregion
    }
}