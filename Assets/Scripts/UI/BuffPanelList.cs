using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuffPanelList : MonoBehaviour {

    public RectTransform m_BuffList;
    public GameObject m_BuffPanelPrefab;

    public void AddBuff(Buff buff, InteractionMode interactionMode = InteractionMode.None, UnityAction onInteraction = null)
    {
        GameObject buffPanel = Instantiate(m_BuffPanelPrefab, m_BuffList);
        buffPanel.GetComponent<BuffPanel>().Init(buff, interactionMode, onInteraction);
    }

    public void Clear()
    {
        foreach (Transform child in m_BuffList)
        {
            Destroy(child.gameObject);
        }
    }
}
