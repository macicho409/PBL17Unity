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
                LibidoNeed.Value * 0.1f; 
        } 
    }

    public ListOfNeeds? PurposeOfLife { get; set; }
    public Need FoodNeed { get; set; }
    public Need WaterNeed { get; set; }
    public Need DreamNeed { get; set; }
    public Need LibidoNeed { get; set; }
    public Need ToiletNeed { get; set; }
    public Need HigherOrderNeeds { get; set; }

    public Vector3 SleepSpot { get; set; }

    private Vector3 PreviousPosition;
    private ThirdPersonUserControl agent;
    #endregion

    void Start()
    {
        var sleepSpots = new List<Vector3>() {
                GameObject.Find("sleepSpot_0").transform.position,
                GameObject.Find("sleepSpot_1").transform.position,
                GameObject.Find("sleepSpot_2").transform.position,
                GameObject.Find("sleepSpot_3").transform.position,
                GameObject.Find("sleepSpot_4").transform.position,
                GameObject.Find("sleepSpot_5").transform.position,
                GameObject.Find("sleepSpot_6").transform.position,
                GameObject.Find("sleepSpot_7").transform.position,
                GameObject.Find("sleepSpot_8").transform.position,
                GameObject.Find("sleepSpot_9").transform.position };

        SleepSpot = sleepSpots[UnityEngine.Random.Range(0,sleepSpots.Count-1)];

        agent = GetComponent<ThirdPersonUserControl>();

        FoodNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = UnityEngine.Random.Range(0.7f*StaticContainerService.WeightTimeFood, StaticContainerService.WeightTimeFood),
            Name = "Food",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        WaterNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = UnityEngine.Random.Range(0.7f * StaticContainerService.WeightTimeWater, StaticContainerService.WeightTimeWater),
            Name = "Water",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        DreamNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = UnityEngine.Random.Range(0.7f * StaticContainerService.WeightTimeSleep, StaticContainerService.WeightTimeSleep),
            Name = "Dream",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - (action * actionCost + timeWeight) * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        LibidoNeed = new Need
        {
            LowerLimit = 0.3f,
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = UnityEngine.Random.Range(0.7f * StaticContainerService.WeightTimeSex, StaticContainerService.WeightTimeSex),
            Name = "Libido",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value - 0.3f) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        ToiletNeed = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.03f, 0.1f),
            TimeWeight = UnityEngine.Random.Range(0.7f * StaticContainerService.WeightTimeToilet, StaticContainerService.WeightTimeToilet),
            Name = "Toilet",
            OnUpdateFunc = (float value, float actionCost, float timeWeight, float action, float time) =>
            value - timeWeight * (float)Math.Sqrt(value) * time,
            OnUpdateFuncSatisfy = (float value, float timeWeight, float time) => 1.0f
        };

        HigherOrderNeeds = new Need
        {
            Value = UnityEngine.Random.Range(0.3f, 1.0f),
            ActionCost = UnityEngine.Random.Range(0.003f, 0.05f),
            TimeWeight = UnityEngine.Random.Range(0.7f * StaticContainerService.WeightTimeHigerNeeds, StaticContainerService.WeightTimeHigerNeeds),
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
        UpdateNeed(LibidoNeed, action, agent.currentPossitionNeeds.Contains(ListOfNeeds.Sex));
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