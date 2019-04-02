using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour {

    public BuffPanelList m_BuffList;
    public int m_BuffsShown;

    private System.Random m_Rand;

    private void Awake()
    {
        m_Rand = new System.Random();
    }

    public void Init()
    {
        m_BuffList.Clear();

        List<Buff> lockedBuffs = new List<Buff>(TowerBuffManager.Instance.LockedBuffs());
        List<Buff> buffs = new List<Buff>();

        int numBuffsShown = Mathf.Min(m_BuffsShown, lockedBuffs.Count);

        for (int i = 0; i < numBuffsShown; i++)
        {
            int index = m_Rand.Next(lockedBuffs.Count);
            buffs.Add(lockedBuffs[index]);
            lockedBuffs.RemoveAt(index);
        }

        foreach(Buff buff in buffs)
        {
            m_BuffList.AddBuff(buff);
        }
    }
}
