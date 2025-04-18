/* ---------------------------------------------------------------------------------------------+
 *@author - Ryan Herwig
 *@Last Modified - 02/04/2025
 *@Description - Controls what spline the player / cart is on. This is done by grabbing all
 *               SplineAnimate scripts on the object, then enabling and disbling them.
 *               Essentially, the SplineAnimate script will hold a section of the spline. The
 *               spline section will have a controlled speed, and when the speed needs to be
 *               changed, the new spline can control it. Once the event from SplineAnimate
 *               is triggered (which should only be triggered when the cart reaches the end
 *               of the specified spline section), the subscribed method will switch to the
 *               next spline. This is continued until all splines are completed.
 * ---------------------------------------------------------------------------------------------+/
 */

using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    [SerializeField] private int splineStartIndex;
    [SerializeField] private SplineCheckpoint[] splineCheckPoints;
    [SerializeField] private LawnmowerPointsSystem lawnmowerPointsSystem;
    private int currentSplineIndex;
    private SplineAnimate[] splinePath;

    //FOR ENDING THE GAME
    [SerializeField] float endingTimer;
    public GameObject EndScreen;

    bool endingCalled = false;

    private void Start()
    {
        //Sets current spline
        currentSplineIndex = splineStartIndex;

        //Subscribes event to method
        //SplineAnimate.ReachedEndOfSpline += SwitchSpline;

        //Gets all spline scripts on cart
        splinePath = gameObject.GetComponentsInChildren<SplineAnimate>();
        SetSplineTrack();
    }

    /// <summary>
    /// Switches to next spline after the current one is finished
    /// </summary>
    private void Update()
    {
        //out of bounds check
        if (currentSplineIndex < splinePath.Length)
        {
            //checks to see if current spline is finished
            if (splinePath[currentSplineIndex].elapsedTime >= splinePath[currentSplineIndex].duration)
            {
                //switches to next spline
                SwitchSpline();
            }
        }
        
        //if(currentSplineIndex >= splinePath.Length)
        //{

        //    if (splinePath[currentSplineIndex].elapsedTime >= splinePath[currentSplineIndex].duration)
        //    {
        //        float timer = 0;
        //        timer += Time.deltaTime;

        //        if (timer >= endingTimer)
        //        {

        //            EndScreen.SetActive(true);
        //            Time.timeScale = 0;

        //        }
        //    }

        //}

        if(currentSplineIndex == splinePath.Length)
        {

            float timer = 0;
            timer += Time.deltaTime;

            if (timer >= endingTimer && endingCalled == false)
            {
                endingCalled = true;
                CallEnding();

            }

        }

    }

    /// <summary>
    /// The method for the cart / player to switch splines. This should only be 
    /// called when the Action (which was added manually - not originally in package)
    /// is Invoked. The Action should only be Invoked when the cart has reached the
    /// end of the track AND it is set to not loop.
    /// </summary>
    private void SwitchSpline()
    {
        //Increases Spline
        currentSplineIndex++;

        SetSplineTrack();
    }

    /// <summary>
    /// Enables the SplineAnimate script that corresponds to the spline section
    /// the track is currently on.
    /// 
    /// Disables all other SplineAnimate scripts on the object this script (SplineController)
    /// is attached to
    /// </summary>
    private void SetSplineTrack()
    {
        //Checks for IndexOutOfBounds error
        if (currentSplineIndex >= splinePath.Length)
            return;

        //Sets all spline scripts to be disabled except for starting spline
        for (int i = 0; i < splinePath.Length; i++)
        {
            if (i == currentSplineIndex) //If this is the current spline
            {
                splinePath[i].enabled = true;
                splineCheckPoints[i].RotateChair();
            }
            else //All other splines
                splinePath[i].enabled = false;
        }
    }

    /*private void OnDisable()
    {
        SplineAnimate.ReachedEndOfSpline -= SwitchSpline;
    }
    */

    public void CallEnding()
    {

        float randomPointGain = Random.Range(0.5f, 0.75f);
        lawnmowerPointsSystem.GainPointBonus(randomPointGain);
        EndScreen.SetActive(true);
        EndScreen.GetComponent<Animator>().enabled = true;
        EndScreen.GetNamedChild("Score Text").GetComponent<Animator>().enabled = true;
        EndScreen.GetNamedChild("Number Score Text").GetComponent<Animator>().enabled = true;
        EndScreen.GetComponentInChildren<GnomesKilledIncrement>().enabled = true;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.OldManRambles, transform.position);

    }

}