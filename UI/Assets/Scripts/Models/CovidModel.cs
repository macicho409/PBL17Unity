using Assets.Scripts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Models.Enums;
using Assets.Scripts.Services;

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

    public float SphereRadius { get; set; }

    public CovidModel(GameObject thisAgent, float distanceWeigth, float healthWeigth, float maskWeigth, float sphereRadius, bool isInfected)
    {
        this.thisAgent = thisAgent;
        this.distanceWeigth = distanceWeigth;
        this.healthWeigth = healthWeigth;
        this.maskWeigth = maskWeigth;
        this.SphereRadius = sphereRadius;

        Infected = isInfected;
    }

    public void InvokeCovidCollision(List<Covid> covidAgents)
    {
        var probability = 0f;
        var agentPhysiologicalModel = thisAgent.GetComponent<PhysiologicalModel>();

        foreach (var covidAgent in covidAgents)
        {
            var r = SphereRadius - Mathf.Sqrt(
                Mathf.Pow(thisAgent.transform.position.x - covidAgent.transform.position.x, 2) +
                Mathf.Pow(thisAgent.transform.position.y - covidAgent.transform.position.y, 2) +
                Mathf.Pow(thisAgent.transform.position.z - covidAgent.transform.position.z, 2));

            probability +=
                (r * distanceWeigth
                + (1.0f - agentPhysiologicalModel.Health) * healthWeigth
                - maskWeigth * (float)Convert.ToDouble(covidAgent.GetComponent<Mask>().MaskOn)).LimitToRange(0f, 1.0f);
        }

        if ((samplingTime += Time.deltaTime) >= StaticContainerService.SampleTimeCovid)
        {
            if ((int)(probability * 2.0f) >= UnityEngine.Random.Range(0, 100)) Infected = true;
            samplingTime = 0;
        }
    }

    public void SpecifyInfectionType() //change infection from healthy to ill
    {
        var infectionTypeProbability = UnityEngine.Random.Range(0f, 1f);

        if(infectionTypeProbability < 0.5f)
                InfectionType = InfectionType.InfectedWithoutSymptoms;
        else if (infectionTypeProbability < 0.96f)
            InfectionType = InfectionType.InfectedWithSymptoms;
        else
            InfectionType = InfectionType.SeriouslyIll;
    }
}

