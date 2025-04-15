using NaughtyAttributes;
using UnityEngine;
using TMPro;

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

    public TMP_Text ScoreText;
    public TMP_Text NumberScoreText;

    int gnomesKilled;
    public TMP_Text GnomesKilledText;

    int scoreNumberTens;
    int scoreNumberOnes;

    NumberConverter numberConverter;

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

        numberConverter = new NumberConverter();
        GainPoints();
    }

    public void GainPoints()
    {

        points += PointIncreases;
        gnomesKilled += 1;

    }

    public void LosePoints()
    {
        points -= PointLosses;
        if (points < MinimumPointValue)
        {
            points = MinimumPointValue;
        }

    }

    public void UpdateScore()
    {

        if (ScoreText != null)
            ScoreText.text = numberConverter.ConvertNumber(points);

        if(NumberScoreText != null)
        {

            if(points > 20)
            {

                scoreNumberTens = Mathf.FloorToInt(points / 10);

                if(points % 10 > 0)
                {

                    scoreNumberOnes = Mathf.FloorToInt(points % 10);

                }

            }
            else if (points >= 1)
            {

                scoreNumberOnes = Mathf.FloorToInt(points);

            }

            int decimalValue = (int)((points - Mathf.FloorToInt(points)) * 100) + 1;

            if(decimalValue < 10)
            {

                NumberScoreText.text = "$" + scoreNumberTens.ToString() + scoreNumberOnes.ToString() + ".0" + decimalValue.ToString();

            }
            else
            {

                NumberScoreText.text = "$" + scoreNumberTens.ToString() + scoreNumberOnes.ToString() + decimalValue.ToString();

            }


        }

        if(GnomesKilledText != null)
        {



        }

    }
}