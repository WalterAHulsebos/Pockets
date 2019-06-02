using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ItemType))]
public class ItemTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        ItemType type = (ItemType)this.target;
        DrawDefaultInspector();
        if (GUI.changed) { SaveData(type); }
    }

    private static void SaveData(ItemType test)
    {
        Debug.Log("Saved Item because it was changed");
        EditorUtility.SetDirty(test);
    }
}