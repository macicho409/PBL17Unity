using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Services;

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

    public void TextUpdateCovidDelay(float value)
    {
        StaticContainerService.CovidDelay = Mathf.RoundToInt(value);
        textNumberOfFill.text = StaticContainerService.CovidDelay.ToString();
    }

    public void TextUpdateNoAgents(float value)
    {
        StaticContainerService.NoAgents = Mathf.RoundToInt(value);
        textNumberOfFill.text = StaticContainerService.NoAgents.ToString();
    }

    public void TextUpdateSampleTimeCovid(float value)
    {
        StaticContainerService.SampleTimeCovid = Mathf.RoundToInt(value);
        textNumberOfFill.text = StaticContainerService.SampleTimeCovid.ToString();
    }

    public void TextUpdateWeightTimeSleep(float value)
    {
        StaticContainerService.WeightTimeSleep = value;
        textNumberOfFill.text = StaticContainerService.WeightTimeSleep.ToString();
    }

    public void TextUpdateWeightTimeFood(float value)
    {
        StaticContainerService.WeightTimeFood = value;
        textNumberOfFill.text = StaticContainerService.WeightTimeFood.ToString();
    }

    public void TextUpdateWeightTimeWater(float value)
    {
        StaticContainerService.WeightTimeWater = value;
        textNumberOfFill.text = StaticContainerService.WeightTimeWater.ToString();
    }

    public void TextUpdateWeightTimeSex(float value)
    {
        StaticContainerService.WeightTimeSex = value;
        textNumberOfFill.text = StaticContainerService.WeightTimeSex.ToString();
    }

    public void TextUpdateWeightTimeToilet(float value)
    {
        StaticContainerService.WeightTimeToilet = value;
        textNumberOfFill.text = StaticContainerService.WeightTimeToilet.ToString();
    }

    public void TextUpdateWeightTimeHigerNeeds(float value)
    {
        StaticContainerService.WeightTimeHigerNeeds = value;
        textNumberOfFill.text = StaticContainerService.WeightTimeHigerNeeds.ToString();
    }

    public void TextWeightTime(float value)
    {
        textNumberOfFill.text = value.ToString();
    }
}