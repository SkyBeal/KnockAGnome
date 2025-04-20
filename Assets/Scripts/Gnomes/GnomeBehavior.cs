/******************************************************************************
 * Author: Campbell Dugal
 * Last Modified: 2/4/25
 * Description: A basic behavior script for the gnome enemies that run up to
 *              and attack the lawnmower. 
 * Collaborators: Ryan Herwig
 *****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Splines;
using NaughtyAttributes;

[RequireComponent(typeof(Animator))]
public class GnomeBehavior : MonoBehaviour
{
    [Tooltip("Should be the child with the skinned mesh renderer on it.")]
    public GameObject GnomeModel;
    #region Variables

    [Tooltip("Check this box if this is the first gnome in the game!")]
    [SerializeField] bool firstGnome;

    [SerializeField, Tooltip("The transform that the gnome should move towards.")]
    private Transform target;

    [SerializeField, Tooltip("How fast the gnome should move.")]
    private float moveSpeed;

    [SerializeField, Tooltip("The time in seconds between a gnome's attacks.")]
    private float attackInterval;

    [SerializeField, Tooltip("What the gnome will do when activated")] private GnomeAction gnomeAction;

    [SerializeField, Tooltip("The Particles for the gnome exploding")] private ParticleSystem explosionParticles;

    //Pick random Particle System inside folder to play
    [SerializeField, Tooltip("The folder containing all of the onomatopeias")] private Transform onomatopeiasFolder;

    [SerializeField, Tooltip("")] private BalloonScript balloon;


    private Rigidbody rb;
    private LawnmowerPointsSystem pointsSystem;

    private bool isMoving;
    private bool isAttacking;
    private bool isDead;

    private bool isChasingPlayer;

    private int numOfOnomatopeias;

    private float speedBeforePhysicsUpdate;

    private Animator animator;
    private EventInstance attachSFX;
    [SerializeField] private GameObject shatterObject;
    [SerializeField] private float speedToBreak;

    public bool playOno = true;
    #endregion

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        pointsSystem = FindObjectOfType<LawnmowerPointsSystem>();

    }

    //Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        isChasingPlayer = false;
        numOfOnomatopeias = onomatopeiasFolder.childCount;
        animator = GetComponent<Animator>();

        //Here for testing until theres a reliable way to kill the gnome in the scene.
        //Die(this.GetComponent<Rigidbody>().velocity);
    }

        // Update is called once per frame
    void Update()
    {
        attachSFX.set3DAttributes(RuntimeUtils.To3DAttributes(GetComponent<Transform>(), rb));
    }

    private void FixedUpdate()
    {
        speedBeforePhysicsUpdate = rb.velocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(speedBeforePhysicsUpdate);        
        if(speedBeforePhysicsUpdate > speedToBreak)
        {
            Die();
        }

        // If gnome should be attacking the player, and has made contact with the player
        // Grapple the player and start dealing damage
        if (isAttacking && collision.gameObject.GetComponent<LawnmowerPointsSystem>() != null)
        {
            AttachToCart(collision.transform);
            if (collision.gameObject.GetComponent<LawnmowerPointsSystem>() != null)
            {
                AttachToCart(collision.transform);
            }
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
    [Button]
    public void Die()
    {
        Debug.Log(gameObject.name + " was killed");
        if (!isDead)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Shatter, transform.position);
            if (playOno)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.Onomatopoeia, transform.position);
            }
            Instantiate(shatterObject, transform.position, Quaternion.identity); // spawns particle gameobject, which gives the illusion of gnome shattering
            isDead = true;
            isMoving = false;
            isAttacking = false;
            transform.parent = null;
            if(pointsSystem != null)
                pointsSystem.GainPoints();

            GnomeModel.SetActive(false);
            GetComponent<CapsuleCollider>().enabled = false;
            
            //temp fix for gnome mesh destruction (replaced with above)
            //MeshRenderer mr = GnomeModel.GetComponent<MeshRenderer>();
            //mr.enabled = false;
            //mr2.enabled = true;

            SkinnedMeshRenderer mr = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

            if (mr != null)
            {
                mr.enabled = false;
            }


            explosionParticles.Play(); // Plays explosion particle system

            //Gets a random int
            int randomInt = Random.Range(0, numOfOnomatopeias);

            //Plays random onomatopeia
            onomatopeiasFolder.GetChild(randomInt).GetComponent<ParticleSystem>().Play();

            if(firstGnome)
            {
                GameObject.Find("PlayerPrefab").GetComponent<SplineAnimate>().Play();

                GetComponentInChildren<GnomeAnimationManager>().BalloonSmash();

                MusicManager.instance.switchMusic(1);
            }

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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Attack, transform.position);
            if (pointsSystem != null)
                pointsSystem.LosePoints();
            
            yield return new WaitForSeconds(attackInterval);
        }
    }

    /// <summary>
    /// Method is called when the gnome should start doing the action they are 
    /// assigned, whether that be chasing the player or wrecking the garden
    /// </summary>
    public void ActivateGnome()
    {
        //Gnome chases the player
        if (gnomeAction == GnomeAction.ChasePlayer)
        {
            isMoving = true;
            isChasingPlayer = true;
            StartCoroutine(MoveTowardTarget());
        }
        //Gnome wrecks the garden
        else
        {
            animator.SetTrigger("Activate");
        }
    }

    public enum GnomeAction
    {
        ChasePlayer,
        WreckGarden
    }
}
