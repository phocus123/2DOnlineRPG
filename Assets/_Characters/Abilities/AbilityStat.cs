using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    public class AbilityStat
    {
        public float BaseValue;
        public string abilityName;
        public StatOperator statOperator;
        [SerializeField] float value;

        bool isDirty = true;
        readonly List<StatModifier> statModifiers;

        public enum StatOperator
        {
            Add,
            Subtract
        }

        //public float Value
        //{
        //    get
        //    {
        //        if (isDirty)
        //        {
        //            value = CalculateFinalValue();
        //            isDirty = false;
        //        }
        //        else
        //        {
        //            value = BaseValue;
        //        }
        //        return value;
        //    }
        //}

        public float Value
        {
            get
            {
                if (value == 0)
                {
                    value = BaseValue;
                }
                return value;
            }
            set { this.value = value; }
        }

        public AbilityStat(float baseValue)
        {
            BaseValue = baseValue;
            statModifiers = new List<StatModifier>();
        }

        public void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
        }

        public bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        float CalculateFinalValue()
        {
            float finalValue = BaseValue;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                finalValue += statModifiers[i].Value;
            }

            return (float)Math.Round(finalValue, 4);
        }

        [Serializable]
        public class StatChanges
        {
            public List<float> statChanges;
        }
    }
}