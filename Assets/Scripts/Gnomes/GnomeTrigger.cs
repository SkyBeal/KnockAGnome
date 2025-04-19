using UnityEngine;

public class GnomeTrigger : MonoBehaviour
{
    private GnomeAnimationManager animationManager;

    private void Start()
    {
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