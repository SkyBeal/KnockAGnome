using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour

{
    [SerializeField] private Rigidbody hand;
    [SerializeField] private ConfigurableJoint joint;
    [SerializeField] private float velocityToKill;
    [SerializeField] Transform pointToTrack;

    private Vector3 previousPos;
    private float velocityMagnitude;


    private void Awake()
    {
        PublicEventManager.RotateChair += SetVelocityThreshold;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttachToHand());
    }

    // Update is called once per frame
    void Update()
    {
        velocityMagnitude = (pointToTrack.position - previousPos).magnitude / Time.deltaTime;
        previousPos = pointToTrack.position;
        Debug.Log(velocityMagnitude);
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.GetComponent<GnomeBehavior>() != null)
        {
            Debug.Log("shovel hit at " + velocityMagnitude + " speed");
            if (velocityMagnitude >= velocityToKill)
            {
                collision.gameObject.GetComponent<GnomeBehavior>().Die();
            }
        }

    }

    public void SetVelocityThreshold(RotoManager.RotoInstructions roto)
    {
        velocityToKill = roto.shovelVelocity;
    }

    private void OnDisable()
    {
        PublicEventManager.RotateChair -= SetVelocityThreshold;
    }

    public IEnumerator AttachToHand()
    {
        yield return null;
        joint.connectedBody = hand;
    }
}
