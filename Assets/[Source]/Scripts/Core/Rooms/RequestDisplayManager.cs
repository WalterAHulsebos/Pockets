using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequestDisplayManager : MonoBehaviour
{
    [SerializeField] TextMeshPro m_TimerLabel = null;

    [SerializeField] RequestDisplay[] m_RequestDisplays = null;

    private void Awake()
    {
        HideRequest();
    }

    void Update()
    {
        // TODO: Poll
    }

    void HideRequest()
    {
        m_TimerLabel.gameObject.SetActive(false);

        m_RequestDisplays[0].gameObject.SetActive(false);
        m_RequestDisplays[1].gameObject.SetActive(false);
        m_RequestDisplays[2].gameObject.SetActive(false);
    }

    void SetTimer(int time)
    {
        m_TimerLabel.gameObject.SetActive(true);
        m_TimerLabel.text = time.ToString();
    }

    void SetDisplays(int itemCount)
    {
        m_RequestDisplays[0].gameObject.SetActive(false);
        m_RequestDisplays[1].gameObject.SetActive(false);
        m_RequestDisplays[2].gameObject.SetActive(false);

        if (itemCount > 0)
        {
            m_RequestDisplays[0].gameObject.SetActive(true);
            m_RequestDisplays[0].SetUp("", 0);
        }

        if (itemCount > 1)
        {
            m_RequestDisplays[1].gameObject.SetActive(true);
            m_RequestDisplays[1].SetUp("", 0);
        }

        if (itemCount > 2)
        {
            m_RequestDisplays[2].gameObject.SetActive(true);
            m_RequestDisplays[2].SetUp("", 0);
        }
    }
}
