using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Models;


namespace Assets.Scripts.Menu
{
    public class ScriptSlider
    {

        private float _val;

        private Slider slider;
        public Slider Sllider { set { slider = value; } }

        public float LowerLimit { get; set; } = 0.0f;
        public float UpperLimit { get; set; } = 1.0f;

        public float Value { get { return _val; } set { _val = value.LimitToRange(LowerLimit, UpperLimit); } }


        public bool NeedsOn { get; set; }

        public void Updateslider(float value)
        {
            if(NeedsOn)
            {
                Value = value;
                slider.value = _val;
            }
        }
    }
}
    
