using Assets.Scripts.Models.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light Light;
    private Covid covid;

    void Start()
    {
        Light.enabled = false;
        covid = this.GetComponent<Covid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (covid != null)
        {
            if (covid.CovidInfection.InfectionType == InfectionType.InfectedWithoutSymptoms)
            {
                Light.enabled = true;
                Light.color = new Color(1f, 1f, 0f);
            }
            else if (covid.CovidInfection.InfectionType == InfectionType.InfectedWithSymptoms)
            {
                Light.enabled = true;
                Light.color = new Color(0f, 0.6f, 1f);
            }
            else if (covid.CovidInfection.InfectionType == InfectionType.SeriouslyIll)
            {
                Light.enabled = true;
                Light.color = new Color(1f, 0.2f, 0f);
            }
            else //healthy one
            {
                Light.enabled = false;
            }
        }
    }
}
