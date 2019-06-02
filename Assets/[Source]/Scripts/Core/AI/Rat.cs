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
    float minDistance = .75f;
    bool grabbed;

    public void Init()
    {
        agent = GetComponent<NavMeshAgent>();

        returnLoc = transform.position;

        agent.SetDestination(target.transform.position);
    }

    private void Update()
    {
        if(agent != null)
        {
            if (!grabbed)
            {
                if (Vector3.Distance(target.transform.position, transform.position) <= minDistance)
                {
                    grabbed = true;

                    target.GetComponent<Rigidbody>().isKinematic = true;
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
                }
            }
        }
    }

    public void IsPickedUp()
    {
        grabbed = false;
        target.transform.parent = null;
        target.GetComponent<Rigidbody>().isKinematic = false;
    }
}
