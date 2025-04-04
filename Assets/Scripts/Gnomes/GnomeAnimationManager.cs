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
        sit,
        sleep,
        swing,
        mock,
        pull,
        twirl,
        taunt,
        booty,
        beating,
        beaten,
        wave,
        pluckgrass,
        balloon,
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
            case AnimatedActions.sleep:
                SetAnimation(7);
                break;
            case AnimatedActions.swing:
                SetAnimation(8);
                break;
            case AnimatedActions.mock:
                SetAnimation(9);
                break;
            case AnimatedActions.pull:
                SetAnimation(10);
                break;
            case AnimatedActions.twirl:
                SetAnimation(11);
                break;
            case AnimatedActions.taunt:
                SetAnimation(12);
                break;
            case AnimatedActions.booty:
                SetAnimation(13);
                break;
            case AnimatedActions.beating:
                SetAnimation(14);
                break;
            case AnimatedActions.beaten:
                SetAnimation(15);
                break;
            case AnimatedActions.wave:
                SetAnimation(16);
                break;
            case AnimatedActions.pluckgrass:
                SetAnimation(17);
                break;
            case AnimatedActions.balloon:
                SetAnimation(18);
                break;
            default:
                Debug.Log("Error! givenAction outside of knowable range!");
                break;
        }
        //Eventually I'll change this to be more efficient, with it just pulling from the order of the states in the enum and using that number
       //to call the anims but for now this works lol
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
