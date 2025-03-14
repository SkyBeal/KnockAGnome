using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to be used on the ShatterParticle gameObject, runs an invoke at start that then calls a function to destroy itself.
/// Basically, it spawns a non-child temp gameObject for better particle position and it can be used for other stuff as well.
/// </summary>
public class ShatterParticleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeleteParticle", 7f);
    }
    public void DeleteParticle()
    {
        Destroy(gameObject);
    }
}
