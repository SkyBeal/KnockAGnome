using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DetectEnding : MonoBehaviour
{
    Rigidbody rb;

    [Tooltip("How long should the player remain still before the game ends?")]
    [SerializeField] float endTimer;

    [Tooltip("Drop the final screen here!")]
    public GameObject EndScreen;

    void Start()
    {

    }

    public void OnTriggerStay(Collider other)
    {

        EndScreen.SetActive(true);
        Time.timeScale = 0;

    }

}
