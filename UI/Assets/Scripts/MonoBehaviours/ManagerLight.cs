using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLight : MonoBehaviour
{
    public Light light;
    private Covid covid;
    void Start()
    {
        light.enabled = false;
        covid = this.GetComponent<Covid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (covid.CovidInfection.rangeOfInfaccted[0])
        {
            light.enabled = true;
            light.color = new Color(1f, 1f, 0f);
        }
        else if(covid.CovidInfection.rangeOfInfaccted[1])
        {
            light.enabled = true;
            light.color = new Color(0f, 0.6f, 1f);
        }
        else if(covid.CovidInfection.rangeOfInfaccted[2])
        {
            light.enabled = true;
            light.color = new Color(1f, 0.2f, 0f);
        }
        

    }
}
