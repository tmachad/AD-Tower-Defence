using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuffManager : MonoBehaviour {

    public static TowerBuffManager Instance;

    public Buff[] m_Buffs;

    public event EventHandler<BuffEventArgs> BuffUnlocked;

    [SerializeField]
    private bool m_UnlockAllBuffs;
    private System.Random m_Rand;
    private List<Buff> m_UnlockedBuffs;
    private List<Buff> m_LockedBuffs;

    private void Awake()
    {
        Instance = this;

        m_Rand = new System.Random();
        m_UnlockedBuffs = new List<Buff>();
        m_LockedBuffs = new List<Buff>();

        foreach (Buff buff in m_Buffs)
        {
            if (m_UnlockAllBuffs)
            {
                m_UnlockedBuffs.Add(buff);
            } else
            {
                m_LockedBuffs.Add(buff);
            }
        }
    }

    public void UnlockRandomBuff()
    {
        if (m_LockedBuffs.Count > 0)
        {
            int index = m_Rand.Next(m_LockedBuffs.Count);
            Buff buff = m_LockedBuffs[index];

            UnlockBuff(buff);
        }
    }

    public void UnlockBuff(Buff buff)
    {
        if (m_LockedBuffs.Contains(buff))
        {
            m_LockedBuffs.Remove(buff);
            m_UnlockedBuffs.Add(buff);

            BuffEventArgs eventArgs = new BuffEventArgs()
            {
                buff = buff
            };
            OnBuffUnlocked(eventArgs);
        }
    }

    public Buff[] UnlockedBuffs()
    {
        return m_UnlockedBuffs.ToArray();
    }

    public Buff[] LockedBuffs()
    {
        return m_LockedBuffs.ToArray();
    }

    protected virtual void OnBuffUnlocked(BuffEventArgs e)
    {
        if (BuffUnlocked != null)
        {
            BuffUnlocked.Invoke(this, e);
        }
    }
}

public class BuffEventArgs : EventArgs
{
    public Buff buff;
}