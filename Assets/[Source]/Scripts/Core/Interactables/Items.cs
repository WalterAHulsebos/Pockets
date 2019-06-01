using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour, IInteractable
{
    #region Variables

    public string type;
    public GameObject prefab;
    public ItemRoom storageRoom;
    public ItemEffects effects;
    public float degradePerTick;

    [HideInInspector] public float integity = 100;
    private float integrityBuffer = 5;
    [HideInInspector] public Container container;

    #endregion

    public void DoDegration()
    {
        if(container != null)
        {
            if(!container.IsInCorrectRoom(this))
            {
                if (integrityBuffer > 0)
                {
                    integrityBuffer -= 1;
                }
                else
                {
                    integity -= degradePerTick;
                }
            }
        }
        else
        {
            if (integrityBuffer > 0)
            {
                integrityBuffer -= 1;
            }
            else
            {
                integity -= degradePerTick;
            }
        }
    }
}

