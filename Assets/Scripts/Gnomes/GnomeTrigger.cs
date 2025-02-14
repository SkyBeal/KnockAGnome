using UnityEngine;

public class GnomeTrigger : MonoBehaviour
{
    private GnomeBehavior gnomeBehavior;
    private void Start()
    {
        gnomeBehavior = GetComponent<GnomeBehavior>();
        print(gnomeBehavior);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            gnomeBehavior.ActivateGnome();
        }
    }
}