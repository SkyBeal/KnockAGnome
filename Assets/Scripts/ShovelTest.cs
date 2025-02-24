using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelTest : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision occurs");
        if(collision.gameObject.GetComponent<GnomeBehavior>() != null)
        {
            //Debug.Log("collision with gnome occurs");
            collision.gameObject.GetComponent<GnomeBehavior>().Die(rb.velocity);
       
        }
    }
}
