using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class Leafblower : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    public GameObject leafBlowerHitBox;
    [SerializeField] Transform origin;
    private LeafblowerHitBox hitbox;
    public UnityAction ActivateLeafBlower;

    private InputAction activeLeafBlower;

    private bool isLeafBlowerActive;

    private GameObject target;
    void Start()
    {
        if (leafBlowerHitBox == null)
            Debug.LogWarning("LEAF BLOWER HITBOX IS NULL!");
        hitbox = leafBlowerHitBox.GetComponent<LeafblowerHitBox>();
    }

    private void OnEnable()
    {
        activeLeafBlower = playerInput.currentActionMap.FindAction("ActivateLeafblower");
        activeLeafBlower.performed += context => isLeafBlowerActive = true;
        activeLeafBlower.performed += Suck_Leaf_Blower;
        activeLeafBlower.canceled += Shoot_Leaf_Blower;
    }

    private void OnDisable()
    {
        activeLeafBlower.performed -= context => isLeafBlowerActive = true;
        activeLeafBlower.performed -= Suck_Leaf_Blower;
        activeLeafBlower.canceled -= Shoot_Leaf_Blower;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(2, 2, 0));
    }

    private void Suck_Leaf_Blower(InputAction.CallbackContext obj)
    {
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

    IEnumerator LeafBlowerSuck()
    {
        while (isLeafBlowerActive)
        {
            if (hitbox.target != null)
            {
                hitbox.target.transform.LookAt(origin.position - new Vector3(90, 90, 90));
                hitbox.target.GetComponent<Rigidbody>().useGravity = false;
                if (Vector3.Distance(hitbox.target.transform.position, origin.position) > 0.1f)
                    hitbox.target.transform.position = Vector3.MoveTowards(hitbox.target.transform.position, origin.position, 0.25f);
                else
                {
                    hitbox.target.transform.position = origin.position;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void LeafBlowerBlow()
    {
        if (hitbox.target != null)
        {
            hitbox.target.GetComponent<Rigidbody>().useGravity = true;
            hitbox.target.GetComponent<Rigidbody>().AddForce(hitbox.target.transform.forward * 20f, ForceMode.Impulse);
        }
    }
}