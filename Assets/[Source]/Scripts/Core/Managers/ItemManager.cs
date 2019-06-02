using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Extensions;
using Utilities;

public class ItemManager : EnsuredSingleton<ItemManager>
{
    //private Pipe lightRay;

    public Transform requestPosition;
    private Vector3 requestCapsuleBottom, requestCapsuleTop;
    public float requestPositionRadius;

    public Transform spawnPosition;
    public float spawnRadius;

    public GameObject baseItemPrefab;

    public LayerMask itemLayerMask;

    public float dropForce;

    WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.2f);

    public List<Item> globalItems = new List<Item>();
    public List<Item> targetedItems = new List<Item>();

    [HideInInspector] public delegate void DegradeCallback();
    public event DegradeCallback degradeCallback;
    public void CallEventDegradeCallback()
    {
        if(degradeCallback != null)
        {
            degradeCallback();
        }
    }

    private void Start()
    {
        Invoke("SetInitialReferences",0.05f);
    }

    private void SetInitialReferences()
    {
        requestCapsuleBottom = requestPosition.position - Vector3.up;
        requestCapsuleTop = requestPosition.position + Vector3.up;

        //lightRay = FindObjectOfType<Pipe>();
    }

    public void CreateItems(List<ItemType> items, List<int> counts, int mutationChance)
    {
        StartCoroutine(ActivateItem(items, counts, mutationChance));
    }

    public void RemoveItems(List<ItemType> items, List<int> counts)
    {
        List<int> itemCounter = counts;

        Collider[] colliders = UnityEngine.Physics.OverlapCapsule(requestCapsuleTop, requestCapsuleBottom, requestPositionRadius, itemLayerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Item removeItem = colliders[i].GetComponent<Item>();
            
            for(int j = 0; j < items.Count; j++)
            {
                if(removeItem.type == items[j].type)
                {
                    itemCounter[j]--;
                    break;
                }
            }
            
            DestroyItem(removeItem);
        }
        CalculateFinalScore(itemCounter);
    }

    public void DestroyItem(Item item)
    {
        globalItems.Remove(item);

        if(targetedItems.Contains(item))
        {
            targetedItems.Remove(item);
        }

        degradeCallback -= item.DoDegration;
        DestroyImmediate(item.gameObject);
    }

    private Pipe Lightray
    {
        get
        {
            return FindObjectOfType<Pipe>();
        }
    }
    private IEnumerator ActivateItem(List<ItemType> items, List<int> counts, int mutationChance)
    {
        yield return Lightray.Open(false);
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < counts[i]; j++)
            {
                //turn to inside unit circle on x and z
                Item newItem = ItemSpawner.Instance.SpawnItem(baseItemPrefab, spawnPosition.position + (Random.insideUnitSphere * spawnRadius)).GetComponent<Item>(); ;


                newItem.type = items[i].type;
                newItem.storageRoom = items[i].room;
                newItem.effects = items[i].effects;
                if (newItem.effects != ItemEffects.None)
                {
                    if (Random.Range(0, 100) < mutationChance)
                    {
                        //TODO Mutation
                    }

                    if (newItem.effects == ItemEffects.Fire)
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
                MeshCollider newMeshCollider = newItem.GetComponent<MeshCollider>();
                newMeshCollider.sharedMesh = items[i].mesh;
                newMeshCollider.convex = true;

                newItem.GetComponent<Rigidbody>().AddForce(Vector3.down * dropForce, ForceMode.Impulse);

                globalItems.Add(newItem);
                degradeCallback += newItem.DoDegration;
                yield return waitTime;
                
            }
        }
        yield return Lightray.Close();
    }

    private void CalculateFinalScore(List<int> counts)
    {
        foreach(int count in counts)
        {
            int satisfaction = 0;
            if (count == 0)
            {
                satisfaction = 10;
            }
            else
            {
                satisfaction = -(Mathf.Abs(count) * 5);
            }
            ScoreManager.Instance.ChangeSatisfaction(satisfaction);
        }
    }
}
