using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.Extensions;

public class ContainerSpawner : EnsuredSingleton<ContainerSpawner>
{
    public GameObject containerPrefab;
    public float spawnTime;

    public GameObject currentContainer;
    private bool isSpawning = false;

    private void Update()
    {
        if(currentContainer == null &&
            !isSpawning)
        {
            isSpawning = true;
            Invoke("SpawnContainer", spawnTime);
        }
    }

    private void SpawnContainer()
    {
        currentContainer = Instantiate(containerPrefab, transform.position, transform.rotation);
        Container container = currentContainer.GetComponent<Container>();
        isSpawning = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Container>())
        {
            currentContainer = null;
        }
    }
}
