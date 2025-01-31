using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shatter))]

public class GnomeBehavior : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    private Rigidbody rb;
    private Shatter shatter;

    private bool isMoving;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        shatter = GetComponent<Shatter>();
    }

    //Start is called before the first frame update
    void Start()
    {
        //target will eventually be set to the lawnmower here once it exists.
        isMoving = true;
        StartCoroutine("MoveTowardTarget");

        //Invoke("Die", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void Die()
    {
        Debug.Log(this.name + " has died.");
        isMoving = false;
        shatter.BreakObject();
    }

    IEnumerator MoveTowardTarget()
    {
        while (isMoving)
        {
            Vector3 direction = (target.position - body.position).normalized;
            
            rb.velocity = new Vector3 (direction.x, rb.velocity.y, direction.z) * moveSpeed;

            yield return new WaitForFixedUpdate();
        }
        
    }
}
