using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTile : MonoBehaviour, ITile {

    [HideInInspector]
    public Vector2[] m_Waypoints;

    private void Start()
    {
        WaveManager.Instance.m_Spawner = this;
    }

    public IEnumerator Spawn(Wave wave, float waveSize)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z -= 1;    // Move enemies a bit closer to the camera so they always show above the tiles

        foreach (Wave.Phase phase in wave.m_Phases)
        {
            float spawnDelay = 1.0f / phase.m_SpawnRate;
            
            for (int spawnCount = Mathf.CeilToInt(waveSize * wave.m_SizeMultiplier * phase.m_PortionOfWave); spawnCount > 0; spawnCount--)
            {
                GameObject obj = Instantiate(phase.m_Enemy);
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Initialize(spawnPos, m_Waypoints);

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        // Spawning has finished, so allow the player to force start the next wave
        WaveManager.Instance.m_NextWaveButton.SetActive(true);
    }

    #region Tile Behaviours
    public void Build(GameObject tower)
    {
        // Do nothing, since this tile can't be built on
    }

    public bool IsBlocked()
    {
        return false;   // Spawner tiles will never have anything built on them
    }

    public bool IsBuildable()
    {
        return false;   // Spawner tiles can never be built on
    }
    #endregion
}
