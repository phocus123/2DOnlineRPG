using System;

namespace RPG.Characters
{
    [Serializable]
    public class StatModifier
    {
        public float Value;
        public readonly object Source;

        public StatModifier(float value, object source)
        {
            Value = value;
            Source = source;
        }

        public StatModifier(float value) : this(value, null) { }
    }
}