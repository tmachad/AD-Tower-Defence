using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPanel : MonoBehaviour {

    public Image m_TowerImage;
    public Button m_UpgradeButton;
    public Button m_SellButton;
    public Dropdown m_TargetDropdown;

    public BuffPanelList m_BuffList;

    private Tower m_Tower;

    public void Setup(Tower tower)
    {
        if (m_Tower != null)
        {
            m_Tower.ShowRangeIndicator(false);  // If switching to another tower without closing, turn off indicator for old tower
        }

        tower.ShowRangeIndicator(true);
        m_Tower = tower;

        gameObject.SetActive(true);

        m_BuffList.Clear();

        m_TowerImage.sprite = tower.GetComponent<SpriteRenderer>().sprite;

        foreach(Buff buff in tower.m_Buffs)
        {
            if (buff != null)
            {
                m_BuffList.AddBuff(buff);
            }
        }

        m_TargetDropdown.value = (int)tower.m_TargetPriority;

        bool buffsMatch = true;
        for(int i = 0; i < tower.m_Buffs.Length && i < tower.m_Template.m_Buffs.Length; i++)
        {
            Buff towerBuff = tower.m_Buffs[i];
            Buff templateBuff = tower.m_Template.m_Buffs[i];

            if ((towerBuff != null && templateBuff == null) || (towerBuff == null && templateBuff))
            {
                // One of the buffs is null but the other isn't
                buffsMatch = false;
            }
            else if (towerBuff != null && templateBuff != null && !towerBuff.GetType().Equals(templateBuff.GetType()))
            {
                // Both buffs exist but are not the same type
                buffsMatch = false;
            }
        }

        m_UpgradeButton.gameObject.SetActive(!buffsMatch);

        if (!buffsMatch)
        {
            List<Buff> towerBuffs = new List<Buff>(tower.m_Buffs);

            float cost = 0;
            foreach (Buff buff in tower.m_Template.m_Buffs)
            {
                if (buff != null && !towerBuffs.Contains(buff))
                {
                    cost += buff.m_Cost;
                }
            }

            cost = Mathf.Max(cost, 0);

            m_UpgradeButton.GetComponentInChildren<Text>().text = string.Format("Upgrade (${0})", Math.Round(cost, 2));

            m_UpgradeButton.onClick.RemoveAllListeners();
            m_UpgradeButton.onClick.AddListener(() =>
            {
                if (GameManager.Instance.m_Money >= cost)
                {
                    TowerManager.Instance.UpgradeToTemplate(tower);
                    GameManager.Instance.m_Money -= cost;
                    Refresh();
                }
            });
        }

        m_SellButton.GetComponentInChildren<Text>().text = string.Format("Sell (${0})", Math.Round(tower.m_Cost / 2, 2));
    }

    public void ChangeTowerTarget(Dropdown dropdown)
    {
        m_Tower.m_TargetPriority = (Tower.TargetPriority)dropdown.value;
    }

    public void Refresh()
    {
        Setup(m_Tower);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (m_Tower != null)
        {
            m_Tower.ShowRangeIndicator(false);
        }
        m_Tower = null;
    }

    public void SellTower()
    {
        GameManager.Instance.m_Money += m_Tower.m_Cost / 2;
        Destroy(m_Tower.gameObject);
        Close();
    }
}
