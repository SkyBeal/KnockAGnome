using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour

{
    [SerializeField] private Rigidbody hand;
    [SerializeField] private ConfigurableJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttachToHand());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.GetComponent<GnomeBehavior>() != null)
        {

            collision.gameObject.GetComponent<GnomeBehavior>().Die(this.GetComponent<Rigidbody>().velocity);

        }

    }

    public IEnumerator AttachToHand()
    {
        yield return null;
        joint.connectedBody = hand;
    }
}
