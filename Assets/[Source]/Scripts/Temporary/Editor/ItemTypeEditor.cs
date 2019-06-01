using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemType))]
public class Test001Editor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        ItemType type = (ItemType)this.target;
        DrawDefaultInspector();
        if (GUI.changed) { Test001Editor.SaveData(type); }
    }

    private static void SaveData(ItemType test)
    {
        Debug.Log("Saved Item because it was changed");
        EditorUtility.SetDirty(test);
    }
}