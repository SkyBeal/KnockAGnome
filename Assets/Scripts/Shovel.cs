using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour

{

    // Start is called before the first frame update
    void Start()
    {
        
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
}
