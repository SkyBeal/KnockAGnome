using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference TestSFX { get; private set; }



    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one FMODEvents in the scene");
        }
        instance = this;
    }
}
