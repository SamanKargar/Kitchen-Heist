using System.Collections.Generic;

namespace _Game.Scripts.Utils {
    public static class KeyIconHelper {
        private static readonly Dictionary<string, string> KeyIconMap = new Dictionary<string, string> {
            { "W", KeyIcons.KEY_ICON_W },
            { "A", KeyIcons.KEY_ICON_A },
            { "S", KeyIcons.KEY_ICON_S },
            { "D", KeyIcons.KEY_ICON_D },
            { "E", KeyIcons.KEY_ICON_E },
            { "F", KeyIcons.KEY_ICON_F },
            { "Q", KeyIcons.KEY_ICON_Q }
        };
        
        public static string GetKeyIcon(string keyName) {
            return KeyIconMap.GetValueOrDefault(keyName.ToUpper(), keyName);
        }
    }
}