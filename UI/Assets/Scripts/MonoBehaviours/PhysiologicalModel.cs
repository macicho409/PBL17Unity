using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using UnityEngine;
using Assets.Scripts.Models;
using UnityEngine.PlayerLoop;

public class PhysiologicalModel : MonoBehaviour
{
    #region [vars]

    public Need FoodNeed { get; set; }
    public Need WaterNeed { get; set; }
    public Need DreamNeed { get; set; }
    public Need SexNeed { get; set; }
    public Need ToiletNeed { get; set; }

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
    }

    void Update()
    {
        UpdateNeed(FoodNeed);
        UpdateNeed(WaterNeed);
        UpdateNeed(DreamNeed);
        UpdateNeed(SexNeed);
        UpdateNeed(ToiletNeed);
    }

    private void UpdateNeed(Need need)
    {
        need.Update(0, Time.deltaTime);
        UnityEngine.Debug.Log(need.Name + ": " + need.Value);
    }
}