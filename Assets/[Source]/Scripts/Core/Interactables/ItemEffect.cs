using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class ItemEffectFactory : PersistentSingleton<ItemEffectFactory>
{
    public GameObject fireParticleSystemPrefab;
    public GameObject poisonParticleSystemPrefab;
    public GameObject frozenParticleSystemPrefab;

    public GameObject CreateFireEffect(Transform parent)
    {
        GameObject newFireEffect = Instantiate(fireParticleSystemPrefab, parent);
        newFireEffect.transform.localPosition = Vector3.zero;
        return newFireEffect;
    }

    public GameObject CreatePoisonEffect(Transform parent)
    {
        GameObject newPoisonEffect = Instantiate(fireParticleSystemPrefab, parent);
        newPoisonEffect.transform.localPosition = Vector3.zero;
        return newPoisonEffect;
    }

    public GameObject CreateFrozenEffect(Transform parent)
    {
        GameObject newFrozenEffect = Instantiate(fireParticleSystemPrefab, parent);
        newFrozenEffect.transform.localPosition = Vector3.zero;
        return newFrozenEffect;
    }
}
