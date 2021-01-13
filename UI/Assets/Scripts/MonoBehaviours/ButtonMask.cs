using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMask : MonoBehaviour
{
    private GameObject[] agents;

    public bool Switch;
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("Agent");
        Switch = false;
    }


    void Update()
    {
        foreach (GameObject agent in agents)
        {
            if (Switch)
                agent.GetComponent<Mask>().MaskOn = true;
            else
                agent.GetComponent<Mask>().MaskOn = false;
        }
    }

    public void ButtonOn()
    {
        Switch = true;
    }
    public void ButtonOff()
    {
        Switch = false;
    }
}
