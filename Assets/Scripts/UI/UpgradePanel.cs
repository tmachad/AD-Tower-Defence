using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour {

    public RectTransform m_BuffList;
    public GameObject m_BuffPanelPrefab;

    public void Setup(Buff[] buffs)
    {
        foreach(Buff buff in buffs)
        {
            GameObject buffPanel = Instantiate(m_BuffPanelPrefab, m_BuffList);
            buffPanel.GetComponent<BuffPanel>().Init(buff, false);
        }
    }
}
