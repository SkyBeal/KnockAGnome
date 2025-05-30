using System.Collections.Generic;
using UnityEngine;

public class NumberConverter
{
    string[] onesArray = new[] {"zero", "one", "two", "three", "four", "five",
        "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
        "thirteen", "fourteen", "fifteen", "sixteen", "seventeen",
        "eighteen", "nineteen"};
    string[] tensArray = new[] { "zero", "ten", "twenty", "thirty", "forty",
        "fifty", "sixty", "seventy", "eighty", "ninety"};
    public string ConvertNumber(float num)
    {
        //Debug.Log(num + "convert number");
        //num = Mathf.Floor(num * 100) / 100;
        string snum = "";

        if (num < 0)
        {
            snum += "negative";
            num *= -1;
        }
        
        if (num > 20)
        {
            snum += tensArray[Mathf.FloorToInt(num) / 10] + " ";
            if (num % 10 > 0)
                snum += onesArray[Mathf.FloorToInt(num) % 10];
        }
        else if (num >= 0)
        {
            snum += onesArray[Mathf.FloorToInt(num)];
        }

        int decimalValue = Mathf.FloorToInt((num -  Mathf.FloorToInt(num)) * 100);

        snum += " dollars and ";

        if (decimalValue > 20)
        {
            snum += tensArray[Mathf.FloorToInt(decimalValue) / 10] + " ";
            if (decimalValue % 10 > 0)
                snum += onesArray[Mathf.FloorToInt(decimalValue) % 10];
        }
        else if (decimalValue >= 1)
        {
            snum += onesArray[Mathf.FloorToInt(decimalValue)];
        }
        else { }

        snum += " cents";

        snum = snum.ToUpper();
        
        return snum;
    }
}