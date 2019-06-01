using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequestDisplay : MonoBehaviour
{
    [SerializeField] TextMeshPro m_QuantityLabel = null;
    [SerializeField] TextMeshPro m_NameLabel = null;

    public void SetUp(string name, int quantity)
    {
        m_NameLabel.text = name;
        m_QuantityLabel.text = quantity.ToString();
    }
}
