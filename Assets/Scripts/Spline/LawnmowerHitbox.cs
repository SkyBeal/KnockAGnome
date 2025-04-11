using UnityEngine;

public class LawnmowerHitbox : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<GnomeBehavior>() != null)
        {
            collider.gameObject.GetComponent<GnomeBehavior>().Die();
        }
    }
}