using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Composite")]
public class CompositeAudioEvent : AudioEvent
{
	[Serializable]
	public struct CompositeEntry
	{
		public AudioEvent audioEvent;
		public float weight;
	}

	public CompositeEntry[] entries;

	public override void Play(AudioSource source)
	{
		float totalWeight = 0;
		for (int i = 0; i < entries.Length; ++i)
		{
			totalWeight += entries[i].weight;
		}

		float pick = Random.Range(0, totalWeight);
		for (int i = 0; i < entries.Length; ++i)
		{
			if (pick > entries[i].weight)
			{
				pick -= entries[i].weight;
				continue;
			}

			entries[i].audioEvent.Play(source);
			return;
		}
	}
}