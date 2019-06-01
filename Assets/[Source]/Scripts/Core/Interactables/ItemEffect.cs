using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class ItemEffectFactory : PersistentSingleton<ItemEffectFactory>
{
    public ParticleSystem fireParticleSystem;
    public ParticleSystem poisonParticleSystem;
    public ParticleSystem frozenParticleSystem;

}
