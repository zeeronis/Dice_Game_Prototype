using UnityEngine;

public abstract class SpawnPointsBase: MonoBehaviour
{
    public abstract Vector3[] GetSpawnPoints(Vector3 spawnCenter, int count, float unitRadius = 0.5f);
}
