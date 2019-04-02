using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class Wave : ScriptableObject {

    [Serializable]
	public struct Phase
    {
        public GameObject m_Enemy;
        [Range(0f, 1f)]
        public float m_PortionOfWave;
        public ScalingStat m_SpawnRate;
    }

    public Phase[] m_Phases;
    public float m_SizeMultiplier = 1.0f;

    public float Duration(int waveSize)
    {
        float duration = 0;

        foreach (Phase p in m_Phases)
        {
            float spawns = Mathf.CeilToInt(waveSize * m_SizeMultiplier * p.m_PortionOfWave);
            duration += spawns / p.m_SpawnRate;
        }

        return duration;
    }
}
