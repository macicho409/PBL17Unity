using Assets.Scripts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CovidModel
{
    public bool Infected = false;

    public List<bool> rangeOfInfaccted;

    private readonly GameObject thisAgent;

    private readonly System.Random rand;

    private double SampleTime = 0;

    private readonly float distanceWeigth;
    private readonly float healthWeigth;
    private readonly float maskWeigth;
    private readonly float sphereRadius;

    public CovidModel(GameObject thisAgent, float distanceWeigth, float healthWeigth, float maskWeigth, float sphereRadius)
    {
        this.thisAgent = thisAgent;
        this.distanceWeigth = distanceWeigth;
        this.healthWeigth = healthWeigth;
        this.maskWeigth = maskWeigth;
        this.sphereRadius = sphereRadius;

        rand = new System.Random();

        rangeOfInfaccted = new List<bool>
        {
            false,
            false,
            false
        };
    }

    public void InvokeCovidCollision(List<Covid> covidAgents)
    {
        foreach(var covidAgent in covidAgents)
        {
            var health = thisAgent.GetComponent<Helath>();

            var r = sphereRadius - Mathf.Sqrt(
                Mathf.Pow(thisAgent.transform.position.x - covidAgent.transform.position.x, 2) + 
                Mathf.Pow(thisAgent.transform.position.y - covidAgent.transform.position.y, 2) + 
                Mathf.Pow(thisAgent.transform.position.z - covidAgent.transform.position.z, 2));

            var probability = 
                ((r * distanceWeigth 
                + (1.0f - health.Value) * healthWeigth 
                -  maskWeigth * (float) Convert.ToDouble(covidAgent.GetComponent<Mask>().MaskOn))).LimitToRange(0f, 1.0f);

            if((SampleTime += Time.deltaTime) >= 3)
            {
                if ((int)(probability * 100.0f) >= rand.Next(0, 100)) Infected = true;
                SampleTime = 0;
            }       
        }
    }

    public void InovkeCovidInfaction()
    {
        if(Infected && !rangeOfInfaccted[0] && !rangeOfInfaccted[1] && !rangeOfInfaccted[2])
        {
            if (33 >= rand.Next(0, 100))
                rangeOfInfaccted[0] = true;
            else if(33 < rand.Next(0, 100)  && rand.Next(0, 100) >= 66)
                rangeOfInfaccted[1] = true;
            else
                rangeOfInfaccted[2] = true;
        }
    }
}

