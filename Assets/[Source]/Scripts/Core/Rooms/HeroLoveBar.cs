using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLoveBar : MonoBehaviour
{
    [SerializeField] Transform m_BarTransform = null;
	[SerializeField] Renderer m_InnerRenderer = null;
	[SerializeField] Color m_HealthyColor = Color.green;
	[SerializeField] Color m_UnhealthyColor = Color.red;

	// Input in range 0 - 1
	public void SetHeroLove(float value)
    {
        value = Mathf.Clamp01(value);
        var scale = m_BarTransform.localScale;
        scale.x = value;
        m_BarTransform.localScale = scale;

		m_InnerRenderer.material.color = Color.Lerp(m_UnhealthyColor, m_HealthyColor, value);
	}

    void Update()
    {
        if (ScoreManager.Instance != null)
        {
            float heroLove = (ScoreManager.Instance.heroSatisfactionRating / 100.0f); // Needs to be between 0 and 1
            SetHeroLove(heroLove);
        }
    }
}