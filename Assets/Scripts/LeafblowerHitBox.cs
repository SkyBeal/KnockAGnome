using System;
using UnityEngine;
using UnityEngine.Events;

public class LeafblowerHitBox : MonoBehaviour
{
    [NonSerialized] public bool foundTarget;
    [NonSerialized] public GameObject target;
    
    void Start()
    {
        foundTarget = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gnome"))
        {
            if (!foundTarget)
            {
                foundTarget = true;
                target = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Gnome"))
        {
            if (other.gameObject == target)
            {
                foundTarget = false;
                target = null;
            }
        }
    }
}