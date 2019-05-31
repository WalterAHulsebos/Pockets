using System.Reflection;
using UnityEngine;

namespace Utilities.UI.Themes
{
    [ExecuteInEditMode]
    public abstract class FlexibleUIBase : MonoBehaviour
    {
        #region Variables 
        
        [SerializeField] private string id;
        
        private Theme theme;
        private object flexibleObjectData;
        private ThemeList themeList;
        
        #endregion
        
        #region Methods
        
        public virtual void Start()
        {
            OnUIChanged();
        }
        
        public virtual void OnEnable()
        {
            themeList =
                #if UNITY_EDITOR
                //TODO: Check if this works?
                UIThemeManager.Instance.themeList.Load;
                #else
                UIThemeManager.Instance.themeList;
                #endif
                
            UIThemeManager.Instance.UIChanged += OnUIChanged;
        }

        public virtual void OnDisable()
        {
            UIThemeManager.Instance.UIChanged -= OnUIChanged;
        }

        private void OnUIChanged()
        {
            theme = themeList.CurrentTheme;
            
            if(theme == null) return;
            
            FieldInfo fieldInfo = theme.GetType().GetField(id);
            
            if(fieldInfo == null) return;
            
            flexibleObjectData = fieldInfo.GetValue(theme);
            if(flexibleObjectData != null)
            {
                UpdateUI();
            }
        }
        
        public virtual void UpdateUI()
        {
            
        }
        
        internal T GetFlexibleObject<T>()
        {
            return (T)flexibleObjectData;
        }
        
        public virtual void Update()
        {
            #if UNITY_EDITOR
            
            OnUIChanged();
            
            #endif
            
            /*
            if (Application.isEditor)
            {
                OnUIChanged();
            }
            */
        }
        
        #endregion
        
    }
}
