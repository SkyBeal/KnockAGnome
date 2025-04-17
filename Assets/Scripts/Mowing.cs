using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Sky Beal
/// Date: 4/1/25
/// Description: Allows the Lawnmower to cut grass via a terrain copying system. Reapplies grass on restart.
/// 
/// References: Big Game Design Journey's blog post on live terrain updating
/// </summary>

public class Mowing : MonoBehaviour
{
    [Tooltip("Radius of the box that cuts the grass.")]
    public float GrassCutterRadius;

    private Terrain terrain;
    private int[,] map = null;

    /// <summary>
    /// obtains terrain and terrain data
    /// </summary>
    void Start()
    {
        terrain = GameObject.FindObjectOfType<Terrain>();
        terrain.terrainData = Instantiate(terrain.terrainData);
    }

    /// <summary>
    /// cuts grass where the mower is
    /// </summary>
    void Update()
    {
        CutTheGrass(terrain, gameObject.transform.position, GrassCutterRadius);
    }

    /// <summary>
    /// Draws a cube area of where the grass will be cut
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(GrassCutterRadius * 2, GrassCutterRadius * 2, GrassCutterRadius * 2));
    }

    /// <summary>
    /// Cuts the grass by setting terrain detail to 0,0,0
    /// t: Terrain object in the scene
    /// position: cart's current position in the scene
    /// radius: **RADIUS** of area where grass will be cut
    /// </summary>
    public void CutTheGrass(Terrain t, Vector3 position, float radius)
    {

        //gets the size of the terrain to compare to the terrain data size
        int TerrainDetailMapSize = terrain.terrainData.detailResolution;
        if (terrain.terrainData.size.x != terrain.terrainData.size.z)
        {
            Debug.Log("X and Y Size of terrain have to be the same (RemoveGrass.CS Line 43)");
            return;
        }

        //the rest is a lot of math to calculate where the grass should be cut
        float PrPxSize = TerrainDetailMapSize / terrain.terrainData.size.x;

        Vector3 TexturePoint3D = position - t.transform.position;
        TexturePoint3D = TexturePoint3D * PrPxSize;

        float[] xymaxmin = new float[4];
        xymaxmin[0] = TexturePoint3D.z + radius;
        xymaxmin[1] = TexturePoint3D.z - radius;
        xymaxmin[2] = TexturePoint3D.x + radius;
        xymaxmin[3] = TexturePoint3D.x - radius;

        map = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 0);

        for (int y = 0; y < terrain.terrainData.detailHeight; y++)
        {
            for (int x = 0; x < terrain.terrainData.detailWidth; x++)
            {

                if (xymaxmin[0] > x && xymaxmin[1] < x && xymaxmin[2] > y && xymaxmin[3] < y)
                    map[x, y] = 0;
            }
        }

        //cuts grass!
        terrain.terrainData.SetDetailLayer(0, 0, 0, map);
    }
}
