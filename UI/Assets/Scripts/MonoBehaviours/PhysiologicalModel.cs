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
using Debug = UnityEngine.Debug;
using Assets.Scripts.Services;

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

    public ListOfNeeds? PurposeOfLife { get; set; }
    public Need FoodNeed { get; set; }
    public Need WaterNeed { get; set; }
    public Need DreamNeed { get; set; }
    public Need SexNeed { get; set; }
    public Need ToiletNeed { get; set; }
    public Need HigherOrderNeeds { get; set; }

    private Vector3 PreviousPosition;
    private ThirdPersonUserControl agent;
    private int counter = 0;
    private readonly System.Random rnd = new System.Random();



    #endregion

    void Start()
    {
        agent = GetComponent<ThirdPersonUserControl>();


        FoodNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = StaticContainerService.WeightTimeFood,
            Name = "Food",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        WaterNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = StaticContainerService.WeightTimeWater,
            Name = "Water",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        DreamNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = StaticContainerService.WeightTimeSleep,
            Name = "Dream",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        SexNeed = new Need
        {
            LowerLimit = 0.3f,
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = StaticContainerService.WeightTimeSex,
            Name = "Sex",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value - 0.3f) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        ToiletNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = StaticContainerService.WeightTimeToilet,
            Name = "Toilet",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        HigherOrderNeeds = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.003f, 0.05f),
            TimeWeight = StaticContainerService.WeightTimeHigerNeeds,
            Name = "HigherOrderNeeds",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        PurposeOfLife = null;

        PreviousPosition = this.transform.position;
    }

    void Update()
    {
        /* workaround for determining purpose of life
         * agent.isPositionAcquired can be used in fute to determine
         * whether the need is satisfied*/
        //counter++;
        //if (counter > 100)
        //{
        //    PurposeOfLife = (ListOfNeeds)rnd.Next(6);
        //    counter = 0;
        //    Debug.Log(PurposeOfLife.ToString());
        //}
        /*end of workaroung*/

        float action = UpdateAction();
        UpdateNeed(FoodNeed, action, agent.currentPossitionNeeds.Contains(ListOfNeeds.Food));
        UpdateNeed(WaterNeed, action, agent.currentPossitionNeeds.Contains(ListOfNeeds.Water));
        UpdateNeed(DreamNeed, action, agent.currentPossitionNeeds.Contains(ListOfNeeds.Dream));
        UpdateNeed(SexNeed, action, agent.currentPossitionNeeds.Contains(ListOfNeeds.Sex));
        UpdateNeed(ToiletNeed, action, agent.currentPossitionNeeds.Contains(ListOfNeeds.Toilet));
        UpdateNeed(HigherOrderNeeds, action, agent.currentPossitionNeeds.Contains(ListOfNeeds.HigherOrderNeeds));
    }

    private void UpdateNeed(Need need, float action, bool isNeedBeingSatisfied)
    {
        need.Update(action, Time.deltaTime, isNeedBeingSatisfied);
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