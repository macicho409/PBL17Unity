using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public static class StaticContainerService
    {
        public static int NoAgents = 50;

        public static float WeightTimeSleep = 0.0025f;
        public static float WeightTimeFood = 0.0025f;
        public static float WeightTimeWater = 0.0025f;
        public static float WeightTimeToilet = 0.0025f;
        public static float WeightTimeSex = 0.0025f;
        public static float WeightTimeHigerNeeds = 0.0025f;

        public static int SampleTimeCovid = 10;

        public static int CovidDelay = 30;
    }
}
