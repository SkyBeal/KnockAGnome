/******************************************************************************
 * Author: Campbell Dugal
 * Last Modified: 2/4/25
 * Description: A basic behavior script for the gnome enemies that run up to
 *              and attack the lawnmower. 
 * Contributers: Ryan Herwig
 *****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using UnityEngine.Splines;

[RequireComponent(typeof(Animator))]
public class GnomeBehavior : MonoBehaviour
{
    [Tooltip("Should be the child with the skinned mesh renderer on it.")]
    public GameObject GnomeModel;
    #region Variables

    [Tooltip("The transform that the gnome should move towards.")]
    public Transform target;
    [NonSerialized] public int ID;

    [Tooltip("Check this box if this is the first gnome in the game!")]
    [SerializeField] bool firstGnome;

    [SerializeField, Tooltip("How fast the gnome should move.")]
    private float moveSpeed;

    [SerializeField, Tooltip("The time in seconds between a gnome's attacks.")]
    private float attackInterval;

    [Tooltip("What the gnome will do when activated")] public GnomeType gnomeAction;

    [SerializeField, Tooltip("The Particles for the gnome exploding")] private ParticleSystem explosionParticles;

    //Pick random Particle System inside folder to play
    [SerializeField, Tooltip("The folder containing all of the onomatopeias")] private Transform onomatopeiasFolder;

    Transform attachedPosition;


    private Rigidbody rb;
    private LawnmowerPointsSystem pointsSystem;

    [NonSerialized] public bool isMoving;
    [NonSerialized] public bool isAttacking;
    [NonSerialized] public bool isDead;

    [NonSerialized] public bool isRunningAway;

    private int numOfOnomatopeias;

    private GnomeAnimationManager gnomeAnim;
    private EventInstance attachSFX;

    [NonSerialized] public bool isAlive;

    public static UnityAction updateGnomesRunningAway; //If gnomes should be running away
    GnomeManager gnomeManager; //Gets gnome manager
    ReserveManager reserveManager; //Gnomes in reserve

    [SerializeField] private GameObject shatterObject;

    #endregion

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        pointsSystem = FindObjectOfType<LawnmowerPointsSystem>();
        isRunningAway = false;
        gnomeManager = GnomeManager.Instance;
        reserveManager = ReserveManager.Instance;
    }

    //Start is called before the first frame update
    public void Start()
    {
        isMoving = false;
        numOfOnomatopeias = onomatopeiasFolder.childCount;
        gnomeAnim = GetComponentInChildren<GnomeAnimationManager>();
        isRunningAway = false;

        //Here for testing until theres a reliable way to kill the gnome in the scene.
        //Die(this.GetComponent<Rigidbody>().velocity);
    }

    public void OnEnable()
    {
        updateGnomesRunningAway += UpdateGnomesRunningAway;
    }

    public void OnDisable()
    {
        updateGnomesRunningAway -= UpdateGnomesRunningAway;
    }

    public void UpdateGnomesRunningAway()
    {
        if (reserveManager.numberOfGnomesOnLawnMower < reserveManager.maxNumOfGnomesOnLawnMower)
        {
            isRunningAway = false;
        }
        else
            isRunningAway = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        attachSFX.set3DAttributes(RuntimeUtils.To3DAttributes(GetComponent<Transform>(), rb));
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If gnome should be attacking the player, and has made contact with the player
        // Grapple the player and start dealing damage
        if (!isAttacking && collision.gameObject.transform == target)
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
    public void Die()
    {

        //Do not kill object. instead call gnome manager's RemoveEnemy()
        Debug.Log("Die is called");

        if (!isDead)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Shatter, transform.position);

            //Reduces number of gnomes on the lawn mower
            if (isAttacking)
                reserveManager.numberOfGnomesOnLawnMower--;

            //Gnome is no longer occupying position
            if (attachedPosition != null)
                gnomeManager.gnomeAttachPosition[attachedPosition] = false;
            


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



            shatter.BreakObject(killingBlowVelocity);
            explosionParticles.Play(); // Plays explosion particle system

            //Gets a random int
            int randomInt = Random.Range(0, numOfOnomatopeias);

            //Plays random onomatopeia
            onomatopeiasFolder.GetChild(randomInt).GetComponent<ParticleSystem>().Play();

            if(firstGnome)
            {

                GameObject.Find("PlayerPrefab").GetComponent<SplineAnimate>().Play();

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
        //If gnome is not running away, attach to the cart
        if (!isRunningAway)
        {
            isMoving = false;
            isAttacking = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //Iterates over dictionary
            foreach(KeyValuePair<Transform, bool> gnomePos in gnomeManager.gnomeAttachPosition)
            {
                //If a position is open, take it
                //Gives gnomes exact positions to go to when they attach to the cart
                //Takes first available spot
                if (!gnomePos.Value)
                {
                    attachedPosition = gnomePos.Key; //Sets the gnome attached position for when the gnome gets knocked off
                    gnomeManager.gnomeAttachPosition[gnomePos.Key] = true; //Sets the position as taken
                    transform.position = gnomePos.Key.transform.position; //Sets gnome position
                    break;
                }
            }

            transform.SetParent(cart);
            StartCoroutine(Attack());
            reserveManager.numberOfGnomesOnLawnMower++; //Adds to the gnome reserves counter
            updateGnomesRunningAway?.Invoke();
        }
    }

    /// <summary>
    /// Moves the gnome in the direction of its assigned target.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveTowardTarget()
    {
        while (isMoving)
        {
            print(target);
            Vector3 direction = (target.position - transform.position).normalized;
            
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            
            //If the gnome reaches a certain distance away from the player, add the gnome to reserves
            if (Vector3.Distance(target.position, transform.position) > gnomeManager.distanceFromPlayerToDespawn)
            {
                gnomeManager.RemoveEnemy(this);
                reserveManager.AddGnomeToReserve();
            }

            //If gnome is running away, it goes the opposite direction of the cart
            if (isRunningAway)
            {
                rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z) * -moveSpeed;
            }
            //Chase the player
            else
            {
                rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z) * moveSpeed;
                updateGnomesRunningAway?.Invoke();
            }

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
        if (gnomeAction == GnomeType.ChasePlayer)
        {
            isMoving = true;
            print("Anim: " + gnomeAnim);
            gnomeAnim.SetAnimation(1);
            StartCoroutine(MoveTowardTarget());
        }
        //Gnome wrecks the garden
        else
        {
            print("Wrecking Garden");
            //animator.SetInteger("action", 1);
        }
    }

    public enum GnomeType
    {
        ChasePlayer,
        WreckGarden
    }
}