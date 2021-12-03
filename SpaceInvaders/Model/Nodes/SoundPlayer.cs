using System;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     Handles playing of sound effects
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public class SoundPlayer : Node
    {
        #region Data members

        private const string AudioFolder = "ms-appx:///Audio/";

        private readonly MediaPlayer mediaPlayer;
        private string audioFile;
        private bool previousPlayState;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the volume.
        /// </summary>
        /// <value>
        ///     The volume.
        /// </value>
        public double Volume
        {
            get => this.mediaPlayer.Volume;
            set => this.mediaPlayer.Volume = value;
        }

        /// <summary>
        ///     Gets or sets the audio file.
        /// </summary>
        /// <value>
        ///     The audio file.
        /// </value>
        public string AudioFile
        {
            get => this.audioFile;
            set
            {
                if (this.audioFile == value)
                {
                    return;
                }

                var uri = new Uri($"{AudioFolder}{value}");
                this.audioFile = value;
                this.mediaPlayer.Source = MediaSource.CreateFromUri(uri);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SoundPlayer" /> is looping.
        /// </summary>
        /// <value>
        ///     <c>true</c> if looping; otherwise, <c>false</c>.
        /// </value>
        public bool Looping
        {
            get => this.mediaPlayer.IsLoopingEnabled;
            set => this.mediaPlayer.IsLoopingEnabled = value;
        }

        /// <summary>
        ///     Gets whether this instance is playing.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaying => this.mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SoundPlayer" /> class.<br />
        ///     Precondition: fileName != null<br />
        ///     Postcondition: this.AudioFile == fileName
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.ArgumentNullException">fileName</exception>
        public SoundPlayer(string fileName)
        {
            this.mediaPlayer = new MediaPlayer();
            this.AudioFile = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [started playing].
        /// </summary>
        public event EventHandler StartedPlaying;

        /// <summary>
        ///     Occurs when [stopped].
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        ///     The update loop for the Node.<br />
        ///     Precondition: None<br />
        ///     Postcondition: Node completes its update step
        /// </summary>
        /// <param name="delta">The amount of time (in seconds) since the last update tick.</param>
        public override void Update(double delta)
        {
            if (this.IsPlaying != this.previousPlayState)
            {
                if (this.IsPlaying)
                {
                    this.StartedPlaying?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    this.Stopped?.Invoke(this, EventArgs.Empty);
                }
            }

            this.previousPlayState = this.IsPlaying;

            base.Update(delta);
        }

        /// <summary>
        ///     Plays the sound effect. If the sound effect is already playing, it will restart it.<br />
        ///     Precondition: None<br />
        ///     Postcondition: The sound effect will be playing
        /// </summary>
        public void Play()
        {
            this.mediaPlayer.PlaybackSession.Position = TimeSpan.MinValue;
            this.mediaPlayer.Play();
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
            this.mediaPlayer.Dispose();
        }

        #endregion
    }
}