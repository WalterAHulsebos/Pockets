using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemRoom
{
    Freezer,
    Armory,
    Library,
    Bank,
    Trophy
}

[System.Flags, System.Serializable]
public enum ItemEffects
{
    None = 0x0,         //00000000
    Fire = 0x1,         //00000001    
    Poison = 0x2,       //00000010
    Freeze = 0x4,       //00000100
    //TODO: Add more effects if needed
}


