using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mowing : MonoBehaviour
{
    private CutGrass CG;
    // Start is called before the first frame update
    void Start()
    {
        CG = FindAnyObjectByType<CutGrass>();
    }

    // Update is called once per frame
    void Update()
    {
        CG.CutTheGrass(CG.te, gameObject.transform.position, CG.GrassCutterRadius);
        OnDrawGizmos();
    }

    private void OnDrawGizmos()
    {
        {
            CG = FindAnyObjectByType<CutGrass>();
        }
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(CG.GrassCutterRadius, CG.GrassCutterRadius, CG.GrassCutterRadius));
    }
}
