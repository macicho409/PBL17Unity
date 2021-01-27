using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using Assets.Scripts.Services;

public class WriteCSV : MonoBehaviour
{
    private double time;
    private GameObject[] agents;
    private int i = 0;
    private string filePath;

    // Update is called once per frame
    void Update()
    {
        i++;
        if (i == 300)
        {
            try
            {
                Directory.CreateDirectory(Application.dataPath + "/Data");
            }
            catch
            {
            }

            time = 0;
            agents = GameObject.FindGameObjectsWithTag("Agent");
            filePath = Application.dataPath + "/Data/" + "Needs_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
            StreamWriter writer = new StreamWriter(filePath);
            writer.Close();
        }

        int covidCounter = 0;
        int covidIllWithNoSypmtomsCounter = 0;
        int covidIllWithSypmtomsCounter = 0;
        int covidSeriouslyIllCounter = 0;

        IFormatProvider iFormatProvider = new System.Globalization.CultureInfo("en");

        if ((time += Time.deltaTime) >= 10)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath, true);

                writer.WriteLine("#TIMESTAMP");
                writer.WriteLine(DateTimeService.CurrentDateTime);

                writer.WriteLine("#NEEDS");

                foreach (GameObject agent in agents)
                {
                    PhysiologicalModel need = agent.GetComponent<PhysiologicalModel>();
                    Covid covid = agent.GetComponent<Covid>();
                    writer.WriteLine(String.Format(iFormatProvider, "{0:0.###}", need.FoodNeed.Value) + ","
                    + String.Format(iFormatProvider, "{0:0.###}", need.WaterNeed.Value) + ","
                    + String.Format(iFormatProvider, "{0:0.###}", need.DreamNeed.Value) + ","
                    + String.Format(iFormatProvider, "{0:0.###}", need.LibidoNeed.Value) + ","
                    + String.Format(iFormatProvider, "{0:0.###}", need.ToiletNeed.Value) + ","
                    + String.Format(iFormatProvider, "{0:0.###}", need.HigherOrderNeeds.Value));

                    if (covid.Infected)
                        covidCounter++;
                    if (covid.CovidInfection.InfectionType == Assets.Scripts.Models.Enums.InfectionType.InfectedWithoutSymptoms)
                        covidIllWithNoSypmtomsCounter++;
                    if (covid.CovidInfection.InfectionType == Assets.Scripts.Models.Enums.InfectionType.InfectedWithSymptoms)
                        covidIllWithSypmtomsCounter++;
                    if (covid.CovidInfection.InfectionType == Assets.Scripts.Models.Enums.InfectionType.SeriouslyIll)
                        covidSeriouslyIllCounter++;
                }

                writer.WriteLine("#INFECTED");
                writer.WriteLine(covidCounter.ToString());
                writer.WriteLine("#INFECTED_NO_SYMPTOMS");
                writer.WriteLine(covidIllWithNoSypmtomsCounter.ToString());
                writer.WriteLine("#INFECTED_MILD_SYMPTOMS");
                writer.WriteLine(covidIllWithSypmtomsCounter.ToString());
                writer.WriteLine("#INFECTED_SERIOUSLY_ILL");
                writer.WriteLine(covidSeriouslyIllCounter.ToString());
                writer.WriteLine("");
                writer.Flush();
                writer.Close();
            }
            catch { }
            time = 0;
        }
    }
}
