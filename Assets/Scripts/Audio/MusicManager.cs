using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    private EventInstance TempBGM;

    //temp lawnmower sfx placement
    private EventInstance LawnmowerHum;
    // Start is called before the first frame update
    void Start()
    {
        TempBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.TempBGM);
        LawnmowerHum = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Humming);
        TempBGM.start();
        LawnmowerHum.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
