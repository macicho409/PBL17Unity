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
using Assets.ThirdPerson;

public class PhysiologicalModel : MonoBehaviour
{
    #region [vars]

    public float Health
    { 
        get 
        { 
            return FoodNeed.Value * 0.3f + 
                WaterNeed.Value * 0.3f + 
                DreamNeed.Value * 0.15f + 
                ToiletNeed.Value * 0.15f + 
                SexNeed.Value * 0.1f; 
        } 
    }

    public enum ListOfNeeds { Food, Water, Dream, Sex, Toilet, HigherOrderNeeds };
    public ListOfNeeds PurposeOfLife { get; set; }
    public Need FoodNeed { get; set; }
    public Need WaterNeed { get; set; }
    public Need DreamNeed { get; set; }
    public Need SexNeed { get; set; }
    public Need ToiletNeed { get; set; }
    public Need HigherOrderNeeds { get; set; }

    private Vector3 PreviousPosition;
    private ThirdPersonUserControl agent;
    private int counter = 0;
    private System.Random rnd = new System.Random();

    #endregion

    void Start()
    {
        agent = GetComponent<ThirdPersonUserControl>();
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

        HigherOrderNeeds = new Need
        {
            Value = 1.0f,
            ActionCost = 0.05f,
            TimeWeight = 0.01f,
            Name = "HigherOrderNeeds",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time
        };

        PurposeOfLife = ListOfNeeds.Food;

        PreviousPosition = this.transform.position;
    }

    void Update()
    {
        /* workaround for determining purpose of life
         * agent.isPositionAcquired can be used in fute to determine
         * whether the need is satisfied*/
        counter++;
        if(agent.isPositionAcquired & counter > 100)
        {
            PurposeOfLife = (ListOfNeeds)rnd.Next(5);
            counter = 0;
        }
        /*end of workaroung*/

        float action = UpdateAction();
        UpdateNeed(FoodNeed, action);
        UpdateNeed(WaterNeed, action);
        UpdateNeed(DreamNeed, action);
        UpdateNeed(SexNeed, action);
        UpdateNeed(ToiletNeed, action);
        UpdateNeed(HigherOrderNeeds, action);
    }

    private void UpdateNeed(Need need, float action)
    {
        need.Update(action, Time.deltaTime);
    }

    private float UpdateAction()
    {
        Vector3 CurrentPosition = this.transform.position;

        float action = 0.1f*Mathf.Sqrt(Mathf.Pow(CurrentPosition.x - PreviousPosition.x, 2)
                                + Mathf.Pow(CurrentPosition.y - PreviousPosition.y, 2)
                                + Mathf.Pow(CurrentPosition.z - PreviousPosition.z, 2)) / Time.deltaTime;
        PreviousPosition = CurrentPosition;
        return action;
    }
}