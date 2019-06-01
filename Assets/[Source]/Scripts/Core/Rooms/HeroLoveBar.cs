using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLoveBar : MonoBehaviour
{
    [SerializeField] Transform m_BarTransform = null;

    // Input in range 0 - 1
    public void SetHeroLove(float value)
    {
        value = Mathf.Clamp01(value);
        var scale = m_BarTransform.localScale;
        scale.x = value;
        m_BarTransform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.H))
        {
            SetHeroLove(0.5f);
        }*/

        // TODO: Poll score manager to get the hero's opinion/love and then set the value using SetHeroLove function
    }
}
