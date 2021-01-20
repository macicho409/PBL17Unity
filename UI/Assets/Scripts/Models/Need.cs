using Assets.Scripts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Need
    {
        /// <summary>
        /// Current value stored as private to handle range limit
        /// </summary>
        private float _val;

        public float LowerLimit { get; set; } = 0.0f;
        public float UpperLimit { get; set; } = 1.0f;

        /// <summary>
        /// To set value outside the 0-1 limit change UpperLimit and LowerLimit
        /// </summary>
        public float Value { get { return _val; } set { _val = value.LimitToRange(LowerLimit, UpperLimit); } }

        public float ActionCost { get; set; }
        public float TimeWeight { get; set; }

        /// <summary>
        /// Used as a label for debugging
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pass lambda expression to change what's happening on Update method in form: (value, actionCost, timeWeight, action, time)
        /// </summary>
        public Func<float, float, float, float, float, float> OnUpdateFunc { get; set; }
        /// <summary>
        /// Pass lambda expression to change what's happening on Update method in form: (value, timeWeight, time)
        /// </summary>
        public Func<float, float, float, float> OnUpdateFuncSatisfy { get; set; }

        /// <summary>
        /// Changes the value stored in a way described by OnUpdateFunc
        /// </summary>
        /// <param name="action">Action value</param>
        /// <param name="time">Passing time - usually Time.deltaTime</param>
        public void Update(float action, float time, bool isNeedBeingSatisfied)
        {
            if (!isNeedBeingSatisfied)
            {
                this.Value = OnUpdateFunc.Invoke(this.Value, this.ActionCost, this.TimeWeight, action, time);
            }
            else
            {
                this.Value = OnUpdateFuncSatisfy.Invoke(this.Value, this.TimeWeight, time);
            }
        }
    }
}
