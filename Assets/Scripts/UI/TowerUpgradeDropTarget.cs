using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerUpgradeDropTarget : MonoBehaviour, IDropHandler {

    public int m_BuffIndex;

    private Tower m_Tower;
    private TowerButton m_TowerButton;

    public void Init(Tower tower, TowerButton towerButton)
    {
        m_Tower = tower;
        m_TowerButton = towerButton;
    }

    public void OnDrop(PointerEventData eventData)
    {
        BuffPanel buffPanel = eventData.pointerDrag.GetComponent<BuffPanel>();

        if (buffPanel != null) {
            TowerManager.Instance.ApplyUpgrade(m_Tower, m_BuffIndex, buffPanel.m_Buff);
            m_TowerButton.Refresh();
        }
    }
}
