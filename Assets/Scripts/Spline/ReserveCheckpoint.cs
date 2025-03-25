using UnityEngine;

public class ReserveCheckpoint : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField, Tooltip("This value MUST be less than the maximum")] float minSpawnRange = 0;
    [SerializeField, Tooltip("This value MUST be greater than the minimum")] float maxSpawnRange = 1;

    ReserveManager reserveManager;
    private void Start()
    {
        reserveManager = ReserveManager.Instance;

        if (minSpawnRange < maxSpawnRange)
            minSpawnRange = maxSpawnRange * 1.2f;
    }

    public void ActivateReserves()
    {
        reserveManager.SpawnReserveGnomes(spawnPoint.position, minSpawnRange, maxSpawnRange);
    }
}