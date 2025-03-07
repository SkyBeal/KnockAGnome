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
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.GetComponent<GnomeBehavior>() != null)
        {
            if (velocityMagnitude >= velocityToKill)
            {
                collision.gameObject.GetComponent<GnomeBehavior>().Die(this.GetComponent<Rigidbody>().velocity);
            }
        }

    }

    public IEnumerator AttachToHand()
    {
        yield return null;
        joint.connectedBody = hand;
    }
}
