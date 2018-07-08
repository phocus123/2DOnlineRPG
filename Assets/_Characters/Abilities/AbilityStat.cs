using System;
using System.Collections.Generic;

namespace RPG.Characters
{
    [Serializable]
    public class AbilityStat
    {
        bool isDirty = true;
        float value;

        public float BaseValue;

        public float Value
        {
            get
            {
                if (isDirty)
                {
                    value = CalculateFinalValue();
                    isDirty = false;
                }
                else
                {
                    value = BaseValue;
                }
                return value;
            }
        }

        readonly List<StatModifier> statModifiers;
        

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
            isDirty = true;
            return statModifiers.Remove(mod);
        }

        private float CalculateFinalValue()
        {
            float finalValue = BaseValue;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                finalValue += statModifiers[i].Value;
            }

            return (float)Math.Round(finalValue, 4);
        }
    }
}