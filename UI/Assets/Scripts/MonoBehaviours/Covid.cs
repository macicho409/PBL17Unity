using System.Collections.Generic;
using UnityEngine;

public class Covid : MonoBehaviour
{
    public CovidModel CovidInfection;
    public bool Infected;

    void Start()
    {
        CovidInfection = new CovidModel(this.gameObject, 0.1f, 0.1f, 0.1f, 4)
        {
            Infected = Infected
        };
    }

    void Update()
    {
        DetectCollisions();
    }

    private void DetectCollisions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 4.0f, 256);
        List<Covid> covids = new List<Covid>();

        foreach (var hitCollider in hitColliders)
        {
            var differentAgent = hitCollider.transform.GetComponent<Covid>();

            if ((hitCollider.name != this.name) && differentAgent.CovidInfection.Infected)
                covids.Add(differentAgent);
        }

        this.CovidInfection.InvokeCovidCollision(covids);
    }
}
