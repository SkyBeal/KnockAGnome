/*////////////////////////////////////////////////
//Rotomanager created on 11/23/24 by Tyler Hayes
//
//This script aims to allow the user to easily manipulate
//when the rotochair turns and waits through the 
//inspector. Will need to be reconfigured through
//unityEvents later
//
*/////////////////////////////////////////////////
using NaughtyAttributes;
using Roto.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotoController))]
public class RotoManager : MonoBehaviour
{
    //I LOVE ENUMS YIPPEE
    //allows me to type easily with switch statements
    public enum RotoDir
    {
        turnLeft,
        turnRight,
        wait
    }


    /// <summary>
    /// This class holds all the data needed to queue up an action
    /// </summary>
    [System.Serializable]
    public class RotoInstructions
    {

        [Tooltip("The type of action")] public RotoDir direction;
        [Tooltip("If turning, what angle to turn to"), Range(0, 359)] public int angle;
        [Tooltip("If turning, how fast to turn"), Range(0, 100)] public int power;
        [Tooltip("If waiting, how long to wait")] public float time;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="direction">The direction to turn the chair</param>
        /// <param name="power">How fast the chair turns: 0-100</param>
        /// <param name="time">If not moving the chair, how long to wait for</param>
        /// <param name="angle">What angle to turn the chair to: 0-359</param>
        public RotoInstructions(RotoDir direction, int power, float time, int angle)
        {
            this.direction = direction;
            this.power = power;
            this.time = time;
            this.angle = angle;
        }
    }

    //This is the action queue for the chair
    [SerializeField, Tooltip("Action queue for the chair")]
    private List<RotoInstructions> RotoTimeline;

    //holds a ref to the roto controller
    private RotoController rotoCon;

    //this is for the pseudo-recursion of MoveChair
    //keeps track of the current index of RotoTimeline
    [SerializeField, ReadOnly, Tooltip("What index in the list the chair is currently on")]
    private int placeInTimeline;

    //the chair is always 4 degrees off, so this should always be 4.
    //IF the chair ever gets more unsynced, change this variable
    [SerializeField, Tooltip("DEBUG ONLY - If something is off with calibration, it's probably this. DO NOT TOUCH UNLESS YOU'RE TYLER."), Header("DO NOT TOUCH UNLESS YOU ARE TYLER")]
    private int degreesOff = 4;


    /// <summary>
    /// instantiates variables
    /// </summary>
    private void Start()
    {
        rotoCon = GetComponent<RotoController>();

        //sets chair to free mode
        rotoCon.EnableFreeMode();

        //sets max power to 100
        rotoCon.SetChairTurnPower(100);

        //starts action queue
        MoveChair(RotoTimeline[0]);

    }

    /// <summary>
    /// Moves the chair based on the parameter, then
    /// calls itself again once finished until
    /// no more items are in RotoTimeline
    /// 
    /// The recursive elements can easily be reconfigured
    /// to work with UnityEvents
    /// </summary>
    /// <param name="rotoIns">holds all info needed to turn chair - 
    /// refer to the RotoInstructions class</param>
    /// <exception cref="UnityException">Throws an out of bounds exception if 
    /// the direction is not a possible value in the enum</exception>
    public void MoveChair(RotoInstructions rotoIns)
    {
        //checks to see if there are still actions queued up
        if (placeInTimeline < RotoTimeline.Count)
        {
            int tempAngle = 0;
            //checks to see the type of action
            switch (rotoIns.direction)
            {
                //turns left
                case RotoDir.turnLeft:

                    //makes sure the angle is between 0 and 359
                    tempAngle = ClampAngle(rotoIns.angle + degreesOff);

                    //yippee the rotochair is always 4 degrees off so we have to note angles like this
                    //while loop cus the roto funcs need to be constantly run instead of run once
                    while (rotoCon.GetOutputRotation() != tempAngle)
                    {
                        rotoCon.TurnLeftToAngleAtSpeed(rotoIns.angle, rotoIns.power);
                    }

                    //in order to get waiting working, I have to run this like recursion
                    placeInTimeline++;

                    //if too big, exit the loop 
                    if (placeInTimeline >= RotoTimeline.Count)
                    {
                        return;
                    }

                    MoveChair(RotoTimeline[placeInTimeline]);
                    break;

                //turns right
                case RotoDir.turnRight:

                    //makes sure the angle is between 0 and 359
                    tempAngle = ClampAngle(rotoIns.angle - degreesOff);

                    //yippee the rotochair is always 4 degrees off so we have to note angles like this
                    //while loop cus the roto funcs need to be constantly run instead of run once
                    while (rotoCon.GetOutputRotation() != tempAngle)
                    {
                        rotoCon.TurnRightToAngleAtSpeed(rotoIns.angle, rotoIns.power);
                    }

                    //in order to get waiting working, I have to run this like recursion
                    placeInTimeline++;

                    //if too big, exit the loop 
                    if (placeInTimeline >= RotoTimeline.Count)
                    {
                        return;
                    }
                    MoveChair(RotoTimeline[placeInTimeline]);
                    break;

                //waits for specified time limit
                case RotoDir.wait:

                    //just waits before starting the next command
                    StartCoroutine(countdownTimer(rotoIns.time));
                    break;
                default:
                    throw new UnityException("Somehow the switch statement in MoveCHair got to the default case");
            }
        }
    }


    #region PRIVATEFUNCS
    /// <summary>
    /// simple timer - waits then calls the next action
    /// </summary>
    /// <param name="timeToWait">how long to set the timer</param>
    /// <returns>nothing</returns>
    private IEnumerator countdownTimer(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        placeInTimeline++;
        MoveChair(RotoTimeline[placeInTimeline]);
    }

    /// <summary>
    /// Clamps the parameter to make sure its
    /// between 1 and 360
    /// </summary>
    /// <param name="angle">the angle to clamp</param>
    /// <returns>the clamped angle</returns>
    private int ClampAngle(int angle)
    {
        int tempAngle = angle;

        //clamps if too big
        if (tempAngle > 359)
        {
            tempAngle -= 360;
        }

        //clamps if too small
        else if (tempAngle < 0)
        {
            tempAngle += 360;
        }
        return tempAngle;
    }

    #endregion

    /// <summary>
    /// update is used for one off testing of the chair - depreciated once i got my 
    /// action queue working
    /// 
    /// Still here just in case
    /// </summary>
    void Update()
    {
        //rotoCon.MoveChairToZero(50);
        //rotoCon.TurnLeftToAngleAtSpeed(200, 50);
        //rotoCon.TurnRightToAngleAtSpeed(100, 50);

        //Debug.Log(rotoCon.GetOutputRotation());
    }
}
