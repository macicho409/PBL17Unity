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
    }

    // Update is called once per frame
    void Update()
    {  
        if((time += Time.deltaTime) >= 1)
        {
            string filePath = Application.dataPath + "/Data/" + "Needs.csv";
            try
            {
                StreamWriter writer = new StreamWriter(filePath);
                foreach (GameObject agent in agents)
                {
                    PhysiologicalModel need = agent.GetComponent<PhysiologicalModel>();
                    writer.WriteLine(need.FoodNeed.Value.ToString() + ","
                    + need.WaterNeed.Value.ToString() + ","
                    + need.DreamNeed.Value.ToString() + ","
                    + need.SexNeed.Value.ToString() + ","
                    + need.ToiletNeed.Value.ToString());
                }
                writer.Flush();
                writer.Close();
            }
            catch { }
            time = 0;
        }
    }
}
