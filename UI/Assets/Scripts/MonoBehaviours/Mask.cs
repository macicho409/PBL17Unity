using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public bool MaskOn;

    public Mask()
    {
        MaskOn = false;
    }

    // The concept is to put on a physical mask
    public void PutOnTakeOffMask()
    {
        if (MaskOn)
            MaskOn = false;
        else
            MaskOn = true;
    }
}
