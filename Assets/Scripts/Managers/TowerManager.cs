using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour {

    public static TowerManager Instance;

    public GameObject[] m_TowerPrefabs;
    public GameObject m_TowerButtonPrefab;
    public RectTransform m_TowerMenu;

    public BuffPanelList m_BuffMenu;

    public LayerMask m_TowerMask;
    public TowerInfoPanel m_TowerInfoPanel;

    private List<TowerButton> m_TowerButtons;

    private bool m_MouseUp;
    private int m_FingerID = -1;

    private void Awake()
    {
        Instance = this;

        m_TowerButtons = new List<TowerButton>();

        for(int i = 0; i < m_TowerPrefabs.Length; i++)
        {
            m_TowerPrefabs[i] = Instantiate(m_TowerPrefabs[i], transform);
            m_TowerPrefabs[i].SetActive(false);

            GameObject newButton = Instantiate(m_TowerButtonPrefab, m_TowerMenu);
            TowerButton button = newButton.GetComponent<TowerButton>();
            button.Init(m_TowerPrefabs[i].GetComponent<Tower>());
            m_TowerButtons.Add(button);
        }

#if !UNITY_EDITOR
        m_FingerID = 0;
#endif
    }

    private void Start()
    {
        Buff[] buffs = TowerBuffManager.Instance.UnlockedBuffs();
        foreach(Buff buff in buffs)
        {
            m_BuffMenu.AddBuff(buff);
        }

        TowerBuffManager.Instance.BuffUnlocked += OnBuffUnlocked;
    }

    private void Update()
    {
        m_MouseUp = Input.GetMouseButtonUp(0);
    }

    private void FixedUpdate()
    {
        if (!TowerBuilder.Instance.m_UsingMouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20, m_TowerMask);

            if (m_MouseUp)
            {
                if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject(m_FingerID))
                {
                    Tower tower = hit.collider.GetComponent<Tower>();
                    m_TowerInfoPanel.Setup(tower);
                }
                else if (!EventSystem.current.IsPointerOverGameObject(m_FingerID))
                {
                    m_TowerInfoPanel.Close();
                }
            }
        }
    }

    public void ApplyUpgrade(Tower tower, int buffIndex, Buff buff)
    {
        if (buffIndex >= tower.m_Buffs.Length)
        {
            Buff[] buffArray = new Buff[buffIndex + 1];

            for (int i = 0; i < tower.m_Buffs.Length; i++)
            {
                buffArray[i] = tower.m_Buffs[i];
            }

            tower.m_Buffs = buffArray;
        }

        for(int i = 0; i < tower.m_Buffs.Length; i++)
        {

            if (tower.m_Buffs[i] != null && buff.GetType().Equals(tower.m_Buffs[i].GetType()))
            {
                // This type of buff already exists on this tower, so remove the old one
                tower.m_Buffs[i] = null;
            }
        }

        tower.m_Buffs[buffIndex] = Instantiate(buff);

        if (tower.m_Template == null && m_TowerInfoPanel.gameObject.activeSelf)
        {
            // This buff was added to a template and the tower info panel is open
            m_TowerInfoPanel.Refresh();
        }
    }

    public void UpgradeToTemplate(Tower tower)
    {
        for(int i = 0; i < tower.m_Template.m_Buffs.Length; i++)
        {
            if (tower.m_Template.m_Buffs[i] != null)
            {
                ApplyUpgrade(tower, i, tower.m_Template.m_Buffs[i]);
            }
        }
    }

    public void ResetToInitialState()
    {
        // Clear out buff menu
        m_BuffMenu.Clear();

        Buff[] buffs = TowerBuffManager.Instance.UnlockedBuffs();
        foreach (Buff buff in buffs)
        {
            m_BuffMenu.AddBuff(buff);
        }

        // Clear buffs from tower templates
        foreach (GameObject template in m_TowerPrefabs)
        {
            Tower tower = template.GetComponent<Tower>();
            tower.m_Buffs = new Buff[tower.m_Buffs.Length];
        }

        // Refresh buttons to show no buffs
        foreach(TowerButton button in m_TowerButtons)
        {
            button.Refresh();
        }
    }

    private void OnBuffUnlocked(object sender, BuffEventArgs e)
    {
        m_BuffMenu.AddBuff(e.buff);
    }
}
