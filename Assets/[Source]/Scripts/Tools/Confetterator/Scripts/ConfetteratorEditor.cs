#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class ConfetteratorEditor : OdinEditorWindow
{
    [MenuItem("Tools/Confetterator")]
    private static void Open()
    {
        //RPGEditorWindow window = GetWindow<RPGEditorWindow>();
        //window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
    }
}

#endif