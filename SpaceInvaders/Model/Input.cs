using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Handles all input for the game
    /// </summary>
    public static class Input
    {
        #region Data members

        private static readonly Dictionary<VirtualKey, bool> keyStates = new Dictionary<VirtualKey, bool> {
            {VirtualKey.Left, false},
            {VirtualKey.Right, false},
            {VirtualKey.Space, false}
        };

        #endregion

        #region Methods

        public static void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            var pressedKey = args.VirtualKey;

            if (keyStates.ContainsKey(pressedKey))
            {
                keyStates[pressedKey] = true;
            }
        }

        public static void OnKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            var pressedKey = args.VirtualKey;

            if (keyStates.ContainsKey(pressedKey))
            {
                keyStates[pressedKey] = false;
            }
        }

        /// <summary>
        ///     Determines whether [the specified key is pressed].
        ///     Keys that are not actively used will always return <c>false</c>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     <c>true</c> if [the specified key is pressed] and is actively used; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyPressed(VirtualKey key)
        {
            if (keyStates.ContainsKey(key))
            {
                return keyStates[key];
            }

            return false;
        }

        /// <summary>
        ///     Determines whether [the specified key is released].
        ///     Keys that are not actively used will always return <c>false</c>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     <c>true</c> if [the specified key is released] and is actively used; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyReleased(VirtualKey key)
        {
            if (keyStates.ContainsKey(key))
            {
                return !keyStates[key];
            }

            return false;
        }

        #endregion
    }
}