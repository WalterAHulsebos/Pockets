using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Extensions;
using Utilities;

public class ItemManager : PersistentSingleton<ItemManager>
{
    public Transform requestPosition;
    private Vector3 requestCapsuleBottom, requestCapsuleTop;
    public float requestPositionRadius;

    public Transform spawnPosition;
    public float spawnRadius;

    public GameObject baseItemPrefab;

    LayerMask itemLayerMask;



    public List<Item> globalItems = new List<Item>();

    [HideInInspector] public delegate void DegradeCallback();
    public event DegradeCallback degradeCallback;
    public void CallEventDegradeCallback()
    {
        if(degradeCallback != null)
        {
            degradeCallback();
        }
    }

    private void Awake()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        requestCapsuleBottom = requestPosition.position - Vector3.up;
        requestCapsuleTop = requestPosition.position + Vector3.up;
    }

    public void CreateItems(List<ItemType> items, List<int> counts, int mutationChance)
    {
        for(int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < counts[i]; j++)
            {
                //turn to inside unit circle on x and z
                Item newItem = Instantiate(baseItemPrefab, spawnPosition.position + (Random.insideUnitSphere * spawnRadius), Quaternion.identity).GetComponent<Item>();
                newItem.type = items[i].type;
                newItem.storageRoom = items[i].room;
                newItem.effects = items[i].effects;
                if (newItem.effects != ItemEffects.None)
                {
                    if (Random.Range(0, 100) < mutationChance)
                    {
                        //TODO Mutation
                    }

                    if(newItem.effects == ItemEffects.Fire)
                    {
                        ItemEffectFactory.Instance.CreateFireEffect(newItem.transform);
                    }
                    if (newItem.effects == ItemEffects.Poison)
                    {
                        ItemEffectFactory.Instance.CreatePoisonEffect(newItem.transform);
                    }
                    if (newItem.effects == ItemEffects.Freeze)
                    {
                        ItemEffectFactory.Instance.CreateFrozenEffect(newItem.transform);
                    }
                }

                newItem.degradePerTick = items[i].degradePerTick;

                newItem.GetComponent<MeshFilter>().mesh = items[i].mesh;
                newItem.GetComponent<MeshRenderer>().material = items[i].material;
                globalItems.Add(newItem);
                degradeCallback += newItem.DoDegration;
            }
        }
    }

    public void RemoveItems(List<ItemType> items, List<int> counts)
    {
        List<int> itemCounter = counts;

        Collider[] colliders = UnityEngine.Physics.OverlapCapsule(requestCapsuleTop, requestCapsuleBottom, requestPositionRadius, itemLayerMask);

        for(int i = 0; i < colliders.Length; i++)
        {
            Item removeItem = colliders[i].GetComponent<Item>();
            if(removeItem == null)
            {
                continue;
            }
            
            for(int j = 0; j < items.Count; j++)
            {
                if(removeItem.type == items[j].type)
                {
                    itemCounter[j]--;
                    break;
                }
            }

            globalItems.Remove(removeItem);
            degradeCallback -= removeItem.DoDegration;
            Destroy(removeItem);
        }
        CalculateFinalScore(itemCounter);
    }

    private void CalculateFinalScore(List<int> counts)
    {
        foreach(int count in counts)
        {
            int satisfaction = count == 0 ? 10 : -10;
            ScoreManager.Instance.ChangeSatisfaction(satisfaction);
        }
    }
}
