using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour, ITile
{
    public void Build(GameObject tower)
    {
        // Do nothing, since this tile can't be built on
    }

    public bool IsBlocked()
    {
        return false;   // Path tiles will never have anything built on them
    }

    public bool IsBuildable()
    {
        return false;   // Path tiles can never be built on
    }
}
