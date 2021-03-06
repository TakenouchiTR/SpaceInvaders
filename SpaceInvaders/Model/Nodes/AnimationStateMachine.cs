using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Model.Nodes
{
    /// <summary>
    ///     A state machine for changing animations. AnimatedSprites should <i>not</i> be added as a child to this node.<br />
    ///     Animations should be a child of a different node so that they properly move with the parent.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.Node" />
    public class AnimationStateMachine : Node
    {
        #region Data members

        private readonly Dictionary<string, AnimatedSprite> animations;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the name of the current animation.
        /// </summary>
        /// <value>
        ///     The name of the current animation.
        /// </value>
        public string CurrentAnimationName { get; protected set; }

        /// <summary>
        ///     Gets the current animation.<br />
        ///     If no animations are added, returns null.
        /// </summary>
        /// <value>
        ///     The current animation if animations have been added, otherwise null.
        /// </value>
        public AnimatedSprite CurrentAnimation
        {
            get
            {
                if (this.animations.Count == 0)
                {
                    return null;
                }

                return this.animations[this.CurrentAnimationName];
            }
        }

        /// <summary>
        ///     Gets a list of all added animations.
        /// </summary>
        /// <value>
        ///     The animations.
        /// </value>
        public IList<AnimatedSprite> Animations => this.animations.Values.ToList();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnimationStateMachine" /> class.<br />
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public AnimationStateMachine()
        {
            this.animations = new Dictionary<string, AnimatedSprite>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds an animation with a specified name. The name <i>must</i> be unique.<br />
        ///     Newly added animations will be stopped, reset to the first frame, and be made invisible.<br />
        ///     Precondition: name != null &amp;&amp;<br />
        ///     animation != null &amp;&amp;<br />
        ///     an animation using name doesn't already exist<br />
        ///     Postcondition: the animation is added with the specified name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="animation">The animation.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     name
        ///     or
        ///     animation
        /// </exception>
        /// <exception cref="System.ArgumentException">Animation {name} already exists</exception>
        public void AddAnimation(string name, AnimatedSprite animation)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (this.animations.ContainsKey(name))
            {
                throw new ArgumentException($"Animation {name} already exists");
            }

            this.animations[name] = animation ?? throw new ArgumentNullException(nameof(animation));
            animation.Stop();
            animation.Visible = false;
        }

        /// <summary>
        ///     Changes the animation.<br />
        ///     If the animation is already playing, nothing will occur.<br />
        ///     Precondition: nextAnimation != null &amp;&amp;<br />
        ///     An animation with the specified name exists<br />
        ///     Postcondition: Animation changes to specified animation, if it was not already playing.
        /// </summary>
        /// <param name="nextAnimation">The next animation.</param>
        /// <exception cref="System.ArgumentNullException">nextAnimation</exception>
        /// <exception cref="System.ArgumentException">Animation {nextAnimation} doesn't exist</exception>
        public void ChangeAnimation(string nextAnimation)
        {
            if (nextAnimation == null)
            {
                throw new ArgumentNullException(nameof(nextAnimation));
            }

            if (!this.animations.ContainsKey(nextAnimation))
            {
                throw new ArgumentException($"Animation {nextAnimation} doesn't exist");
            }

            if (nextAnimation == this.CurrentAnimationName)
            {
                return;
            }

            if (this.CurrentAnimationName != null)
            {
                this.CurrentAnimation.Stop();
                this.CurrentAnimation.Visible = false;
            }

            this.CurrentAnimationName = nextAnimation;
            this.CurrentAnimation.Start();
            this.CurrentAnimation.Visible = true;
        }

        /// <summary>
        ///     Sets the render layer for all animated sprites.<br />
        ///     Precondition: None<br />
        ///     Postcondition: all items in this.Animations have Layer set to layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public void SetRenderLayer(RenderLayer layer)
        {
            foreach (var animatedSprite in this.Animations)
            {
                animatedSprite.Layer = layer;
            }
        }

        #endregion
    }
}