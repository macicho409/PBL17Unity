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

    private Vector3 FirstPosition;

 


    #endregion

    void Start()
    {
        FoodNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.1f,
            TimeWeight = 0.01f,
            Name = "Food",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time
        };

        WaterNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.1f,
            TimeWeight = 0.01f,
            Name = "Water",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time
        };

        DreamNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.1f,
            TimeWeight = 0.01f,
            Name = "Dream",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time
        };

        SexNeed = new Need
        {
            LowerLimit = 0.3f,
            Value = 1.0f,
            ActionCost = 0.1f,
            TimeWeight = 0.01f,
            Name = "Sex",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value - 0.3f) * time
        };

        ToiletNeed = new Need
        {
            Value = 1.0f,
            ActionCost = 0.1f,
            TimeWeight = 0.01f,
            Name = "Toilet",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value) * time
        };

        FirstPosition = this.transform.position;

    }

    void Update()
    {
        float action = UpdateAction();
        UpdateNeed(FoodNeed, action);
        UpdateNeed(WaterNeed, action);
        UpdateNeed(DreamNeed, action);
        UpdateNeed(SexNeed,action);
        UpdateNeed(ToiletNeed,action);
        

    }

    private void UpdateNeed(Need need, float action)
    {
        need.Update(action, Time.deltaTime);
    }

    private float UpdateAction()
    {
        Vector3 SecondPosition = this.transform.position;

        float action = 0.1f*Mathf.Sqrt(Mathf.Pow(SecondPosition.x - FirstPosition.x, 2)
                                + Mathf.Pow(SecondPosition.y - FirstPosition.y, 2)
                                + Mathf.Pow(SecondPosition.z - FirstPosition.z, 2)) / Time.deltaTime;
        FirstPosition = SecondPosition;
        return action;
    }
}