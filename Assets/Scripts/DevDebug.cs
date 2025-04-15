using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.SceneManagement;

public class DevDebug : MonoBehaviour
{
    [SerializeField] private BalloonScript balloon;
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("PlayerPrefab").GetComponent<SplineAnimate>().Play();
            balloon.StartBalloonFly();

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
