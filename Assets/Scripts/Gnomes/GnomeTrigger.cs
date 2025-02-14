using UnityEngine;

public class GnomeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //Activate Gnome
        }
    }
}