/******************************************************************************
 * Author: Campbell Dugal
 * Last Modified: 2/4/25
 * Description: This is a script meant for designer use to control the behavior
 *              of the MeshDemolisher package. The demolish behavior functions 
 *              by creating a Voronoi pattern and breaking the mesh into pieces 
 *              based on the sections of this pattern. These points, for now, 
 *              need to be manually placed as child objects of the 
 *              breakPointsParent object.
 *
 *****************************************************************************/

using Hanzzz.MeshDemolisher;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("The object that contains the mesh to be shattered.")]
    private GameObject target;

    [SerializeField, Tooltip("The parent object of the points which determine the Voronoi pattern.")]
    private Transform breakPointsParent;

    [SerializeField, Tooltip("The parent object that broken mesh pieces will be placed under.")]
    private Transform fragmentsParent;

    [SerializeField, Tooltip("The material for the inner sides of the new broken mesh pieces")]
    private Material meshInterior;

    [SerializeField, Tooltip("A value between zero and one that scales the broken mesh pieces." +
        " A value of zero will make pieces infinitely small. A value of one will not scale pieces."), Range(0f, 1f)]
    private float fragmentScale;

    [SerializeField, Tooltip("The force applied to new broken pieces; will be multiplied by the weapon's velocity")]
    private float breakForce;

    [SerializeField, Tooltip("The time, in seconds, before broken pieces will despawn")]
    private float fragmentLifetime;

    private static MeshDemolisher meshDemolisher = new MeshDemolisher();
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> breakPoints
            = Enumerable.Range(0, breakPointsParent.childCount).Select(x => breakPointsParent.GetChild(x)).ToList();

        bool res = meshDemolisher.VerifyDemolishInput(target, breakPoints);
        if (res)
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Breaks the mesh into individual pieces of a Voronoi pattern determined by the points inside of breakPoints, adds
    /// physics components to them, and organizes than under a results parent object.
    /// </summary>
    public async void BreakObject(Vector3 fragmentVelocity)
    {
        //Collects all of the points childed to the breakPoints object.
        List<Transform> breakPoints 
            = Enumerable.Range(0, breakPointsParent.childCount).Select(x => breakPointsParent.GetChild(x)).ToList();

        //Asynchonously runs the internal demolition code and sets the pieces' parent to be the result object.
        //This can take a second to complete depending on the complexity of the mesh, since the final gnome mesh is
        //pretty low poly, I don't think it'll be an issue. If it is, lower the number of break points.
        List<GameObject> results = await meshDemolisher.DemolishAsync(target, breakPoints, meshInterior);
        results.ForEach(x => x.transform.SetParent(fragmentsParent, true));

        //Add physics components to all of the new pieces.
        results.ForEach(x => x.AddComponent<MeshCollider>().convex = true);
        results.ForEach(x => x.AddComponent<Rigidbody>().AddExplosionForce(/*fragmentVelocity.magnitude **/ breakForce, 
            transform.position, 1, 0, ForceMode.Impulse));
        
        //Scales the results pieces by the fragmentScale.
        Enumerable.Range(0, fragmentsParent.childCount).Select(i => fragmentsParent.GetChild(i)).ToList().ForEach(x => 
            x.localScale = fragmentScale * Vector3.one);
        
        //Hides the original full mesh
        target.gameObject.SetActive(false);
        StartCoroutine(FragmentDespawn());
    }

    public IEnumerator FragmentDespawn()
    {
        yield return new WaitForSeconds(fragmentLifetime);
        Destroy(this.gameObject);
    }
}
