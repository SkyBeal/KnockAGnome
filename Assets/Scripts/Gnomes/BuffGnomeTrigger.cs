using UnityEngine;

public class BuffGnomeTrigger : MonoBehaviour
{
    [SerializeField] private BuffGnomeParticleManager buffGnomeScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Mowing>() != null)
        {
            buffGnomeScript.EnterBuffGnome();
            Debug.Log("Buff Gnome Activated");
        }
    }
}