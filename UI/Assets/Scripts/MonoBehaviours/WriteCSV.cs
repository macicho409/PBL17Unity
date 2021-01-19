using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;


public class WriteCSV : MonoBehaviour
{
    private double time;
    private GameObject[] agents;

    void Start()
    {
        time = 0;
        agents = GameObject.FindGameObjectsWithTag("Agent");
        string filePath = Application.dataPath + "/Data/" + "Needs.csv";
        StreamWriter writer = new StreamWriter(filePath);
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        int counter = 0;
        if((time += Time.deltaTime) >= 1)
        {
            string filePath = Application.dataPath + "/Data/" + "Needs.csv";
            try
            {
                StreamWriter writer = new StreamWriter(filePath, true);
                foreach (GameObject agent in agents)
                {
                    PhysiologicalModel need = agent.GetComponent<PhysiologicalModel>();
                    Covid covid = agent.GetComponent<Covid>();
                    writer.WriteLine(need.FoodNeed.Value.ToString() + ","
                    + need.WaterNeed.Value.ToString() + ","
                    + need.DreamNeed.Value.ToString() + ","
                    + need.SexNeed.Value.ToString() + ","
                    + need.ToiletNeed.Value.ToString() + ","
                    + need.HigherOrderNeeds.Value.ToString());

                    if (covid.Infected)
                        counter++;
                }
                writer.WriteLine(counter.ToString());
                writer.WriteLine("");
                writer.Flush();
                writer.Close();
            }
            catch { }
            time = 0;
        }
    }
}
