using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Character Stats/Stat")]
    public class CharacterStat : ScriptableObject
    {
        public float BaseValue;
        [SerializeField] List<StatModifier> statModifiers;

        float value;
        bool isDirty = true;

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
    }
}