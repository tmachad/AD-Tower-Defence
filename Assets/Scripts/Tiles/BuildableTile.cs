using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTile : MonoBehaviour, ITile {

    private GameObject m_Tower;

    public void Build(GameObject tower)
    {
        Vector3 towerPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);   // Place tower closer to the camera in the z direction so it shows above the tile
        m_Tower = Instantiate(tower, towerPos, Quaternion.identity, transform);
        m_Tower.GetComponent<Tower>().m_Template = tower.GetComponent<Tower>();
        m_Tower.SetActive(true);
    }

    public bool IsBlocked()
    {
        return m_Tower != null;
    }

    public bool IsBuildable()
    {
        return m_Tower == null;
    }
}
