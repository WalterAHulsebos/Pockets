using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable,  CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : MonoBehaviour, IInteractable
{
    #region Variables

    public string name;
    public GameObject prefab;
    public ItemRoom room;
    public ItemEffects effects;
    public float degradePerTick;

    [HideInInspector] public float integity = 100;
    [HideInInspector] public Container container;

    #endregion

    public void DoDegration()
    {
        if(container != null)
        {
            if(container.IsInCorrectContainer(this))
            {
                integity -= degradePerTick;
            }
        }
    }
}

