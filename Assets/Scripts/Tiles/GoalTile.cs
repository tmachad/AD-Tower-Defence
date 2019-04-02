using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : MonoBehaviour, ITile {

    public void Build(GameObject tower)
    {
        // Do nothing, since this tile can't be built on
    }

    public bool IsBlocked()
    {
        return false;   // Goal tiles will never have anything built on them
    }

    public bool IsBuildable()
    {
        return false;   // Goal tiles can never be built on
    }
}
