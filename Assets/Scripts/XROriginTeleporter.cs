using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROriginTeleporter : MonoBehaviour
{
    [SerializeField] private GameObject cartTeleport; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cartTeleport.transform.position;
    }
}
