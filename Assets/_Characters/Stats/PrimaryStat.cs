using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    [CreateAssetMenu(menuName = "RPG/Character Stats/Primary Stat")]
    public class PrimaryStat : ScriptableObject
    {
        public float BaseValue;
        [SerializeField] protected List<StatModifier> statModifiers = new List<StatModifier>();

        [SerializeField] protected float value;
        protected float lastBaseValue = Mathf.Epsilon;
        protected bool isDirty = true;

        public float Value
        {
            get
            {
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    value = CalculateFinalValue();
                    isDirty = false;
                }
                return value;
            }
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

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return didRemove;
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