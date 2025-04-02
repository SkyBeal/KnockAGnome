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
    private bool ChairConnected = false;
    private bool ChairStopped = false;

    //Button Refs
    [SerializeField, Foldout("Refs")] private Toggle ChairConnectedToggle;
    [SerializeField, Foldout("Refs")] private Toggle ChairStoppedToggle;
    [SerializeField, Foldout("Refs")] private TMP_Text chairConnectedButtonText;
    [SerializeField, Foldout("Refs")] private TMP_Text chairEmerStopButtonText;
    [SerializeField, Foldout("Refs")] private Button emergencyStopButton;

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

    public void ConnectChair()
    {
        if (ChairConnected)
        {
            rotoMan.DisconnectChair();
            ChairConnected = false;
            ChairConnectedToggle.isOn = false;
            chairConnectedButtonText.text = "Connect Chair";
            emergencyStopButton.interactable = false;
        }
        else
        {
            rotoMan.ConnectChair();
            ChairConnected = true;
            ChairConnectedToggle.isOn = true;
            chairConnectedButtonText.text = "Disconnect Chair";
            emergencyStopButton.interactable = true;
            
        }
        if (ChairStopped)
        {
            EmergencyStopChair();
        }
    }

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
}
