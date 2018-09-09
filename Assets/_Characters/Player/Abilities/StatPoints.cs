using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class StatPoints : MonoBehaviour
    {
        AbilityPoints abilityPoints;
        List<AbilityStat> abilityChanges = new List<AbilityStat>();
        List<float> abilityChangeAmounts = new List<float>();

        public List<AbilityStat> AbilityChanges
        {
            get { return abilityChanges; }
            set { abilityChanges = value; }
        }

        public List<float> AbilityChangeAmounts
        {
            get { return abilityChangeAmounts; }
            set { abilityChangeAmounts = value; }
        }

        public void RecordStatChange(AbilityPoints abilityPoints, AbilityStat abilityStat, float amount)
        {
            this.abilityPoints = abilityPoints;
            abilityChanges.Add(abilityStat);
            abilityChangeAmounts.Add(amount);
            abilityPoints.CurrentPoints--;
        }

        public void ConfirmStatChange()
        {
            for (int i = 0; i < abilityChanges.Count; i++)
            {
                abilityChanges[i].Value += abilityChangeAmounts[i];
            }
            RemoveExperienceSpent();
        }

        public void ResetChanges()
        {
            abilityChanges.Clear();
            abilityChangeAmounts.Clear();
        }

        void RemoveExperienceSpent()
        {
            if (abilityChanges.Count <= abilityPoints.ExperienceSpent.Count)
            {
                for (int i = 0; i <= abilityChanges.Count - 1; i++)
                {
                    abilityPoints.ExperienceSpent.RemoveAt(0);
                }
            }

            ResetChanges();
        }
    }
}