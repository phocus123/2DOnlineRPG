using System;

namespace RPG.Characters
{
    [Serializable]
    public class StatModifier
    {
        public readonly float Value;

        public StatModifier(float value)
        {
            Value = value;
        }
    }
}