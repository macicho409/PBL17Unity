using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Covid : MonoBehaviour
{
    public CovidModel CovidInfection;
    public bool Infected;

    void Start()
    {
        Infected = (UnityEngine.Random.Range(0f,1f) > 0.9f);

        var distanceWeight = UnityEngine.Random.Range(0.001f, 0.1f);
        var healthWeight = UnityEngine.Random.Range(0.001f, 0.3f);
        var maskWeight = UnityEngine.Random.Range(0.001f, 0.4f);

        CovidInfection = new CovidModel(this.gameObject, distanceWeight, healthWeight, maskWeight, this.transform.GetComponent<Mask>().Radius, Infected);
    }

    void Update()
    {
        DetectCollisions();
        CovidInfection.SphereRadius = this.transform.GetComponent<Mask>().Radius;
    }

    private void DetectCollisions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, this.transform.GetComponent<Mask>().Radius, 256);
        List<Covid> covids = new List<Covid>();

        if(!this.Infected)
        {
            foreach (var hitCollider in hitColliders)
            {
                var differentAgent = hitCollider.transform.GetComponent<Covid>();

                if (differentAgent != null && (hitCollider.name != this.name) && differentAgent.CovidInfection != null && differentAgent.CovidInfection.Infected)
                    covids.Add(differentAgent);
            }

            this.CovidInfection.InvokeCovidCollision(covids);
        }

        this.Infected = CovidInfection.Infected;
    }
}
