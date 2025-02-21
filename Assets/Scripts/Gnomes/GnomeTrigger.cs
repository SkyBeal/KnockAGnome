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
        if (other.transform.CompareTag("Player"))
        {
            gnomeBehavior.ActivateGnome();
        }
    }
}