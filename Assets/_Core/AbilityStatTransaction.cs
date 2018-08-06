using RPG.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class AbilityStatTransaction
    {
        AbilityPointsTransaction abilityPointsTransaction;
        List<AbilityStat> abilityChanges;
        List<float> abilityChangeAmounts;

        public AbilityStatTransaction(AbilityPointsTransaction abilityPointsTransaction, List<AbilityStat> abilityChanges, List<float> abilityChangeAmounts)
        {
            this.abilityPointsTransaction = abilityPointsTransaction;
            this.abilityChanges = abilityChanges;
            this.abilityChangeAmounts = abilityChangeAmounts;
        }

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

        public void ConfirmTransaction()
        {
            for (int i = 0; i < abilityChanges.Count; i++)
            {
                abilityChanges[i].Value += abilityChangeAmounts[i];
            }
            ClearTransaction();
        }

        public void ResetChanges()
        {
            abilityChanges.Clear();
            abilityChangeAmounts.Clear();
        }

        void ClearTransaction()
        {
            if (abilityChanges.Count < abilityPointsTransaction.ExperienceSpent.Count)
            {
                for (int i = 0; i <= abilityChanges.Count - 1; i++)
                {
                    abilityPointsTransaction.OffsetExperienceCost = abilityPointsTransaction.ExperienceSpent[i];
                    abilityPointsTransaction.ExperienceSpent.RemoveAt(0);
                    abilityPointsTransaction.HasLeftOverPoints = true;
                }
            }

            abilityChanges.Clear();
            abilityChangeAmounts.Clear();
            abilityPointsTransaction = null;
        }
    }
}