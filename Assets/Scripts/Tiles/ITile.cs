using UnityEngine;

public interface ITile {

    /// <summary>
    /// Tells the tile to build the given tower prefab on itself. Non-buildable tiles will ignore calls to Build(), so make sure to check IsBuildable() first.
    /// </summary>
    /// <param name="tower">The tower to build on this tile.</param>
    void Build(GameObject tower);

    /// <summary>
    /// Indicates whether or not this tile is blocked by something else already being built on it.
    /// </summary>
    /// <returns>True if there is already a tower on this tile, false otherwise.</returns>
    bool IsBlocked();

    /// <summary>
    /// Indicates whether or not this tile can be built on.
    /// </summary>
    /// <returns>True if the tile can be built on, false otherwise.</returns>
    bool IsBuildable();
}
