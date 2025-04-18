using NaughtyAttributes;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Collections;

public class LawnmowerPointsSystem : MonoBehaviour
{
    #region VARIABLES
    [Foldout("Design Vars"), SerializeField, Tooltip("This changes how much " +
        "each gnome is worth when you kill them")]
    private float minPointIncrease = 0.1f;
    private float maxPointIncrease = 0.05f;

    [Foldout("Design Vars"), SerializeField, Tooltip("This changes how many points " +
        "you lose when a gnome hits you")]
    private int PointLosses = 1;

    [Foldout("Design Vars"), SerializeField, Tooltip("This changes how many points you start with")]
    private int StartingPointValue = 20;

    [Foldout("Design Vars"), SerializeField, Tooltip("This sets the minimum point value the player can have")]
    private int MinimumPointValue = 0;

    [SerializeField, ReadOnly, Foldout("Debug")]
    private float points;

    public TMP_Text ScoreText;
    public TMP_Text NumericalScoreText;

    public int GnomesKilled = 0;

    NumberConverter numberConverter;
    AdminPanel adminPanel;

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
        string test = numberConverter.ConvertNumber(0.5f);
        print(test);
        GainPoints();
        
    }

    private void Awake()
    {
        StartCoroutine(delayedAdminPanel());
    }
    private IEnumerator delayedAdminPanel()
    {
        yield return null;
        adminPanel = FindObjectOfType<AdminPanel>();
    }

    public void GainPoints()
    {
        float randomPointGain = UnityEngine.Random.Range(minPointIncrease, maxPointIncrease);
        points = Mathf.Round((points + randomPointGain) * 100) / 100f;

        GnomesKilled += 1;

        UpdateScore();

    }

    public void GainPointBonus(float pointBonus)
    {
        points = Mathf.Round((points + pointBonus) * 100) / 100f;

        UpdateScore();
    }

    public void LosePoints()
    {
        points -= PointLosses;
        if (points < MinimumPointValue)
        {
            points = MinimumPointValue;
        }

        if (ScoreText != null)
            ScoreText.text = numberConverter.ConvertNumber(points);
    }

    public string AddToSecondTextBox(float score)
    {

        string numericalScore = "";


        if (score < 0)
        {

            numericalScore += "-";

        }

        numericalScore += "$";

        if (score > 20)
        {

            numericalScore += Mathf.FloorToInt(score) / 10 + "";

            if(score % 10 > 0)
            {

                numericalScore += Mathf.FloorToInt(score);

            }
            else
            {

                numericalScore += "0";

            }

        }
        else if (score >=0)
        {

            numericalScore += Mathf.FloorToInt(score);
        }

        int decimalValue = (int) ((score - Mathf.FloorToInt(score)) * 100);

        numericalScore += ".";

        if (decimalValue > 20)
        {

            numericalScore += Mathf.FloorToInt(decimalValue) / 10 + "";

            if (decimalValue % 10 > 0)
            {

                numericalScore += Mathf.FloorToInt(decimalValue) % 10;

            }
                
        }
        else if (decimalValue >= 1)
        {

            numericalScore += Mathf.FloorToInt(decimalValue);

        }

        return numericalScore;

    }
    void UpdateScore()
    {

        if (ScoreText != null)
            ScoreText.text = numberConverter.ConvertNumber(points);
        if (NumericalScoreText != null)
            NumericalScoreText.text = AddToSecondTextBox(points);
        adminPanel.UpdatePoints(points);
    }

}