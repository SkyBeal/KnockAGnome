using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.SceneManagement;

public class DevDebug : MonoBehaviour
{

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject.Find("PlayerPrefab").GetComponent<SplineAnimate>().Play();

            MusicManager.instance.switchMusic(1);

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

}
