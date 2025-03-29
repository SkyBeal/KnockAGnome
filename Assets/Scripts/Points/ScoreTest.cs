using UnityEngine;

public class ScoreTest : MonoBehaviour
{
    [SerializeField] float num;
    void Start()
    {
        NumberConverter numberConverter = new NumberConverter();

        print(numberConverter.ConvertNumber(num));
    }
}
