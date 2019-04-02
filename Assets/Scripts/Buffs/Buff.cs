using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class serves as a base class for all buffs.
/// </summary>
public abstract class Buff : ScriptableObject {

    public Sprite m_Sprite;
    public string m_PrettyName;
    [TextArea]
    public string m_Description;
    public float m_Cost;

    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }
        else
        {
            return GetType().Equals(other.GetType());
        }
    }
}
