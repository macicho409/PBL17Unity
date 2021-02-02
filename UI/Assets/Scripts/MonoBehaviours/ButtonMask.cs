using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMask : MonoBehaviour
{
    private GameObject[] agents;

    public bool Switch;

    public UnityEngine.UI.Button button;

    private Color red;
    private Color green;

    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("Agent");
        Switch = false;
        green = new Color(0.0f, 1.0f, 0.0f);
        red = new Color(1.0f, 0.0f, 0.0f);
    }


    void Update()
    {
        foreach (GameObject agent in agents)
        {
            if (Switch)
            {
                agent.GetComponent<Mask>().MaskOn = true;
                button.image.color = green;
            }
                
            else
            {
                agent.GetComponent<Mask>().MaskOn = false;
                button.image.color = red;
            }
        }
    }

    public void ButtonOn()
    {
        if (Switch)
            Switch = false;
        else
            Switch = true;
    }

}
