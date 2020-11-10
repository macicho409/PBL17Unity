using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSpeedDisplayManager : MonoBehaviour
{
    private TextMeshProUGUI currentTimeDisplay;

    private void Awake()
    {
        currentTimeDisplay = GetComponent<TextMeshProUGUI>(); //access the text component
    }

    private void Update()
    {
        currentTimeDisplay.text = Math.Floor(DateTimeService.GameSpeed).ToString();
    }
}
