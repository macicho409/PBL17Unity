using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public bool BoolMask;



    // The concept is to put on a physical mask
    void PutTakeMask()
    {
        if (BoolMask)
            BoolMask = false;
        else
            BoolMask = true;
    }
}
