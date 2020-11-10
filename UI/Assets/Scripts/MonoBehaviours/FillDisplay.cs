using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FillDisplay : MonoBehaviour
{
    TextMeshProUGUI textNumberOfFill;

    public float LowerLimit { get; set; } = 0.0f;
    public float UpperLimit { get; set; } = 1.0f;

    void Start()
    {
        textNumberOfFill = GetComponent<TextMeshProUGUI>();
    }

    public void TextUpdate(float value)
    {
        textNumberOfFill.text = Mathf.RoundToInt((value - LowerLimit)/(UpperLimit - LowerLimit )* 100).ToString() + "%";
    }

    public void TextUpdateNoNormalization(float value)
    {
        textNumberOfFill.text = Mathf.RoundToInt(value).ToString();
    }
}