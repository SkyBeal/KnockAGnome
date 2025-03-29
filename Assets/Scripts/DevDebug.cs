using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class DevDebug : MonoBehaviour
{

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject.Find("PlayerPrefab").GetComponent<SplineAnimate>().Play();

        }

    }

}
