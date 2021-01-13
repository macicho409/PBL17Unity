using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public bool MaskOn;

    public float Radius {set; get;}

    void Start()
    {
        MaskOn = false;
        Radius = 5.0f;
    }

    // The concept is to put on a physical mask
    void Update()
    {
        if(MaskOn)
        {
            Radius = 2.5f; 
        }
        else
        {
            Radius = 5.0f;
        }
    }
}
