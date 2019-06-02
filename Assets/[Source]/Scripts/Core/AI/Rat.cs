using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat : MonoBehaviour
{
    public Item target;
    public Vector3 returnLoc;
    public Transform itemLoc;

    NavMeshAgent agent;
    Rigidbody rb;
    float minDistance = .75f;
    bool grabbed, pickedup;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        Retarget();
        returnLoc = transform.position;

        
    }

    private void Update()
    {
        if (target == null && pickedup == false)
        {
            Retarget();
        }

        if (agent != null && pickedup != true && target != null)
        {
            if (target.container != null)
            {
                Retarget();
            }

            if (!grabbed)
            {
                if (Vector3.Distance(target.transform.position, transform.position) <= minDistance)
                {
                    grabbed = true;

                    target.GetComponent<Rigidbody>().isKinematic = true;
                    target.GetComponent<MeshCollider>().enabled = false;
                    target.transform.position = itemLoc.position;
                    target.transform.parent = itemLoc;

                    agent.SetDestination(returnLoc);
                }
            }
            else
            {
                if(Vector3.Distance(returnLoc, transform.position) < minDistance)
                {
                    ItemManager.Instance.DestroyItem(target);
                    ScoreManager.Instance.ChangeSatisfaction(-5);
                    Destroy(gameObject);
                    Debug.Log("Yeet");
                    ScoreManager.Instance.stolenItems++;
                }
            }
        }
    }

    public void IsPickedUp()
    {
        pickedup = true;
        grabbed = false;
        
        agent.isStopped = true;
        agent.enabled = false;

        ItemManager.Instance.targetedItems.Remove(target);

        if (target != null)
        {
            
            target.transform.parent = null;
            target.GetComponent<MeshCollider>().enabled = true;
            target.GetComponent<Rigidbody>().isKinematic = false;
            target = null;
        }
    }

    public void Retarget()
    {
        List<Item> unstoredItems = ItemManager.Instance.globalItems.FindAll(x => x.container == null);

        if(unstoredItems.Count == 0)
        {
            Destroy(gameObject);
        }

        foreach(Item item in unstoredItems)
        {
            if(!ItemManager.Instance.targetedItems.Contains(item))
            {
                ItemManager.Instance.targetedItems.Add(item);        
                target = item;
                agent.SetDestination(item.transform.position);

                break;
            }
        }
    }

    public IEnumerator ResetRat()
    {
        while(rb.velocity.y != 0)
        {
            yield return null;
        }

        pickedup = false;

        rb.isKinematic = true;
        agent.enabled = true;
        agent.isStopped = false;

        Retarget();
    }
}
