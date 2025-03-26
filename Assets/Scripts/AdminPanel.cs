using NaughtyAttributes;
using Roto.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminPanel : MonoBehaviour
{
    //stores a refrence to the admin panel
    [SerializeField, Foldout("Refs")] private GameObject panelCanvas;    
    

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
    }



    #region BUTTON FUNCS

    /// <summary>
    /// testing function
    /// </summary>
    public void TestFunc()
    {
        
    }

    #endregion BUTTON FUNCS
}
