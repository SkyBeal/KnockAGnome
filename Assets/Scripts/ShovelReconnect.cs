using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelReconnect : MonoBehaviour
{
    [SerializeField] private GameObject connectedShovel;
    [SerializeField] private Vector3 shovelOffset;

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
        connectedShovel.transform.position = this.transform.position;
        
        connectedShovel.transform.rotation = Quaternion.Euler(
            this.transform.rotation.eulerAngles.x + shovelOffset.x, 
            this.transform.rotation.eulerAngles.y + shovelOffset.y, 
            this.transform.rotation.eulerAngles.z + shovelOffset.z);
        
        connectedShovel.SetActive(true);
    }
}
