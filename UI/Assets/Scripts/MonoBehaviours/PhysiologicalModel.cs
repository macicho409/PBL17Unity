using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using UnityEngine;
using Assets.Scripts.Models;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Assets.Scripts.Models.Enums;

public class PhysiologicalModel : MonoBehaviour
{
    #region [vars]

    public Need FoodNeed { get; set; }
    public Need WaterNeed { get; set; }
    public Need DreamNeed { get; set; }
    public Need SexNeed { get; set; }
    public Need ToiletNeed { get; set; }

    public NeedDisplay FoodSlider { get; set; }
    public NeedDisplay WaterSlider { get; set; }
    public NeedDisplay DreamSlider { get; set; }
    public NeedDisplay SexSlider { get; set; }
    public NeedDisplay ToiletSlider { get; set; }

    #endregion

    void Start()
    {
        FoodNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.0f,
            TimeWeight = 0.01f,
            Name = "Food",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time
        };

        WaterNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.0f,
            TimeWeight = 0.01f,
            Name = "Water",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time
        };

        DreamNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.0f,
            TimeWeight = 0.01f,
            Name = "Dream",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time
        };

        SexNeed = new Need
        {
            LowerLimit = 0.3f,
            Value = 1.0f,
            ActionCost = 0.0f,
            TimeWeight = 0.01f,
            Name = "Sex",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value - 0.3f) * time
        };

        ToiletNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.0f,
            TimeWeight = 0.01f,
            Name = "Toilet",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value) * time
        };

        FoodSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderFood.ToString()).GetComponent<UnityEngine.UI.Slider>());
        WaterSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderWater.ToString()).GetComponent<UnityEngine.UI.Slider>());
        DreamSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderDream.ToString()).GetComponent<UnityEngine.UI.Slider>());
        SexSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderSex.ToString()).GetComponent<UnityEngine.UI.Slider>());
        ToiletSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderToilet.ToString()).GetComponent<UnityEngine.UI.Slider>());
    }

    void Update()
    {
        UpdateNeed(FoodNeed, FoodSlider);
        UpdateNeed(WaterNeed, WaterSlider);
        UpdateNeed(DreamNeed, DreamSlider);
        UpdateNeed(SexNeed, SexSlider);
        UpdateNeed(ToiletNeed, ToiletSlider);
    }

    private void UpdateNeed(Need need, NeedDisplay slider)
    {
        need.Update(0, Time.deltaTime);
        slider.Value = need.Value;
    }
}