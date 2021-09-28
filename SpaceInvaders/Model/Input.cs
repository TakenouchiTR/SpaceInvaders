using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace SpaceInvaders.Model
{
    public static class Input
    {
        private static Dictionary<VirtualKey, bool> keyStates = new Dictionary<VirtualKey, bool>() {
            { VirtualKey.Left, false },
            { VirtualKey.Right, false }
        };

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

        public static bool IsKeyPressed(VirtualKey key)
        {
            if (keyStates.ContainsKey(key))
            {
                return keyStates[key];
            }

            return false;
        }

        public static bool IsKeyReleased(VirtualKey key)
        {
            if (keyStates.ContainsKey(key))
            {
                return !keyStates[key];
            }

            return false;
        }
    }
}
