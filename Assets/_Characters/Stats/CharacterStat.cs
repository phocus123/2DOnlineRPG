using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    [CreateAssetMenu(menuName = "RPG/Character Stat")]
    public class CharacterStat : ScriptableObject
    {
        public float BaseValue;
        [SerializeField] List<StatModifier> statModifiers = new List<StatModifier>();

        [SerializeField] float value;
        float lastBaseValue = Mathf.Epsilon;
        bool isDirty = true;

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

        public List<StatModifier> StatModifiers { get { return statModifiers; } }

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