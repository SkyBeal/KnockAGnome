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
    public Transform playerPrefab;
    [Tooltip("Distance from the player the gnome has to be to despawn while running away")]
    public float distanceFromPlayerToDespawn;

    [SerializeField] Transform gnomeAttachPosition1, gnomeAttachPosition2, gnomeAttachPosition3, gnomeAttachPosition4;
    [NonSerialized] public Dictionary<Transform, bool> gnomeAttachPosition;


    [NonSerialized] public List<GnomeBehavior> spawnedGnomes;

    //Creates Object Pooling
    [NonSerialized] public Queue<GnomeBehavior> gnomeObjectPools;

    public void Start()
    {
        //Creates gnome pool
        gnomeObjectPools = new();
        
        //Creates list of gnomes
        spawnedGnomes = new();

        //Sets attach position and if the gnome is attached to it
        gnomeAttachPosition = new()
        {
            { gnomeAttachPosition1, false },
            { gnomeAttachPosition2, false },
            { gnomeAttachPosition3, false },
            { gnomeAttachPosition4, false }
        };
    }

    /// <summary>
    /// Takes a gnome out of the pool and adds it to the game.
    /// If the pool is empty, create a new gnome.
    /// </summary>
    /// <param name="animatorController"></param>
    /// <param name="spawnLocation"></param>
    /// <returns></returns>
    public GnomeBehavior SpawnGnome(GnomeBehavior.GnomeType gnomeType, AnimatorController animatorController, Vector3 spawnLocation)
    {
        GnomeBehavior spawnedGnome = null;

        //If pool is not empty, take a gnome out of the pool
        if (gnomeObjectPools.Count > 0)
        {
            //Dequeue enemy and initialize it
            spawnedGnome = gnomeObjectPools.Dequeue(); //Removes gnome from pool and adds it in
            //spawnedGnome.Init(); - TODO: When object pooling is active, uncomment this and set GnomeBehavior's Start method to Init()

            //Sets Attributes
            spawnedGnome.gnomeAction = gnomeType; //Sets the gnome type
            spawnedGnome.GetComponent<Animator>().runtimeAnimatorController = animatorController; //Sets animation - Probably needs to be removed
            spawnedGnome.transform.position = spawnLocation; //Sets gnome position
            spawnedGnome.gameObject.SetActive(true); //Activates the gnome
            spawnedGnome.isAttacking = false;
            spawnedGnome.isMoving = true;
        }
        //If pool is empty, create a new gnome
        else
        {
            //Instantiate new insatnce of enemy and initialize
            GameObject newEnemy = Instantiate(gnome, spawnLocation, Quaternion.identity);
            newEnemy.transform.parent = gnomesFolder;
            spawnedGnome = newEnemy.GetComponent<GnomeBehavior>();

            //Sets attributes
            spawnedGnome.gnomeAction = gnomeType;
            spawnedGnome.target = playerPrefab;
            newEnemy.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            newEnemy.transform.position = spawnLocation;
            //spawnedGnome.Init(); - TODO: When object pooling is active, uncomment this and set GnomeBehavior's Start method to Init()
        }

        //If list does not contain this gnome, add it
        if (!spawnedGnomes.Contains(spawnedGnome)) spawnedGnomes.Add(spawnedGnome);

        //Tells every gnome to update if they should be running away from the cart or not
        return spawnedGnome;
    }

    /// <summary>
    /// Removes a gnome and returns it to the pool
    /// </summary>
    /// <param name="gnomeToRemove">The gnome to remove</param>
    public void RemoveEnemy(GnomeBehavior gnomeToRemove)
    {
        //Makes enemy idle and inactive - extremely efficient
        gnomeObjectPools.Enqueue(gnomeToRemove); //Gnome is added back to the pool
        gnomeToRemove.gameObject.SetActive(false); //Gnome is disabled
        gnomeToRemove.isAlive = false; //Gnome is dead

        spawnedGnomes.Remove(gnomeToRemove); //Removes gnome from the list of active gnomes
    }
}