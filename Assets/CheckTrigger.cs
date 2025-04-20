using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    [SerializeField] private GameObject EndScreen;
    private SplineController SC;

    private void Start()
    {
        SC = FindObjectOfType<SplineController>();
    }

    private void LateUpdate()
    {
        var results = Physics.OverlapBox(transform.position, transform.localScale);
        foreach (var result in results)
        {
            Check(result);
        }
    }

    private void Check(Collider other)
    {
        if (other.gameObject.GetComponent<LawnmowerHitbox>() != null)
        {
            if (EndScreen.activeSelf == false)
            {
                SC.CallEnding();
                //EndScreen.SetActive(true);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
