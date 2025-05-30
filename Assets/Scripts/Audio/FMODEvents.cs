using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("BGM")]
    [field: SerializeField] public EventReference TempBGM { get; private set; }
    [field: SerializeField] public EventReference FinalBGM { get; private set; }

    [field: Header("EnvironmentSFX")]
    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference Fountain { get; private set; }
    [field: SerializeField] public EventReference Leaves { get; private set; }

    [field: Header("GnomeSFX")]
    [field: SerializeField] public EventReference Shatter { get; private set; }
    [field: SerializeField] public EventReference Squash { get; private set; }
    [field: SerializeField] public EventReference Onomatopoeia { get; private set; }
    [field: SerializeField] public EventReference Attack { get; private set; }
    //Section for voice acted SFX

    [field: Header("LawnmowerSFX")]
    [field: SerializeField] public EventReference Damage { get; private set; }
    [field: SerializeField] public EventReference Humming { get; private set; }
    [field: SerializeField] public EventReference OverMulch { get; private set; }
    [field: SerializeField] public EventReference OverRock { get; private set; }

    [field: Header("OldManJenkins")]

    [field: SerializeField] public EventReference OldManRambles { get; private set; }


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
