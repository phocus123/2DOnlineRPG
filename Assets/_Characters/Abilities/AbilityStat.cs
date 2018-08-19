using System;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    public class AbilityStat 
    {
        public string statName;
        public float Value;
        public StatOperator statOperator;

        public enum StatOperator
        {
            Add,
            Subtract
        }
    }
}