using UnityEngine;

public class LawnmowerHitbox : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<GnomeBehavior>() != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Squash, transform.position);
            collider.gameObject.GetComponent<GnomeBehavior>().playOno = false;
            collider.gameObject.GetComponent<GnomeBehavior>().Die();
        }
    }
}