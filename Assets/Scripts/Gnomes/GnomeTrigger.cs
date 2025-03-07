using UnityEngine;

public class GnomeTrigger : MonoBehaviour
{
    private GnomeBehavior gnomeBehavior;
    private void Start()
    {
        //gnomeBehavior = GetComponentInParent<GnomeBehavior>();
    }
    private void OnTriggerEnter(Collider other)
    {
        print("Boo");
        if (other.transform.GetComponent<SplineController>() != null)
        {
            print("D");
            gnomeBehavior.ActivateGnome();
        }
    }
}