using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
	[SerializeField] Light m_SpotLight = null;
	[SerializeField] float m_SpotLightOpenAngle = 44.0f;
	[SerializeField] float m_SpotLightOpenIntensity = 8.0f;
	[SerializeField] Color m_SpotLightSpawnColor = Color.white;
	[SerializeField] Color m_SpotLightRequestColor = Color.red;

	public IEnumerator Open(bool isRequest)
	{
		yield return null;

		if (isRequest)
		{
			m_SpotLight.color = m_SpotLightRequestColor;
		}
		else
		{
			m_SpotLight.color = m_SpotLightSpawnColor;
		}

		float amount = 0.0f;

		while (amount < 1.0f)
		{
			amount += Time.deltaTime;
			amount = Mathf.Clamp01(amount);

			m_SpotLight.spotAngle = m_SpotLightOpenAngle * amount;
			m_SpotLight.intensity = m_SpotLightOpenIntensity * amount;

			yield return null;
		}
	}

	public IEnumerator Close()
	{
		yield return null;

		float amount = 1.0f;

		while (amount > 0.0f)
		{
			amount -= Time.deltaTime;
			amount = Mathf.Clamp01(amount);

			m_SpotLight.spotAngle = m_SpotLightOpenAngle * amount;
			m_SpotLight.intensity = m_SpotLightOpenIntensity * amount;

			yield return null;
		}
	}
}
