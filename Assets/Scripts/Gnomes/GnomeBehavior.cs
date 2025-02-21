/******************************************************************************
 * Author: Campbell Dugal
 * Last Modified: 2/4/25
 * Description: A basic behavior script for the gnome enemies that run up to
 *              and attack the lawnmower. 
 *
 *****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Shatter))]

public class GnomeBehavior : MonoBehaviour
{
    public GameObject GnomeModel;
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
    private bool isDead;

    private EventInstance attachSFX;
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
        attachSFX = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Shatter);
        isMoving = true;
        if (target != null)
        {
            StartCoroutine(MoveTowardTarget());
        }

        //Here for testing until theres a reliable way to kill the gnome in the scene.
        //Invoke("Die", 1f);
    }

        // Update is called once per frame
        void Update()
    {
        attachSFX.set3DAttributes(RuntimeUtils.To3DAttributes(GetComponent<Transform>(), rb));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<LawnmowerPointsSystem>() != null)
        {
            AttachToCart(collision.transform);
            PLAYBACK_STATE playbackState;
            attachSFX.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                attachSFX.start();
            }
            else
            {
                attachSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

        }
    }

    /// <summary>
    /// Ends gnome behavior and calls for the gnome to be shatter, passing the velocity of the killing attack.
    /// </summary>
    /// <param name="killingBlowVelocity"></param>
    public void Die(Vector3 killingBlowVelocity)
    {
        if (!isDead)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Shatter, transform.position);
            isDead = true;
            isMoving = false;
            isAttacking = false;
            transform.parent = null;
            if(pointsSystem != null)
                pointsSystem.GainPoints();
            MeshRenderer mr = GnomeModel.GetComponent<MeshRenderer>();
            mr.enabled = false;
            shatter.BreakObject(killingBlowVelocity);
        }
    }

    /// <summary>
    /// Once the gnome reaches the lawnmower it attaches itself to it and continues attacking it until it is killed.
    /// </summary>
    /// <param name="cart"></param>
    void AttachToCart(Transform cart)
    {
        isMoving = false;
        isAttacking = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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
            
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Shatter, transform.position);
            if (pointsSystem != null)
                pointsSystem.LosePoints();
            
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
