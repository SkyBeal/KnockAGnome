using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GnomesKilledIncrement : MonoBehaviour
{

    TMP_Text textBox;
    int currentValue = 0;
    int gnomesKilled;

    private void Start()
    {

        this.gameObject.GetNamedChild("Gnomes Killed Text").GetComponent<Animator>().enabled = true;
        textBox = this.gameObject.GetNamedChild("Gnomes Killed Text").GetComponent<TMP_Text>();
        gnomesKilled = GameObject.FindObjectOfType<LawnmowerPointsSystem>().GnomesKilled;

    }

    public void StartIncrement()
    {

        StartCoroutine(CountUpToGnomesKilled());

    }

    IEnumerator CountUpToGnomesKilled()
    {

        while(currentValue < gnomesKilled)
        {

            currentValue += 1;
            textBox.text = currentValue.ToString();

            if(currentValue == gnomesKilled)
            {

                this.gameObject.GetNamedChild("Gnomes Killed Text").GetComponent<Animator>().SetTrigger("Celebration");

            }

            yield return new WaitForSeconds(0.0000000001f);

        }

    }

}
