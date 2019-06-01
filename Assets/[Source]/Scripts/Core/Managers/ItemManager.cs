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


    /* TODO: [Wybren]
     * Keep track of all global items
     * Add items to world\
     * remove / request items from world
     */

    public List<Item> globalItems = new List<Item>();
    [SerializeField] private Object[] allTypes;

    [HideInInspector] public delegate void DegradeCallback();
    public event DegradeCallback degradeCallback;

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
                Item newItem = Instantiate(baseItemPrefab, spawnPosition.position + (Vector3)(Random.insideUnitCircle * spawnRadius), Quaternion.identity).GetComponent<Item>();
                newItem.type = items[i].type;
                newItem.storageRoom = items[i].room;
                newItem.effects = items[i].effects;
                if (Random.Range(0, 100) < mutationChance)
                {
                    //TODO Mutation
                }

                newItem.degradePerTick = items[i].degradePerTick;

                newItem.GetComponent<MeshFilter>().mesh = items[i].mesh;
                newItem.GetComponent<MeshRenderer>().material = items[i].material;
            }
        }
    }

    public void RemoveItems(List<KeyValuePair<ItemType, int>> items)
    {
        List<int> itemCounter = new List<int>();
        foreach(KeyValuePair<ItemType, int> item in items)
        {
            itemCounter.Add(item.Value);
        }

        Collider[] colliders = UnityEngine.Physics.OverlapCapsule(requestCapsuleTop, requestCapsuleBottom, requestPositionRadius, itemLayerMask);

        for(int i = 0; i < colliders.Length; i++)
        {
            Item newItem = colliders[i].GetComponent<Item>();
            if(newItem == null)
            {
                continue;
            }
            
            for(int j = 0; j < items.Count; j++)
            {
                if(newItem.type == items[j].Key.type)
                {
                    itemCounter[j]--;
                    break;
                }
            }
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
