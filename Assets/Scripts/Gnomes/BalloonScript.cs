using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour
{
    [SerializeField, Tooltip("The time the balloon lasts before despawning")] private float timeAlive;
    [SerializeField, Tooltip("How fast the balloon flies away")] private float balloonSpeed;

    private float timer;
    private void Start()
    {
        timer = 0;
    }
    public void StartBalloonFly()
    {
        StartCoroutine(BalloonFly());
    }

    IEnumerator BalloonFly()
    {
        while (timer < timeAlive)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + balloonSpeed, transform.position.z);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
