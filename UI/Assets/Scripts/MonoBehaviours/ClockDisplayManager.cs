using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.Services;

public class ClockDisplayManager : MonoBehaviour
{
    private TextMeshProUGUI currentTimeDisplay;

    private void Awake()
    {
        currentTimeDisplay = GetComponent<TextMeshProUGUI>(); //access the text component
    }

    private void Update()
    {
        currentTimeDisplay.text = DateTimeService.CurrentDateTime.ToString("HH:mm:ss");
    }
}
