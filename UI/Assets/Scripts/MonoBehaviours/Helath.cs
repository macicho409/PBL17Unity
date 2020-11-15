using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helath : MonoBehaviour
{

    public float Value { get; set; }

    PhysiologicalModel Physiomodel;

    void Start()
    {
        Physiomodel = this.transform.GetComponent<PhysiologicalModel>();
    }

    
    void Update()
    {
        Value = Psyhologicalmodels();
    }

    private float Psyhologicalmodels()
    {
        return Physiomodel.FoodNeed.Value * 0.3f + Physiomodel.WaterNeed.Value * 0.3f + Physiomodel.DreamNeed.Value * 0.15f + Physiomodel.ToiletNeed.Value * 0.15f + Physiomodel.SexNeed.Value * 0.1f;
    }

}
