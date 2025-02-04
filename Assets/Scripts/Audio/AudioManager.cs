using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private Bus masterBus;
    private Bus sfxBus;
    private Bus bgmBus;

    [Range(0, 1)]
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one AudioManager in the scene");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        bgmBus = RuntimeManager.GetBus("bus:/BGM");


    }

    void Start()
    {
        
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        //eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.GetComponent<Transform>(), gameObject.GetComponent<Rigidbody>()));
        return eventInstance;
    }

    void Update()
    {
        
    }

    public void UpdateVolume()
    {
        /*masterVolume = (SettingManager.instance.GetNumberSetting(SettingManager.NumberSettings.masterVol) / 100);
        masterBus.setVolume(masterVolume);
        sfxVolume = (SettingManager.instance.GetNumberSetting(SettingManager.NumberSettings.sfxVol) / 100);
        sfxBus.setVolume(sfxVolume);
        musicVolume = (SettingManager.instance.GetNumberSetting(SettingManager.NumberSettings.musicVol) / 100);
        bgmBus.setVolume(musicVolume);*/
    }
}
