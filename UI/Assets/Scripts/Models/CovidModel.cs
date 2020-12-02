using Assets.Scripts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Models.Enums;

public class CovidModel
{
    private bool _infected = false;

    public bool Infected
    {
        get
        {
            return _infected;
        }
        set
        {
            if (value) SpecifyInfectionType();
            else InfectionType = InfectionType.Healthy;

            _infected = value;
        }
    }

    public InfectionType InfectionType;

    private readonly GameObject thisAgent;

    private readonly System.Random rand;

    private double samplingTime = 0;

    private readonly float distanceWeigth;
    private readonly float healthWeigth;
    private readonly float maskWeigth;
    private readonly float sphereRadius;

    public CovidModel(GameObject thisAgent, float distanceWeigth, float healthWeigth, float maskWeigth, float sphereRadius, bool isInfected)
    {
        this.thisAgent = thisAgent;
        this.distanceWeigth = distanceWeigth;
        this.healthWeigth = healthWeigth;
        this.maskWeigth = maskWeigth;
        this.sphereRadius = sphereRadius;

        rand = new System.Random();

        Infected = isInfected;
    }

    public void InvokeCovidCollision(List<Covid> covidAgents)
    {
        foreach (var covidAgent in covidAgents)
        {
            var agentPhysiologicalModel = thisAgent.GetComponent<PhysiologicalModel>();

            var r = sphereRadius - Mathf.Sqrt(
                Mathf.Pow(thisAgent.transform.position.x - covidAgent.transform.position.x, 2) +
                Mathf.Pow(thisAgent.transform.position.y - covidAgent.transform.position.y, 2) +
                Mathf.Pow(thisAgent.transform.position.z - covidAgent.transform.position.z, 2));

            var probability =
                ((r * distanceWeigth
                + (1.0f - agentPhysiologicalModel.Health) * healthWeigth
                - maskWeigth * (float)Convert.ToDouble(covidAgent.GetComponent<Mask>().MaskOn))).LimitToRange(0f, 1.0f);

            if ((samplingTime += Time.deltaTime) >= 3)
            {
                if ((int)(probability * 100.0f) >= rand.Next(0, 100)) Infected = true;
                samplingTime = 0;
            }
        }
    }

    public void SpecifyInfectionType() //change infection from healthy to ill
    {
        switch(rand.Next(0, 2)) //IMPORTANT do not use 'switch' expression - Unity cannot understand modern programming style
        {
            case 0:
                InfectionType = InfectionType.InfectedWithoutSymptoms;
                break;
            case 1:
                InfectionType = InfectionType.InfectedWithSymptoms;
                break;
            default:
                InfectionType = InfectionType.SeriouslyIll;
                break;
        };
    }
}

