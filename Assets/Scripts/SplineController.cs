using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    [SerializeField] private int splineStartIndex;
    private int currentSplineIndex;
    private SplineAnimate[] splinePath;
    private void Start()
    {
        currentSplineIndex = splineStartIndex;
        //Subscribes event to method
        SplineAnimate.ReachedEndOfSpline += SwitchSpline;

        //Gets all spline scripts on cart
        splinePath = gameObject.GetComponentsInChildren<SplineAnimate>();

        SetSplineTrack();
    }

    private void SwitchSpline()
    {
        currentSplineIndex++;

        SetSplineTrack();
    }

    private void SetSplineTrack()
    {
        //Checks for error
        if (currentSplineIndex >= splinePath.Length)
            return;

        //Sets all spline scripts to be disabled except for starting spline
        for (int i = 0; i < splinePath.Length; i++)
        {
            if (i == currentSplineIndex)
                splinePath[i].enabled = true;
            else
                splinePath[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        SplineAnimate.ReachedEndOfSpline -= SwitchSpline;
    }
}