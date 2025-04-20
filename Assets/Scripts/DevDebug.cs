using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.SceneManagement;

public class DevDebug : MonoBehaviour
{
    private GameObject firstGnome;
    private void Start()
    {
        firstGnome = GameObject.Find("Gnome_First");
    }
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("PlayerPrefab").GetComponent<SplineAnimate>().Play();
            if (firstGnome != null)
            {
                firstGnome.GetComponent<GnomeBehavior>().Die();
            }

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
