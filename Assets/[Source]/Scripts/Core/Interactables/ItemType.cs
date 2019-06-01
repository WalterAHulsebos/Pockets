using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item")]
public class ItemType : ScriptableObject
{
    public Mesh mesh;
    public Material material;
    public ItemRoom room;
    public ItemEffects effects;
    public float degradePerTick;
}
