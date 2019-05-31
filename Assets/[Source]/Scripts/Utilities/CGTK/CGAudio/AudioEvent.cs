using UnityEngine;

#if ODIN_INSPECTOR
using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
#endif

public abstract class AudioEvent : ScriptableObject
{
	public abstract void Play(AudioSource source);
}