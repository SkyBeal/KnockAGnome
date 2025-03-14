// This script manages the Gnome's different animated actions. Can and will be iterated and improved in the future this script and its contents are all tentative at the moment.
// Gnome's anims are all based on an enum: with a list of different "Animated Actions" that it could be set to preform.
//
// Place a gnome in the scene and set it's "givenAction" to give it the appropriate animation.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeAnimationManager : MonoBehaviour
{
    private Animator anim;

    public enum AnimatedActions
    {
        idle,
        run,
        hang,
        meditate,
        stuck,
        teter,
        sit
    }
    public AnimatedActions givenAction;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();


        // Takes your current assigned action (givenAction) and tells the animator to play the
        // animation that matches that action.

        switch (givenAction)
        {
            case AnimatedActions.idle:
                SetAnimation(0);
                break;
            case AnimatedActions.run:
                SetAnimation(1);
                break;
            case AnimatedActions.hang:
                SetAnimation(2);
                break;
            case AnimatedActions.meditate:
                SetAnimation(3);
                break;
            case AnimatedActions.stuck:
                SetAnimation(4);
                break;
            case AnimatedActions.teter:
                SetAnimation(5);
                break;
            case AnimatedActions.sit:
                SetAnimation(6);
                break;
            default:
                Debug.Log("Error! givenAction outside of knowable range!");
                break;
        }
    }

    public void SetAnimation(int animID)
    {
        anim.SetInteger("action", animID);
        if (animID == 1)
        {
            anim.SetInteger("runVariation", Random.Range(0, 3));
        }
    }
}
