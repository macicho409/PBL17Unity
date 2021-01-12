using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject Capsule;
    public bool Switch { set { sswitch = value; } }
    private bool sswitch;

    void Start()
    {
        Switch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sswitch)
        {
            Capsule.SetActive(true);
        }
        else
        {
            Capsule.SetActive(false);
        }
    }
}
