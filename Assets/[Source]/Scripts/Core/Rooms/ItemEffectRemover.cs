using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffectRemover : MonoBehaviour
{
    public ItemEffects inEffect;
    public ParticleSystem effectParticleSystem;

    public void RemoveItemEffect(Item item)
    {
        if((item.effects & inEffect) != 0)
        {
            item.effects &= ~inEffect;
        }

        if(effectParticleSystem != null)
        {
            effectParticleSystem.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Item otherItem = other.GetComponent<Item>();
        if(otherItem != null)
        {
            RemoveItemEffect(otherItem);
        }
    }
}
