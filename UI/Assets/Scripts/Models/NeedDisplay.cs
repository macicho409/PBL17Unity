using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Models;
using System;

namespace Assets.Scripts.Models
{
    public class NeedDisplay
    {
        /// <summary>
        /// Current value stored as private
        /// </summary>
        private float _val;
        private Slider _slider;

        public Slider Slider {
            get { return this._slider; }
            set{

                try 
                {
                        this._slider = value;
                }
                catch
                {

                }
            } 
        }

        /// <summary>
        /// Sets also this.Slider.value if Slider is not null
        /// </summary>
        public float Value { 
            get { return this._val; } 
            set {
                this._val = value;

                try
                {
                    this.Slider.value = this._val;
                }
                catch { }
            } 
        }

        public NeedDisplay(Slider slider, float initValue = 0.0f)
        {
            this.Slider = slider;
            this.Value = initValue;
        }
    }
}