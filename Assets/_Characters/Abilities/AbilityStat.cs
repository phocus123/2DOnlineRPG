using System;
using System.Collections.Generic;

namespace RPG.Characters
{
    [Serializable]
    public class AbilityStat
    {
        public float BaseValue;
        public string abilityName;

        bool isDirty = true;
        float value;
        readonly List<StatModifier> statModifiers;

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

        float CalculateFinalValue()
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