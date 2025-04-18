using UnityEngine;

public class GnomeTrigger : MonoBehaviour
{
    private GnomeBehavior gnomeBehavior;
    private GnomeAnimationManager animationManager;
    private void Start()
    {
        gnomeBehavior = GetComponentInParent<GnomeBehavior>();
        animationManager = GetComponent<GnomeAnimationManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Mowing>() != null)
        {
            animationManager.Jumpscare();
            Debug.Log("Gnome has been triggered");
        }
    }
}