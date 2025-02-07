using NaughtyAttributes;
using UnityEngine;

public class LawnmowerPointsSystem : MonoBehaviour
{
    #region VARIABLES
    [Foldout("Design Vars"), SerializeField, Tooltip("This changes how much " +
        "each gnome is worth when you kill them")]
    private int PointIncreases = 5;

    [Foldout("Design Vars"), SerializeField, Tooltip("This changes how many points " +
        "you lose when a gnome hits you")]
    private int PointLosses = 1;

    [Foldout("Design Vars"), SerializeField, Tooltip("This changes how many points you start with")]
    private int StartingPointValue = 20;

    [Foldout("Design Vars"), SerializeField, Tooltip("This sets the minimum point value the player can have")]
    private int MinimumPointValue = 0;

    [SerializeField, ReadOnly, Foldout("Debug")]
    private int points;

    [Button("Gain Points")]
    private void TempGainPoints()
    {
        GainPoints();
    }

    [Button("LosePoints")]
    private void TempLosePoints()
    {
        LosePoints();
    }

    #endregion VARIABLES   

    private void Start()
    {
        points = StartingPointValue;
    }

    public void GainPoints()
    {
        points += PointIncreases;
    }

    public void LosePoints()
    {
        points -= PointLosses;
        if (points < MinimumPointValue)
        {
            points = MinimumPointValue;
        }
    }
}