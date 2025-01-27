using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class Leafblower : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    public GameObject leafBlowerHitBox;
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
        
    }

    private void Suck_Leaf_Blower(InputAction.CallbackContext obj)
    {
        isLeafBlowerActive = true;
        target = hitbox.target;
    }

    private void Shoot_Leaf_Blower(InputAction.CallbackContext obj)
    {
        isLeafBlowerActive = false;

        if (target != null)
        {

        }
    }
}