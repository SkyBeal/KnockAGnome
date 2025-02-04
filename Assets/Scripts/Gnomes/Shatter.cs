using Hanzzz.MeshDemolisher;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Transform breakPointsParent;
    [SerializeField] private Material meshInterior;
    [SerializeField] [Range(0f, 1f)] private float fragmentScale;
    [SerializeField] private Transform fragments;

    private static MeshDemolisher meshDemolisher = new MeshDemolisher();

    // Start is called before the first frame update
    void Start()
    {
        
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
        results.ForEach(x => x.transform.SetParent(fragments, true));

        //Add physics components to all of the new pieces.
        results.ForEach(x => x.AddComponent<MeshCollider>().convex = true);
        results.ForEach(x => x.AddComponent<Rigidbody>().AddExplosionForce(fragmentVelocity.magnitude, 
            transform.position, 1, 0, ForceMode.Impulse));
        
        //Scales the results pieces by the fragmentScale.
        Enumerable.Range(0, fragments.childCount).Select(i => fragments.GetChild(i)).ToList().ForEach(x => x.localScale
            = fragmentScale * Vector3.one);
        target.gameObject.SetActive(false);
    }
}
