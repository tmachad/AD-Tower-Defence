using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPanelList : MonoBehaviour {

    public RectTransform m_BuffList;
    public GameObject m_BuffPanelPrefab;

    public void AddBuff(Buff buff, InteractionMode interactionMode = InteractionMode.None)
    {
        GameObject buffPanel = Instantiate(m_BuffPanelPrefab, m_BuffList);
        buffPanel.GetComponent<BuffPanel>().Init(buff, interactionMode);
    }

    public void Clear()
    {
        foreach (Transform child in m_BuffList)
        {
            Destroy(child.gameObject);
        }
    }
}
