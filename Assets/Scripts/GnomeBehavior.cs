using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    private Rigidbody rb;

    private bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Start is called before the first frame update
    void Start()
    {
        //target will eventually be set to the lawnmower here once it exists.
        isMoving = true;
        StartCoroutine("MoveTowardTarget");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        Debug.Log(this.name + " has died.");
        isMoving = false;
    }

    IEnumerator MoveTowardTarget()
    {
        while (isMoving)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            
            
            rb.velocity = new Vector3 (direction.x, rb.velocity.y, direction.z) * moveSpeed;

            yield return new WaitForFixedUpdate();
        }
        
    }
}
