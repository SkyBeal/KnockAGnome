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
    private Animator anim;

    private void Start()
    {
        anim = this.gameObject.GetNamedChild("Gnomes Killed Text").GetComponent<Animator>();
        anim.enabled = true;
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
            textBox.text = "DESTROYING " + currentValue.ToString() + " GNOMES";

            if(currentValue == gnomesKilled)
            {
                anim.SetTrigger("Celebration");
            }

            yield return null;

        }
    }
}
