using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

namespace Utilities.UI.Themes
{
    [CreateAssetMenu(fileName = "Theme", menuName = "UI-Themes/Theme", order = 1)]
    public class Theme : MemorizedScriptableObject<Theme>
    {
        //TODO: Edit
        
        [ValueDropdown("_options")]
        public int someSize1;
        
        [SerializeField]
        private IEnumerable _options = new ValueDropdownList<int>()
        {
            { "North", 0 },
            { "Blue", 1 },
            { "Red", 2 },
            { "Whatever", 3 },
            { "South?", 4 }
        };
        
        [Header("Button")]
        public TextData buttonSpriteSwapText;
        public ButtonData buttonSpriteSwap;

        [Header("Toggle")]
        public ImageData toggleCheckMark;
        public ImageData toggleBackground;
        public TextData toggleLableText;

        [Header("Slider")]
        public ImageData sliderBackground;
        public ImageData sliderFill;
        public ImageData sliderKnob;

        [Header("DropDown")]
        public TextData ddSelectedText;
        public ImageData ddSelectedBackground;
        public ImageData ddArrow;
        public ImageData ddTemplateBackground;

        public ImageData ddItemCheckMark;
        public ImageData ddItemBackground;
        public TextData ddItemText;

        public ImageData ddSliderBackground;
        public ImageData ddSliderHandle;

        [Header("Inputfield")]
        public TextData inputFieldPlaceHolder;
        public InputFieldData inputFieldBackground;
        public TextData inputFieldText;


    }
    
}
