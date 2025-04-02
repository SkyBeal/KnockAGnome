using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
/// <summary>
/// BIG HELP FROM THE BIG GAME DESIGN JOURNEY'S GUIDE ON CUTTING GRASS
/// </summary>
public class CutGrass : MonoBehaviour
{
    [Tooltip("Radius of the box that cuts the grass.")]
    public float GrassCutterRadius;
    //public LayerMask layerMask;


    protected int[,] BackupMap;
    [HideInInspector] public Terrain te;

    private TerrainData OGTerrainData;
    private TerrainData modifiedTerrainData;

    // Use this for initialization
    void Start()
    {
        te = GameObject.FindObjectOfType<Terrain>();
        CreateBackup();
    }

    ///
    /// Cuts the grass. BE CAREFULL: Grass may not respawn after game reset!!!!!! 
    ///
    /// The affected Terrain, if only one Terrain pass null
    /// The world position you want to cut the grass
    /// the radius of the square
    public void CutTheGrass(Terrain t, Vector3 position, float radius)
    {
        if (t == null)
        {
            t = GameObject.FindObjectOfType<Terrain>();
        }

        int TerrainDetailMapSize = t.terrainData.detailResolution;
        if (t.terrainData.size.x != t.terrainData.size.z)
        {
            Debug.Log("X and Y Size of terrain have to be the same (RemoveGrass.CS Line 43)");
            return;
        }

        float PrPxSize = TerrainDetailMapSize / t.terrainData.size.x;

        Vector3 TexturePoint3D = position - t.transform.position;
        TexturePoint3D = TexturePoint3D * PrPxSize;

        Debug.Log(TexturePoint3D);

        float[] xymaxmin = new float[4];
        xymaxmin[0] = TexturePoint3D.z + radius;
        xymaxmin[1] = TexturePoint3D.z - radius;
        xymaxmin[2] = TexturePoint3D.x + radius;
        xymaxmin[3] = TexturePoint3D.x - radius;


        int[,] map = t.terrainData.GetDetailLayer(0, 0, t.terrainData.detailWidth, t.terrainData.detailHeight, 0);

        for (int y = 0; y < t.terrainData.detailHeight; y++)
        {
            for (int x = 0; x < t.terrainData.detailWidth; x++)
            {

                if (xymaxmin[0] > x && xymaxmin[1] < x && xymaxmin[2] > y && xymaxmin[3] < y)
                    map[x, y] = 0;
            }
        }
        t.terrainData.SetDetailLayer(0, 0, 0, map);
    }

    public void PlantGrass(Terrain t, Vector3 position, float radius)
    {
        if (t == null)
        {

            t = GameObject.FindObjectOfType<Terrain>();
        }

        int TerrainDetailMapSize = t.terrainData.detailResolution;

        if (t.terrainData.size.x != t.terrainData.size.z)
        {
            Debug.Log("X and Y Size of terrain have to be the same (RemoveGrass.CS Line 43)");
            return;
        }

        float PrPxSize = TerrainDetailMapSize / t.terrainData.size.x;

        Vector3 TexturePoint3D = position - t.transform.position;
        TexturePoint3D = TexturePoint3D * PrPxSize;

        Debug.Log(TexturePoint3D);

        float[] xymaxmin = new float[4];
        xymaxmin[0] = TexturePoint3D.z + radius;
        xymaxmin[1] = TexturePoint3D.z - radius;
        xymaxmin[2] = TexturePoint3D.x + radius;
        xymaxmin[3] = TexturePoint3D.x - radius;


        int[,] map = t.terrainData.GetDetailLayer(0, 0, t.terrainData.detailWidth, t.terrainData.detailHeight, 0);

        for (int y = 0; y < t.terrainData.detailHeight; y++)
        {
            if (xymaxmin[2] > y && xymaxmin[3] < y)
            {
                for (int x = 0; x < t.terrainData.detailWidth; x++)
                {

                    if (xymaxmin[0] > x && xymaxmin[1] < x)
                        map[x, y] = 1;
                }
            }
        }
        t.terrainData.SetDetailLayer(0, 0, 0, map);
    }

    void CreateBackup()
    {
        Debug.Log("DetailBackup Done");
        //BackupMap = te.terrainData.GetDetailLayer(0, 0, te.terrainData.detailWidth, te.terrainData.detailHeight, 0);
        OGTerrainData = te.terrainData;
        var copy = Instantiate(OGTerrainData);
        modifiedTerrainData = copy;
    }

    private void OnDestroy()
    {
        if (modifiedTerrainData != null)
        {
            Destroy(modifiedTerrainData);
        }
    }

    private void OnDrawGizmos()
    {
    }
}