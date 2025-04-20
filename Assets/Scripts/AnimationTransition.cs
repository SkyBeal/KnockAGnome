using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransition : MonoBehaviour
{
   
    public void Transition()
    {

        GetComponent<Animator>().SetTrigger("Transitioned");

    }

}
