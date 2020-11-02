using Assets.Scripts.ExtensionMethods;
using Boo.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Need
    {
        private float _val; //current value stored as private to handle range limit

        public float LowerLimit { get; set; } = 0.0f;
        public float UpperLimit { get; set; } = 1.0f;

        public float Value { get { return _val; } set { _val = value.LimitToRange(LowerLimit, UpperLimit); } }

        public float ActionCost { get; set; }
        public float TimeWeight { get; set; }

        public string Name { get; set; } //used as a label for debuggin

        public Func<float, float, float, float, float, float> OnUpdateFunc { get; set; } //pass lambda expression to change what's happening on Update method

        public void Update(float action, float time) //changes the value stored in a way described by OnUpdateFunc
        {
            this.Value = OnUpdateFunc.Invoke(this.Value, this.ActionCost, this.TimeWeight, action, time);
        }
    }
}
