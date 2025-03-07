using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GnomeManager : MonoBehaviour
{
    #region Singleton
    private static GnomeManager instance;

    public static GnomeManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GnomeManager)) as GnomeManager;
            return instance;
        }
        set { instance = value; }
    }
    #endregion
    [SerializeField] private GameObject gnome;
    [SerializeField] private Transform gnomesFolder;
    [SerializeField] private Transform playerPrefab;
    [Tooltip("Distance from the player the gnome has to be to despawn while running away")]
    public float distanceFromPlayerToDespawn;

    [SerializeField] Transform gnomeAttachPosition1, gnomeAttachPosition2, gnomeAttachPosition3, gnomeAttachPosition4, gnomeAttachPosition5, gnomeAttachPosition6;
    [NonSerialized] public Dictionary<Transform, bool> gnomeAttachPosition;


    [NonSerialized] public List<GnomeBehavior> spawnedGnomes;

    //Creates Object Pooling
    [NonSerialized] public Queue<GnomeBehavior> gnomeObjectPools;

    public void Start()
    {
        gnomeObjectPools = new();
        spawnedGnomes = new();

        gnomeAttachPosition = new()
        {
            { gnomeAttachPosition1, false },
            { gnomeAttachPosition2, false },
            { gnomeAttachPosition3, false },
            { gnomeAttachPosition4, false },
            { gnomeAttachPosition5, false },
            { gnomeAttachPosition6, false }
        };
    }

    /// <summary>
    /// Takes a gnome out of the pool and adds it to the game.
    /// If the pool is empty, create a new gnome.
    /// </summary>
    /// <param name="animatorController"></param>
    /// <param name="spawnLocation"></param>
    /// <returns></returns>
    public GnomeBehavior SpawnGnome(GnomeBehavior.GnomeType gnomeType, AnimatorController animatorController, Transform spawnLocation)
    {
        GnomeBehavior spawnedGnome = null;

        //If pool is not empty, take a gnome out of the pool
        if (gnomeObjectPools.Count > 0)
        {
            //Dequeue enemy and initialize it
            spawnedGnome = gnomeObjectPools.Dequeue();
            //spawnedGnome.Init();

            //Sets Attributes
            spawnedGnome.gnomeAction = gnomeType;
            spawnedGnome.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            spawnedGnome.transform.position = spawnLocation.position;
            spawnedGnome.gameObject.SetActive(true);
        }
        //If pool is empty, create a new gnome
        else
        {
            //Instantiate new insatnce of enemy and initialize
            GameObject newEnemy = Instantiate(gnome, spawnLocation.position, Quaternion.identity);
            newEnemy.transform.parent = gnomesFolder;
            spawnedGnome = newEnemy.GetComponent<GnomeBehavior>();

            //Sets attributes
            spawnedGnome.gnomeAction = gnomeType;
            spawnedGnome.target = playerPrefab;
            newEnemy.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            newEnemy.transform.position = spawnLocation.position;
            //spawnedGnome.Init();
        }

        //If list does not contain this gnome, add it
        if (!spawnedGnomes.Contains(spawnedGnome)) spawnedGnomes.Add(spawnedGnome);

        return spawnedGnome;
    }

    /// <summary>
    /// Removes a gnome and returns it to the pool
    /// </summary>
    /// <param name="gnomeToRemove">The gnome to remove</param>
    public void RemoveEnemy(GnomeBehavior gnomeToRemove)
    {
        gnomeObjectPools.Enqueue(gnomeToRemove); //Makes enemy idle and inactive - extremely efficient
        gnomeToRemove.gameObject.SetActive(false);
        gnomeToRemove.isAlive = false;

        spawnedGnomes.Remove(gnomeToRemove);
    }
}