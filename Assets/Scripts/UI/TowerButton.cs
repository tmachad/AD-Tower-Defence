using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour {

    public Image m_Image;
    public Text m_CostText;
    public Button m_BuildButton;
    public Image[] m_BuffIcons;
    public Sprite m_EmptyBuffSprite;
    public Tower m_Tower;

    public void Init(Tower tower)
    {
        m_Tower = tower;

        m_Image.sprite = m_Tower.GetComponent<SpriteRenderer>().sprite;

        m_CostText.text = string.Format("${0}", m_Tower.m_Cost);

        for(int i = 0; i < m_BuffIcons.Length; i++)
        {
            if (i >= m_Tower.m_Buffs.Length || m_Tower.m_Buffs[i] == null)
            {
                m_BuffIcons[i].sprite = m_EmptyBuffSprite;
            }
            else
            {
                m_BuffIcons[i].sprite = m_Tower.m_Buffs[i].m_Sprite;
            }

            m_BuffIcons[i].GetComponent<TowerUpgradeDropTarget>().Init(m_Tower, this);
        }

        m_BuildButton.onClick.AddListener(() =>
        {
            TowerBuilder.Instance.SetSelectedTowerPrefab(m_Tower.gameObject);
        });
    }

    public void Refresh()
    {
        m_Image.sprite = m_Tower.GetComponent<SpriteRenderer>().sprite;

        m_CostText.text = string.Format("${0}", m_Tower.m_Cost);

        for (int i = 0; i < m_BuffIcons.Length; i++)
        {
            if (i >= m_Tower.m_Buffs.Length || m_Tower.m_Buffs[i] == null)
            {
                m_BuffIcons[i].sprite = m_EmptyBuffSprite;
            }
            else
            {
                m_BuffIcons[i].sprite = m_Tower.m_Buffs[i].m_Sprite;
            }
        }
    }
}
