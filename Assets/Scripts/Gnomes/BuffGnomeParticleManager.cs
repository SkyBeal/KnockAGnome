using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Script meant to be used with the massive buff gnome at the end of the game. It's super messy but.... whatever lol
/// </summary>
public class BuffGnomeParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    // 0 - rustlingGrass
    // 1 - rustlingDirt
    // 2 - dirtBurst
    // 3 - rustlingDirt_Intense
    [SerializeField] private GameObject finalParticle;

    [SerializeField] private Animator anim;


    public void PlayParticle(int particleID)
    {
        particles[particleID].Play();
    }

    public void FallOver()
    {
        Instantiate(finalParticle, transform.position, Quaternion.identity);
    }


    /// <summary>
    /// It also handles its animation stuff because im rushing this im sorry
    /// </summary>
    [Button]
    public void EnterBuffGnome()
    {
        anim.SetTrigger("buff");
    }

    public void DestroySelf()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
