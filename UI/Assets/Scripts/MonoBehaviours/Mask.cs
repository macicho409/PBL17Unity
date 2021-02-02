using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    private bool _maskOn;
    public bool MaskOn { 
        set {
            _maskOn = value;

            if (value)
                Radius = 2.5f;
            else
                Radius = 5.0f;
        } 
        get { return _maskOn; } 
    }

    public float Radius { set; get; }

    void Start()
    {
        MaskOn = false;
    }
}
