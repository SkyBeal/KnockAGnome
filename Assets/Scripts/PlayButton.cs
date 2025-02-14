using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 0f;

    }

    public void HitPlay()
    {

        Time.timeScale = 1f;
        Destroy(this.gameObject);

    }
}
