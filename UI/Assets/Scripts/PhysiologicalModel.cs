using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using UnityEngine;



public class PhysiologicalModel : MonoBehaviour
{
  
    /*===================================================================================       
                                     STATE VARIABLES
     ===================================================================================*/

    private float food;
    public float Food { get { return food; } set { food = value; } }

    private float water;
    public float Water { get { return water; } set { water = value; } }

    private float sleep;
    public float Sleep { get { return sleep; } set { sleep = value; } }

    private float sex;
    public float Sex { get { return sex; } set { sex = value; } }

    private float dream;
    public float Dream { get { return dream; } set { dream = value; } }

    private float toilet;
    public float Toilet { get { return toilet; } set { toilet = value; } }

    /*===================================================================================       
                                    ACTION VARIABLES
     ===================================================================================*/

    private float action;
    public float Action { get { return action; } set { action = value; } }

    private float actioncostfood;
    public float ActionCostFood { get { return actioncostfood; } set { actioncostfood = value; } }

    private float actioncostwater;
    public float ActionCostWater { get { return actioncostwater; } set { actioncostwater = value; } }

    private float actioncostdream;
    public float ActionCostDream { get { return actioncostdream; } set { actioncostdream = value;  } }

    /*===================================================================================       
                                   WEIGHT VARIABLES
     ===================================================================================*/

    private float timeweightfood;
    public float TimeWeightFood { get { return timeweightfood; } set { timeweightfood = value; } }

    private float timeweightwater;
    public float TimeWeightWater { get { return timeweightwater; } set { timeweightwater = value; } }

    private float timeweightdream;
    public float TimeWeightDream { get { return timeweightdream; } set { timeweightdream = value; } }

    private float timeweightsex;
    public float TimeWeightSex { get { return timeweightsex; } set { timeweightsex = value; } }

    private float timeweightoilet;
    public float TimeWeightToilet { get { return timeweightoilet; } set { timeweightoilet = value; } }








    /*===================================================================================       
                                 INITIAL VALUES OF VARIABLES
     ===================================================================================*/

    void Start()
    {
        // variables declared fo test

        //State declared
        food = 1.0f;
        water = 1.0f;
        sleep = 1.0f;
        sex = 1.0f;
        dream = 1.0f;
        toilet = 1.0f;

        //Action declared
        action = 0.0f;
        actioncostfood = 0.0f;
        actioncostwater = 0.0f;
        actioncostdream = 0.0f;

        //Weight declared
        timeweightfood = 0.01f;
        timeweightwater = 0.01f;
        timeweightdream = 0.01f;
        timeweightsex = 0.01f;
        timeweightoilet = 0.01f;
    }


    /*===================================================================================       
                                    UPDATE VALUES PER FRAME 
     ===================================================================================*/

    void Update()
    {
        UpdateFood();
        UpdateWater();
        UpdateDream();
        UpdateSex();
        UpdateToilet();
    }


    /*===================================================================================       
                                            FUNCTION 
     ===================================================================================*/


    private void UpdateFood()
    {
        food = food - (action * actioncostfood + timeweightfood) * (float)Math.Sqrt(food) * Time.deltaTime;

        if(food <= 0.0f)
        {
            food = 0.0f;
        }
        else if(food >= 1.0f)
        {
            food = 1.0f;
        }

        UnityEngine.Debug.Log("Food: " + food);
    }

    private void UpdateWater()
    {
        water = water - (action * actioncostwater + timeweightwater) * (float)Math.Sqrt(water) * Time.deltaTime;

        if(water <= 0.0f)
        {
            water = 0.0f;
        }
        else if(water >= 1.0f)
        {
            water = 1.0f;
        }
        UnityEngine.Debug.Log("Water: " + water);
    }

    private void UpdateDream()
    {
        dream = dream - (action * actioncostdream + timeweightdream) * (float)Math.Sqrt(dream) * Time.deltaTime;

        if (dream <= 0.0f)
        {
            dream = 0.0f;
        }
        else if (dream >= 1.0f)
        {
            dream = 1.0f;
        }

        UnityEngine.Debug.Log("Dream: " + dream);
    }

    private void UpdateSex()
    {
        sex = sex - timeweightsex * (float)Math.Sqrt(Math.Abs(sex - 0.3f)) * Time.deltaTime;

        if (sex <= 0.3f)
        {
            sex = 0.3f;
        }
        else if (dream >= 1.0f)
        {
            sex = 1.0f;
        }

        UnityEngine.Debug.Log("Sex: " + sex);
    }

    private void UpdateToilet()
    {
        toilet = toilet - timeweightoilet * (float)Math.Sqrt(toilet) * Time.deltaTime;

        if (toilet <= 0.0f)
        {
            toilet = 0.0f;
        }
        else if (toilet >= 1.0f)
        {
            toilet = 1.0f;
        }

        UnityEngine.Debug.Log("Toilet: " + toilet);
    }


}
