using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("BGM")]


    [field: Header("EnvironmentSFX")]
    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference Bird { get; private set; }
    [field: SerializeField] public EventReference Feeder { get; private set; }
    [field: SerializeField] public EventReference Fire { get; private set; }
    [field: SerializeField] public EventReference Leaves { get; private set; }
    [field: SerializeField] public EventReference Sparks { get; private set; }

    [field: Header("GnomeSFX")]
    [field: SerializeField] public EventReference Donk { get; private set; }
    [field: SerializeField] public EventReference Shatter { get; private set; }
    [field: SerializeField] public EventReference Squash { get; private set; }
    //Section for voice acted SFX

    [field: Header("LawnmowerSFX")]
    [field: SerializeField] public EventReference Damage { get; private set; }
    [field: SerializeField] public EventReference Humming { get; private set; }
    [field: SerializeField] public EventReference OverMulch { get; private set; }
    [field: SerializeField] public EventReference OverRock { get; private set; }
    [field: SerializeField] public EventReference Smoking { get; private set; }

    [field: Header("OldManJenkins")]


    [field: Header("PlayerSFX")]
    [field: SerializeField] public EventReference Shovel { get; private set; }


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
