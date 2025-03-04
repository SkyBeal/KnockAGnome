using UnityEditor.Animations;
using UnityEngine;

public class SplineCheckpoint : MonoBehaviour
{
    [SerializeField] private RotoManager.RotoInstructions rotoInstructions;
    [SerializeField] private GnomeBehavior.GnomeType[] gnomeTypes;
    [SerializeField] private AnimatorController[] animatorControllers;
    [SerializeField] private Transform[] spawnLocations;

    private GnomeManager gnomeManager;
    private void Awake()
    {
        gnomeManager = GnomeManager.Instance;
        print(gnomeManager);
    }
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
        if (gnomeManager == null)
            gnomeManager = GnomeManager.Instance;
        if (animatorControllers.Length != spawnLocations.Length && gnomeTypes.Length != animatorControllers.Length)
        {
            Debug.LogError("ERROR: NUMBER OF GNOME TYPES, ANIMATION CONTROLLERS, AND SPAWN LOCATIONS DO NOT MATCH IN SPLINE CHECK POINT");
            return;
        }
        for (int i = 0; i < animatorControllers.Length; i++)
        {
            gnomeManager.SpawnGnome(gnomeTypes[i], animatorControllers[i], spawnLocations[i]);
        }
    }
}