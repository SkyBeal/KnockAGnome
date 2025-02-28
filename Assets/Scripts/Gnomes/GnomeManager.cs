using System.Collections.Generic;
using System;
using UnityEngine;

public class GnomeManager : MonoBehaviour
{
    public List<GnomeBehavior> spawnedGnomes;
    public List<Transform> spawnedGnomesTransform;

    //Connects enemy IDs to thier gameobject
    public Dictionary<int, GameObject> gnomePrefab;

    //Creates Object Pooling
    public Dictionary<int, Queue<GnomeBehavior>> gnomeObjectPools;

    public Dictionary<Transform, GnomeBehavior> gnomeTransformDictionary;

    public Transform[] SpawnPoints;

    public bool isInitialized = false;
    [SerializeField] private Transform gnomesFolder;
    public void Start()
    {
        gnomePrefab = new();
        gnomeObjectPools = new();
        gnomeTransformDictionary = new();
        spawnedGnomes = new();
        spawnedGnomesTransform = new();

        //Loads Enemy Data on runtime
        GnomeData[] gnomes = Resources.LoadAll<GnomeData>("Enemies");

        //Loads enemy data into Dictionaries
        foreach (GnomeData gnome in gnomes)
        {
            gnomePrefab.Add(gnome.gnomeID, gnome.gnomePrefab);
            gnomeObjectPools.Add(gnome.gnomeID, new Queue<GnomeBehavior>());
        }

        isInitialized = true;
    }

    public GnomeBehavior SpawnGnome(Tuple<int, int> tuple)
    {
        GnomeBehavior spawnedGnome = null;

        int gnomeID = tuple.Item1;
        int spawnPointID = tuple.Item2;
        if (gnomePrefab.ContainsKey(gnomeID))
        {
            Queue<GnomeBehavior> ReferencedQueue = gnomeObjectPools[gnomeID];

            if (ReferencedQueue.Count > 0)
            {
                //Dequeue enemy and initialize it
                spawnedGnome = ReferencedQueue.Dequeue();
                spawnedGnome.Init();
                spawnedGnome.transform.position = SpawnPoints[spawnPointID].position;
                spawnedGnome.gameObject.SetActive(true);
            }
            else
            {
                //Instantiate new insatnce of enemy and initialize
                GameObject newEnemy = Instantiate(gnomePrefab[gnomeID], SpawnPoints[spawnPointID].position, Quaternion.identity);
                newEnemy.transform.parent = gnomesFolder;
                spawnedGnome = newEnemy.GetComponent<GnomeBehavior>();
                spawnedGnome.Init();
            }
        }
        else
        {
            print("ERROR: ENEMY WITH ID OF \"" + gnomeID + "\" DOES NOT EXIST!");
            return null;
        }

        if (!spawnedGnomes.Contains(spawnedGnome)) spawnedGnomes.Add(spawnedGnome);
        if (!spawnedGnomesTransform.Contains(spawnedGnome.transform)) spawnedGnomesTransform.Add(spawnedGnome.transform);
        if (!gnomeTransformDictionary.ContainsKey(spawnedGnome.transform)) gnomeTransformDictionary.Add(spawnedGnome.transform, spawnedGnome);
        spawnedGnome.ID = gnomeID;
        return spawnedGnome;
    }

    public void RemoveEnemy(GnomeBehavior gnomeToRemove)
    {
        gnomeObjectPools[gnomeToRemove.ID].Enqueue(gnomeToRemove); //Makes enemy idle and inactive - extremely efficient
        gnomeToRemove.gameObject.SetActive(false);
        gnomeToRemove.isAlive = false;

        gnomeTransformDictionary.Remove(gnomeToRemove.transform);
        spawnedGnomesTransform.Remove(gnomeToRemove.transform);
        spawnedGnomes.Remove(gnomeToRemove);
    }
}