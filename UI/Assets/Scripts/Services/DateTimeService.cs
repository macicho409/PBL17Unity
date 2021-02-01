using Assets.Scripts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public static class DateTimeService
    {
        public static DateTime StartSimulationTime { get { return DateTime.MinValue.AddSeconds(StaticContainerService.CovidDelay); } }
        public static DateTime CurrentDateTime { get { return DateTime.MinValue.AddSeconds(Time.time); } }

        /// <summary>
        /// Current game speed, limited to 10.0f, cannot be less than 0.0f
        /// </summary>
        public static float GameSpeed { get { return Time.timeScale; } set { Time.timeScale = value.LimitToRange(0.0001f, 10.0f); } }
    }
}
