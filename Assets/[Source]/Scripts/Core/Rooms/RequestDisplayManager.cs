﻿using System.Collections;
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
        ItemEvent itemEvent = GameManager.Instance.GetActiveRequestEvent();

        if (itemEvent != null)
        {
            SetTimer(itemEvent.timeToExecute);

            SetDisplays(itemEvent);
        }
        else
        {
            HideRequest();
        }
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

    void SetDisplays(ItemEvent itemEvent)
    {
        int itemCount = itemEvent.items.Count;

        m_RequestDisplays[0].gameObject.SetActive(false);
        m_RequestDisplays[1].gameObject.SetActive(false);
        m_RequestDisplays[2].gameObject.SetActive(false);

        if (itemCount > 0)
        {
            m_RequestDisplays[0].gameObject.SetActive(true);
            m_RequestDisplays[0].SetUp(itemEvent.items[0].type, itemEvent.counts[0]);
        }

        if (itemCount > 1)
        {
            m_RequestDisplays[1].gameObject.SetActive(true);
            m_RequestDisplays[1].SetUp(itemEvent.items[1].type, itemEvent.counts[1]);
        }

        if (itemCount > 2)
        {
            m_RequestDisplays[2].gameObject.SetActive(true);
            m_RequestDisplays[2].SetUp(itemEvent.items[2].type, itemEvent.counts[2]);
        }
    }
}
