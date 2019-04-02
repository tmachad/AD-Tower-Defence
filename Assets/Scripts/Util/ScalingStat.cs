using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScalingStat {

    public enum ScalingMode
    {
        Add,
        Multiply,
        CompoundMultiply
    }

    public float m_BaseValue;
    public float m_Increment;
    public ScalingFactor m_ScalingFactor;
    public ScalingMode m_ScalingMode;

    public float Value
    {
        get
        {
            //System.Diagnostics.StackTrace s = new System.Diagnostics.StackTrace();
            //Debug.Log(s.ToString());

            if (m_ScalingMode == ScalingMode.Add)
            {
                return m_BaseValue + m_Increment * m_ScalingFactor.GetScalingFactor();
            }
            else if (m_ScalingMode == ScalingMode.Multiply)
            {
                return m_BaseValue * (1 + m_Increment * m_ScalingFactor.GetScalingFactor());
            }
            else if (m_ScalingMode == ScalingMode.CompoundMultiply)
            {
                return m_BaseValue * Mathf.Pow(1 + m_Increment, m_ScalingFactor.GetScalingFactor());
            }

            return 0;
        }
    }

    public ScalingStat(ScalingFactor scalingFactor)
    {
        m_ScalingFactor = scalingFactor;
    }

    // Implicit conversion to float to make it easier to use in code
    public static implicit operator float(ScalingStat s)
    {
        return s.Value;
    }
}
