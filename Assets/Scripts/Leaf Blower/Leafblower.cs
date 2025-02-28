/* ---------------------------------------------------------------------------------------------+
 *@author - Ryan Herwig
 *@Last Modified - 02/04/2025
 *@Description - Controls the leafblower
 *               The leafblower can suck up gnomes and shoot them back out
 *               It uses a collider to detect if a gnome is in range and then
 *               uses a raycast that points to the gnome. If the raycast hits
 *               a gnome, it sucks it in - assuming the proper input is being held down
 *               When the proper input is released, it rotates the gnome 180 degrees and
 *               shoots it out, in the opposite direction the leaf blower is facing
 * ---------------------------------------------------------------------------------------------+/
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class Leafblower : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    public GameObject leafBlowerHitBox;
    [SerializeField] Transform origin;
    [SerializeField] LayerMask layerMask;

    [Tooltip("How much power the leaf blower has when sucking up a gnome. The higher this value is, the faster the gnome will move")]
    [SerializeField] private float leafBlowerSuckPower;
    
    [Tooltip("How much force is applied to the gnome when it is blasted away. The higher this value, the more force that will be applied")]
    [SerializeField] private float leafBlowerBlowPower;

    [Tooltip("The distance the raycast will shoot out.")]
    [SerializeField] private float rayCastDistance;

    [Tooltip("The minimum distance a gnome has to be from the origin of the vaccuum to be snapped to the origin.")]
    [SerializeField] private float gnomeSnapDistance;
    private LeafblowerHitBox hitbox;
    public UnityAction ActivateLeafBlower;

    private InputAction activeLeafBlower;

    private bool isLeafBlowerActive;

    private GameObject target;
    void Start()
    {
        //Error check
        if (leafBlowerHitBox == null)
            Debug.LogWarning("LEAF BLOWER HITBOX IS NULL!");
        hitbox = leafBlowerHitBox.GetComponent<LeafblowerHitBox>();
    }

    private void OnEnable()
    {
        //Enables inputs
        activeLeafBlower = playerInput.currentActionMap.FindAction("ActivateLeafblower");
        activeLeafBlower.started += Suck_Leaf_Blower;
        activeLeafBlower.canceled += Shoot_Leaf_Blower;
    }

    private void OnDisable()
    {
        //Disables inputs
        activeLeafBlower.started -= Suck_Leaf_Blower;
        activeLeafBlower.canceled -= Shoot_Leaf_Blower;
    }

    private void Update()
    {
        
        //Rotates for checks
        //transform.Rotate(new Vector3(2, 2, 0));

        //Draws a line for debugging
        //This can be commented out
        if (hitbox.target != null)
        {
            Debug.DrawRay(transform.position, (hitbox.target.transform.position - transform.position), Color.green);
        }
        
    }

    private void Suck_Leaf_Blower(InputAction.CallbackContext obj)
    {
        Debug.Log("AHHHHHHHHH");
        isLeafBlowerActive = true;

        //Sucks up an object that is in the leaf blower's range
        StartCoroutine(LeafBlowerSuck());
    }

    private void Shoot_Leaf_Blower(InputAction.CallbackContext obj)
    {
        isLeafBlowerActive = false;

        //This is either an error check or something that causes errors lol
        StopCoroutine(LeafBlowerSuck());

        //Shoots the object
        LeafBlowerBlow();
    }

    /// <summary>
    /// A coroutine that searches for a target, rotates it, and sucks it in
    /// </summary>
    /// <returns></returns>
    IEnumerator LeafBlowerSuck()
    {
        //If the leaf blower is currently active, vaccuums gnomes
        while (isLeafBlowerActive)
        {
            //If there is a gnome within range
            if (hitbox.target != null)
            {
                //Creates a raycast from the vaccuum to the target gnome
                RaycastHit rayCast;

                //If the raycast hits anything, save the target data
                if (Physics.Raycast(transform.position, (hitbox.target.transform.position - transform.position), out rayCast, rayCastDistance))
                {
                    Debug.Log(rayCast.transform.gameObject.name);
                    //If the raycast hit a gnome
                    if (rayCast.transform.GetComponentInParent<GnomeBehavior>())
                    {
                        //Sets target
                        target = hitbox.target;
                        //Sets rotation
                        target.transform.rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
                        //Disables gravity
                        target.GetComponentInParent<Rigidbody>().useGravity = false;

                        //Sucks the gnome to the vaccumm
                        if (Vector3.Distance(target.transform.position, origin.position) > gnomeSnapDistance)
                        {
                            Vector3 direction = target.transform.position - origin.position;
                            target.GetComponentInParent<Rigidbody>().velocity = direction.normalized * leafBlowerSuckPower;
                            //target.transform.position = Vector3.MoveTowards(target.transform.position, origin.position, Time.fixedDeltaTime * leafBlowerSuckPower * 10);
                        }
                        else
                        {
                            //If gnome is close enough to vaccuum, snap gnome to the vaccuum's origin point
                            //This saves resources and probably avoids any bugs or visual glitches
                            target.transform.position = origin.position;
                        }
                    }
                    else
                        target = null;
                }
                else
                    target = null;
            }
            else
                target = null;
            //Infinitely loops coroutine until the vaccuum stops
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Blasts the vaccuum target in a specified direction
    /// </summary>
    private void LeafBlowerBlow()
    {
        Debug.Log("Blow");
        //Checks if the target actually exists
        if (target != null)
        {
            //Re-enables gravity
            target.GetComponentInParent<Rigidbody>().useGravity = true;

            //Rotates the target to be able to be properly shot at the correct angle
            target.transform.rotation *= Quaternion.Euler(-90, 0, 0);

            //Shoots the target
            hitbox.target.GetComponentInParent<Rigidbody>().AddForce(target.transform.forward * leafBlowerBlowPower, ForceMode.Impulse);;

            //Rotates target to look like it was shot at the specified angle
            target.transform.rotation *= Quaternion.Euler(-90, 0, 0);

            //In the end, the target was rotated 180 degrees, which should be correct
        }
    }
}