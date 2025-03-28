using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class EnvironmentalSFX : MonoBehaviour
{
    private EventInstance loopingSFX;
    // Start is called before the first frame update
    void Start()
    {
        if (this.tag.Equals("Feeder"))
        {
            loopingSFX = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Feeder);
        }
        else if (this.tag.Equals("Fountain"))
        {
            loopingSFX = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Fountain);
        }
        else if (this.tag.Equals("Tree"))
        {
            loopingSFX = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Leaves);
        }
        loopingSFX.set3DAttributes(RuntimeUtils.To3DAttributes(GetComponent<Transform>()));
        loopingSFX.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
