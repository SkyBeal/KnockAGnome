using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    [SerializeField] private int splineStartIndex;
    private SplineAnimate[] splinePath;

    private void Start()
    {
        splinePath = gameObject.GetComponentsInChildren<SplineAnimate>();

        for (int i = 0; i < splinePath.Length; i++)
        {
            if (i == splineStartIndex)
                splinePath[i].enabled = true;
            else
                splinePath[i].enabled = false;
        }

        SplineAnimate.
    }

    private void OnDisable()
    {

    }
}