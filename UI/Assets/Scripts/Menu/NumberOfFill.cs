using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberOfFill : MonoBehaviour
{
    TextMeshProUGUI textNumberOfFill;
    void Start()
    {
        textNumberOfFill = GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    public void textUpddate(float value)
    {
        textNumberOfFill.text = Mathf.RoundToInt(value * 100).ToString() + "%";
    }
}
