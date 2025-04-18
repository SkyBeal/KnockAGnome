using UnityEngine;

public class GnomeTrigger : MonoBehaviour
{
    private GnomeBehavior gnomeBehavior;
    private void Start()
    {
        gnomeBehavior = GetComponentInParent<GnomeBehavior>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<SplineController>() != null)
        {
            //gnomeBehavior.ActivateGnome();
            Debug.Log("Gnome has been triggered");
        }
    }
}