/******************************************************************************
 * Author: Ryan Herwig
 * Last Modified: 3/7/2025
 * Description: Sets gnomes into reserve. If there are too many gnomes on the cart,
 *              gnomes that should be chasing the player run away from the cart.
 *              Once the gnomes reach a certain distance away from the cart, they
 *              get removed (via object pooling) and go into reserves.
 *              When a reserve checkpoint is reached (TODO), a set amount of
 *              gnomes get spawned in an area, from the reserve pool.
 *              Those gnomes then either chase the player or run away again.
 *              
 *              This system prevents too many gnomes surrounding the player, and
 *              it also solves the problem of the gnomes not being able to
 *              catch up to the player (as they are teleported in front of the player)
 *              
 *              TODO: The area the gnomes spawn needs to be made.
 *****************************************************************************/

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReserveManager : MonoBehaviour
{
    //Singleton
    #region Singleton
    private static ReserveManager instance;

    public static ReserveManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(ReserveManager)) as ReserveManager;
            return instance;
        }
        set { instance = value; }
    }
    #endregion

    //Sets the maximum number of gnomes that can be on the mower
    //This SHOULD match the amount of attach transforms on the cart
    public int maxNumOfGnomesOnLawnMower;

    //Maximum number of gnomes that can spawn from reserves at once
    //If there are, say 100 gnomes in reserve, this prevents all 100
    //being spawned at once, and say... 3 gnomes get spawned instead
    [SerializeField] private int maxNumberOfReservesToSpawn;

    //Keeps track on how many gnomes are in the reserve
    [NonSerialized] public int numberOfGnomesInReserve;

    //Keeps track on how many gnomes are attached to the cart
    [NonSerialized] public int numberOfGnomesOnLawnMower;

    GnomeManager gnomeManager;

    private void Start()
    {
        gnomeManager = GnomeManager.Instance;
    }

    /// <summary>
    /// Spawns a certain number of gnomes from the reserve pool into the game scene via Object Pooling
    /// </summary>
    /// <param name="spawnLocation"></param>
    public void SpawnReserveGnomes(Vector3 spawnLocation, float minSpawnRange, float maxSpawnRange)
    {

        //Gets the minimum value of the amount of gnomes in reserves and the maximum number of gnomes to spawn
        int numGnomesToSpawn = Math.Min(numberOfGnomesInReserve, maxNumberOfReservesToSpawn);

        //Loops the gnome creation
        for (int i = 0; i < numGnomesToSpawn; i++)
        {
            //Randomize location
            Vector3 randomSpawnLocation;
            do {
                randomSpawnLocation = new Vector3(Random.Range(-maxSpawnRange, maxSpawnRange), 0, Random.Range(-maxSpawnRange, maxSpawnRange));
            } while (Vector3.Distance(randomSpawnLocation + spawnLocation, spawnLocation) < minSpawnRange);
            print("Old Spawn: " + spawnLocation);
            print("Random Spawn: " + randomSpawnLocation);
            spawnLocation += randomSpawnLocation;
            print("New Spawn: " + spawnLocation);

            //Spawns gnomes
            gnomeManager.SpawnGnome(GnomeBehavior.GnomeType.ChasePlayer, null, spawnLocation);
        }
        numberOfGnomesInReserve -= numGnomesToSpawn; //Subtracts from the gnome reserve counter

        //Tells every gnome to update if they should be running away from the cart or not
        GnomeBehavior.updateGnomesRunningAway?.Invoke();
    }

    //Adds to the gnome reserve counter
    public void AddGnomeToReserve()
    {
        numberOfGnomesInReserve++;
    }
}