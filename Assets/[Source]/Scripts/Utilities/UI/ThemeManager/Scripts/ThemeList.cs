using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#endif

namespace Utilities.UI.Themes
{
    [CreateAssetMenu(fileName = "ThemeList", menuName = "UI-Themes/ThemeList", order = 2)]
    public class ThemeList : MemorizedScriptableObject<ThemeList>
    {
        //[MaxValue("ThemeCount")]
        public ushort currentThemeIndex = 0;

        public ThemeList(Theme[] themes)
        {
            this.themes = themes;
        }

        #if ODIN_INSPECTOR
        [OdinSerialize]  
        #endif
        public Theme[] themes { get; }

        public Theme CurrentTheme => themes[currentThemeIndex];

        //public ushort ThemeCount => (ushort)themes.Length;

        public void SetTheme(ushort i)
        {
            if(i > themes.Length) return;

            currentThemeIndex = i;
        }
        public void SetTheme(int i)
        {
            if(i > themes.Length) return;
            
            currentThemeIndex = (ushort)i;
        }

        public Theme this[ushort index] => themes[index];
        public Theme this[int index] => themes[index];
    } 
}
