using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable,  CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject, IInteractable
{
    #region Variables

    public string name;
    public GameObject prefab;
    public ItemRoom room;
    public ItemEffects effects;
    public float degradingSpeed = 0.001f, degradingDelay = 5f;

    [HideInInspector] public float integity = 100;
    [HideInInspector] public bool stored = false;

    #endregion
}

