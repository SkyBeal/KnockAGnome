using System;
using UnityEngine;

public class ReserveGnomes : MonoBehaviour
{
    #region Singleton
    private static ReserveGnomes instance;

    public static ReserveGnomes Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(ReserveGnomes)) as ReserveGnomes;
            return instance;
        }
        set { instance = value; }
    }
    #endregion

    public int maxNumOfGnomesOnLawnMower;
    [SerializeField] private int maxNumberOfReservesToSpawn;

    [NonSerialized] public int numberOfGnomesInReserve;
    [NonSerialized] public int numberOfGnomesOnLawnMower;

    GnomeManager gnomeManager;

    private void Start()
    {
        gnomeManager = GnomeManager.Instance;
    }
    public void SpawnReserveGnomes(Transform spawnLocation)
    {
        int numGnomesToSpawn = Math.Min(numberOfGnomesInReserve, maxNumberOfReservesToSpawn);

        for (int i = 0; i < numGnomesToSpawn; i++)
        {
            gnomeManager.SpawnGnome(GnomeBehavior.GnomeType.ChasePlayer, null, spawnLocation);
        }
        numberOfGnomesInReserve -= numGnomesToSpawn;

        GnomeBehavior.updateGnomesRunningAway?.Invoke();
    }

    public void AddGnomeToReserve()
    {
        numberOfGnomesInReserve++;
    }
}