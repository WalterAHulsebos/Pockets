using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour, IInteractable
{
    #region Variables

    public string type;
    public ItemRoom storageRoom;
    public ItemEffects effects;
    public float degradePerTick;

    public float integity = 100;
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
        if(integity <= 0)
        {
            ScoreManager.Instance.ChangeSatisfaction(5);
            ItemManager.Instance.DestroyItem(this);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Container container = other.GetComponent<Container>();
        if (container != null)
        {
            container.RemoveItemFromContainer(this);
        }
        else
        {
            container = other.GetComponentInParent<Container>();
            if (container != null)
            {
                container.RemoveItemFromContainer(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Container container = other.GetComponent<Container>();
        if (container != null)
        {
            container.AddItemToContainer(this);
        }
        else
        {
            container = other.GetComponentInParent<Container>();
            if (container != null)
            {
                container.AddItemToContainer(this);
            }
        }
    }
}

