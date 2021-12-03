using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using SpaceInvaders.View.UI;

namespace SpaceInvaders.Model.Nodes.UI
{
    /// <summary>
    ///     Draws a button
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.Nodes.RenderableNode" />
    public class Button : RenderableNode
    {
        #region Data members

        private readonly ButtonSprite buttonSprite;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the button's width.
        /// </summary>
        /// <value>
        ///     The button's width.
        /// </value>
        public override double Width
        {
            get => this.buttonSprite.ButtonWidth;
            set => this.buttonSprite.ButtonWidth = value;
        }

        /// <summary>
        ///     Gets the button's height.
        /// </summary>
        /// <value>
        ///     The button's height.
        /// </value>
        public override double Height
        {
            get => this.buttonSprite.ButtonHeight;
            set => this.buttonSprite.ButtonHeight = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Button" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == RenderableNode.DefaultRenderLayer
        /// </summary>
        public Button() : this("")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Button" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer &amp;&amp;<br />
        ///     this.Text == text &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="text">The text.</param>
        public Button(string text) : this(text, DefaultRenderLayer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Button" /> class.<br />
        ///     Precondition: None<br />
        ///     Postcondition: this.Layer == SpriteNode.DefaultRenderLayer &amp;&amp;<br />
        ///     this.Text == text &amp;&amp;<br />
        ///     this.Visible == true
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="layer">The layer.</param>
        public Button(string text, RenderLayer layer) : base(new ButtonSprite(), layer)
        {
            this.buttonSprite = (ButtonSprite) Sprite;
            this.buttonSprite.Text = text;

            this.buttonSprite.Button.Click += this.onButtonClick;
            this.buttonSprite.Button.PointerEntered += this.onButtonPointerEntered;
            this.buttonSprite.Button.PointerExited += this.onButtonPointerExited;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [click].
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        ///     Occurs when [mouse enters the button area].
        /// </summary>
        public event EventHandler<Vector2> MouseEntered;

        /// <summary>
        ///     Occurs when [mouse exits the button area].
        /// </summary>
        public event EventHandler<Vector2> MouseExited;

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

            if (this.Click != null)
            {
                foreach (var subscriber in this.Click.GetInvocationList())
                {
                    this.Click -= subscriber as EventHandler;
                }
            }

            if (this.MouseEntered != null)
            {
                foreach (var subscriber in this.MouseEntered.GetInvocationList())
                {
                    this.MouseEntered -= subscriber as EventHandler<Vector2>;
                }
            }

            if (this.MouseExited != null)
            {
                foreach (var subscriber in this.MouseExited.GetInvocationList())
                {
                    this.MouseExited -= subscriber as EventHandler<Vector2>;
                }
            }

            this.buttonSprite.Button.Click -= this.onButtonClick;
            this.buttonSprite.Button.PointerEntered -= this.onButtonPointerEntered;
            this.buttonSprite.Button.PointerExited -= this.onButtonPointerExited;
        }

        private void onButtonClick(object sender, RoutedEventArgs e)
        {
            this.Click?.Invoke(this, EventArgs.Empty);
        }

        private void onButtonPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.MouseEntered?.Invoke(this, Input.MousePosition);
        }

        private void onButtonPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.MouseExited?.Invoke(this, Input.MousePosition);
        }

        #endregion
    }
}