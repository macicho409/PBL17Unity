using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Services;

public class ChangeGameSpeed : MonoBehaviour
{ 
    /// <summary>
    /// Updates game speed
    /// </summary>
    /// <param name="speed">Specify speed between 0.0f and 10.0f</param>
    public void UpdateGameSpeed(float speed)
    {
        DateTimeService.GameSpeed = speed;
    }
}
