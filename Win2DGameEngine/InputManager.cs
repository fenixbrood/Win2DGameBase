using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Win2DGameEngine
{
    public class InputManager
    {
        public bool Capture(VirtualKey key)
        {
            return true;
        }
        public Vector2 MousePoint { get; set; } = Vector2.One;

        public Dictionary<VirtualKey, bool> KeyCache { get; set; } = new Dictionary<VirtualKey, bool>();

        public void KeyDown(VirtualKey key)
        {
            KeyCache[key] = true;
        }
        public bool IsKeyDown(VirtualKey key)
        {
            return KeyCache.ContainsKey(key) && KeyCache[key];
        }
        public bool IsKeyDownOnce(VirtualKey key)
        {
            var ok = KeyCache.ContainsKey(key) && KeyCache[key];
            KeyUp(key);
            return ok;
        }
        public void KeyUp(VirtualKey key)
        {
            KeyCache[key] = false;
        }

    }
}
