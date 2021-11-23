using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Handles all input for the game
    /// </summary>
    public static class Input
    {
        #region Data members

        private static readonly Dictionary<VirtualKey, bool> KeyStates = new Dictionary<VirtualKey, bool>();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the mouse position relative to the top-left corner of the window.
        /// </summary>
        /// <value>
        ///     The mouse position.
        /// </value>
        public static Vector2 MousePosition
        {
            get
            {
                var screenPosition = CoreWindow.GetForCurrentThread().PointerPosition;
                var windowX = screenPosition.X - Window.Current.Bounds.X;
                var windowY = screenPosition.Y - Window.Current.Bounds.Y;

                return new Vector2(windowX, windowY);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        public static void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            var pressedKey = args.VirtualKey;
            KeyStates[pressedKey] = true;
        }

        /// <summary>
        ///     Called when [key up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        public static void OnKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            var pressedKey = args.VirtualKey;
            KeyStates[pressedKey] = false;
        }

        /// <summary>
        ///     Determines whether [the specified key is pressed].
        ///     Keys that are not actively used will always return <c>false</c>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     <c>true</c> if [the specified key is pressed] and has been pressed before; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyPressed(VirtualKey key)
        {
            if (KeyStates.ContainsKey(key))
            {
                return KeyStates[key];
            }

            return false;
        }

        /// <summary>
        ///     Determines whether [the specified key is released].
        ///     Keys that are not actively used will always return <c>false</c>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     <c>true</c> if [the specified key is released] and has been pressed before; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyReleased(VirtualKey key)
        {
            if (KeyStates.ContainsKey(key))
            {
                return !KeyStates[key];
            }

            return false;
        }

        /// <summary>
        ///     Gets the strength of a specified key press.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>1 if the key is pressed, otherwise 0.</returns>
        public static int GetInputStrength(VirtualKey key)
        {
            return IsKeyPressed(key) ? 1 : 0;
        }

        #endregion
    }
}