using NaughtyAttributes;
using Roto.Control;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanel : MonoBehaviour
{
    //stores a refrence to the admin panel
    [SerializeField, Foldout("Refs")] private GameObject panelCanvas;
    private RotoManager rotoMan;
    private bool ChairStopped = false;

    //Button Refs
    [SerializeField, Foldout("Refs")] private Toggle ChairStoppedToggle;
    [SerializeField, Foldout("Refs")] private TMP_Text chairEmerStopButtonText;
    [SerializeField, Foldout("Refs")] private Button emergencyStopButton;
    [SerializeField, Foldout("Refs")] private TMP_Text pointsText;

    /// <summary>
    /// Sets up displays
    /// </summary>
    void Start()
    {
        //grabs each display connected and turns it on
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            Debug.Log("Activated display " + Display.displays[i]);
        }

        //we set the panel active in start so it can be inactive in the 
        //scene. This stops it from gumming up how the
        //scene looks.
        panelCanvas.SetActive(true);

        rotoMan = FindObjectOfType<RotoManager>();  
    }



    #region BUTTON FUNCS

    

    public void EmergencyStopChair()
    {
        if (ChairStopped)
        {
            rotoMan.ContinueChair();
            ChairStopped = false;
            ChairStoppedToggle.isOn = false;
            chairEmerStopButtonText.text = "Emergency Stop Chair";
        }
        else
        {
            rotoMan.StopChair();
            ChairStopped = true;
            ChairStoppedToggle.isOn = true;
            chairEmerStopButtonText.text = "Turn Off Emergency Stop";
        }
          
    }

    public void ResetChairAfterGameComplete()
    {
        
        if (ChairStopped)
        {
            EmergencyStopChair();
        }
        rotoMan.ResetChairAfterGameComplete();
    }

    public void StartGame()
    {
        //start the game here
    }

    public void MoveChairToZero()
    {
        rotoMan.MoveChairToZero();
    }



    #endregion BUTTON FUNCS

    public void UpdatePoints(float pts)
    {
        pointsText.text = "Money Gained: " + pts;
    }
}
