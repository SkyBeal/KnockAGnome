using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelReconnect : MonoBehaviour
{
    [SerializeField] private GameObject connectedShovel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        connectedShovel.SetActive(false);
        connectedShovel.SetActive(true);
    }
}
