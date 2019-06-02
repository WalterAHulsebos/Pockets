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
        
    }

    private void SpawnContainer()
    {
        currentContainer = Instantiate(containerPrefab, transform.position, transform.rotation);
        isSpawning = true;
    }
    
}
