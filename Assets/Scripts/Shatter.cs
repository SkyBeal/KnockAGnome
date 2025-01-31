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

    public async void BreakObject()
    {
        List<Transform> breakPoints 
            = Enumerable.Range(0, breakPointsParent.childCount).Select(x => breakPointsParent.GetChild(x)).ToList();
        List<GameObject> results = await meshDemolisher.DemolishAsync(target, breakPoints, meshInterior);

        results.ForEach(x => x.transform.SetParent(fragments, true));

        //Add physics components to all of the new pieces
        results.ForEach(x => x.AddComponent<MeshCollider>().convex = true);
        results.ForEach(x => x.AddComponent<Rigidbody>());

        Enumerable.Range(0, fragments.childCount).Select(i => fragments.GetChild(i)).ToList().ForEach(x => x.localScale
            = fragmentScale * Vector3.one);
        target.gameObject.SetActive(false);
    }
}
