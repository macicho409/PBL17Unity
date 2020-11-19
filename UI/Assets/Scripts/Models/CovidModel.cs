using Assets.Scripts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CovidModel : MonoBehaviour
{
    public bool Infected = false;

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





}

