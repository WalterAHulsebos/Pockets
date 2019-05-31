using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.UI.Themes
{
    public class UIThemeManager : EnsuredSingleton<UIThemeManager>
    {
        #region Variables

        [SerializeField] private ushort themeIndex;
        
        // ReSharper disable once InconsistentNaming
        public event Action UIChanged = () => { };
        public ThemeList themeList;
        private static bool _isInitialized;
        
        #endregion

        #region Methods

        //#if UNITY_EDITOR
        
        private void OnValidate()
        {
            ChangeUI(themeIndex);
        }
        
        //#endif
        
        public void ChangeUI(int themeIndex)
        {
            themeList.SetTheme(themeIndex);
            UIChanged();
        }
        
        #endregion
        
    }

}


