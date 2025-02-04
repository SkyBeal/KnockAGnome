/* ---------------------------------------------------------------------------------------------+
 *@author - Ryan Herwig
 *@Last Modified - 02/04/2025
 *@Description - Contains the logic for the leaf blower collider
 *               Determines the range of the leaf blower.
 *               This was created to save resources, since raycasting can take
 *               up a lot of resources.
 *               If no gnome is within the collider range,
 *               there will be no raycast checks
 * ---------------------------------------------------------------------------------------------+/
 */

using System;
using UnityEngine;

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
        //If a gnome enters its radius, mark it
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
                //if a gnome leaves the radius, unmark it
                foundTarget = false;
                target.GetComponent<Rigidbody>().useGravity = true; //Fail safe if object falls out of radius
                target = null;
            }
        }
    }
}