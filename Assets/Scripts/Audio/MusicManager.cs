using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    //private EventInstance TempBGM;

    private EventInstance FinalBGM;

    private EventInstance LawnmowerHum;

    private EventInstance Ambience;

    public static MusicManager instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one FMODEvents in the scene");
        }
        instance = this;

        //TempBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.TempBGM);
        FinalBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.FinalBGM);
        LawnmowerHum = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Humming);
        Ambience = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Ambience);
        FinalBGM.setParameterByName("Music", 0); //set to title track + loop
        //TempBGM.start();
        LawnmowerHum.start();
        FinalBGM.start();
        Ambience.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchMusic(float track)
    {
        FinalBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FinalBGM.setParameterByName("Music", track); //"track" should be either 0 for title or 1 for gameplay whenever called
        FinalBGM.start();
    }

    private void OnDestroy()
    {
        FinalBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        LawnmowerHum.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Ambience.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
