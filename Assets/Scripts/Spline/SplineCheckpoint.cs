using UnityEngine;

public class SplineCheckpoint : MonoBehaviour
{
    [SerializeField] private RotoManager.RotoInstructions rotoInstructions;
    [SerializeField] private Vector3[] spawnLocations;

    public void ActivateCheckPoint()
    {
        RotateChair();
        SpawnGnomes();
    }

    private void RotateChair()
    {
        print(rotoInstructions.power + " " + rotoInstructions.angle);
        PublicEventManager.RotateChair?.Invoke(rotoInstructions);
    }

    private void SpawnGnomes()
    {

    }
}