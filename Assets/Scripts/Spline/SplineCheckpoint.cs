using UnityEngine;

public class SplineCheckpoint : MonoBehaviour
{
    [SerializeField] private RotoManager.RotoInstructions rotoInstructions;

    public void RotateChair()
    {
        print(rotoInstructions.power + " " + rotoInstructions.angle);
        PublicEventManager.RotateChair?.Invoke(rotoInstructions);
    }
}