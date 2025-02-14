/******************************************************************************
 * Author: Campbell Dugal
 * Last Modified: 2/4/25
 * Description: A basic behavior script for the gnome enemies that run up to
 *              and attack the lawnmower. 
 *
 *****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shatter))]

public class GnomeBehavior : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("The transform that the gnome should move towards.")]
    private Transform target;

    [SerializeField, Tooltip("How fast the gnome should move.")]
    private float moveSpeed;

    [SerializeField, Tooltip("The time in seconds between a gnome's attacks.")]
    private float attackInterval;

    private Rigidbody rb;
    private Shatter shatter;
    private LawnmowerPointsSystem pointsSystem;

    private bool isMoving;
    private bool isAttacking;
    #endregion

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        shatter = GetComponent<Shatter>();
        pointsSystem = FindObjectOfType<LawnmowerPointsSystem>();
    }

    //Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        StartCoroutine(MoveTowardTarget());

        //Here for testing until theres a reliable way to kill the gnome in the scene.
        //Invoke("Die", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<LawnmowerPointsSystem>() != null)
        {
            AttachToCart(collision.transform);
        }
    }

    /// <summary>
    /// Ends gnome behavior and calls for the gnome to be shatter, passing the velocity of the killing attack.
    /// </summary>
    /// <param name="killingBlowVelocity"></param>
    public void Die(Vector3 killingBlowVelocity)
    {
        Debug.Log(this.name + " has died.");
        isMoving = false;
        isAttacking = false;
        pointsSystem.GainPoints();
        shatter.BreakObject(killingBlowVelocity);
    }

    /// <summary>
    /// Once the gnome reaches the lawnmower it attaches itself to it and continues attacking it until it is killed.
    /// </summary>
    /// <param name="cart"></param>
    void AttachToCart(Transform cart)
    {
        isMoving = false;
        isAttacking = true;
        transform.SetParent(cart);
        StartCoroutine(Attack());
    }

    /// <summary>
    /// Moves the gnome in the direction of its assigned target.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveTowardTarget()
    {
        while (isMoving)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            
            rb.velocity = new Vector3 (direction.x, rb.velocity.y, direction.z) * moveSpeed;

            yield return new WaitForFixedUpdate();
        }
        
    }

    /// <summary>
    /// Attacks every set interval, losing the player points.
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        while (isAttacking)
        {
            pointsSystem.LosePoints();
            
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
