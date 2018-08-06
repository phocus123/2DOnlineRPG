using RPG.Characters;
using System.Collections.Generic;
using System;

namespace RPG.Core
{
    public class AbilityPointsTransaction 
    {
        int currentPoints;
        int maxPoints;
        List<int> experienceSpent;
        Ability ability;

        public AbilityPointsTransaction(int currentPoints, List<int> experienceSpent, Ability ability)
        {
            this.currentPoints = currentPoints;
            this.maxPoints = currentPoints;
            this.experienceSpent = new List<int>();
            this.experienceSpent = experienceSpent;
            this.ability = ability;
        }

        public List<int> ExperienceSpent
        {
            get { return experienceSpent; }
            set { experienceSpent = value; }
        }

        public int MaxPoints
        {
            get { return maxPoints; }
            set { maxPoints = value; }
        }

        public int CurrentPoints
        {
            get { return currentPoints; }
            set { currentPoints = value; }
        }
        public Ability Ability
        {
            get { return ability; }
            set { ability = value; }
        }

        public int OffsetExperienceCost;
        public bool HasLeftOverPoints = false;

        public void PurchaseAbilityPoint(GameManager gameManager, int requiredExperience)
        {
            gameManager.PlayerExperience -= requiredExperience;
            ability.CurrentExperienceCost = requiredExperience;
            ability.Level++;
        }

        public void RefundTransaction(GameManager gameManager)
        {
            if (HasLeftOverPoints)
            {
                for (int i = experienceSpent.Count - 1; i >= 0; i--)
                {
                    currentPoints--;
                    ability.Level--;
                    gameManager.PlayerExperience += experienceSpent[i];
                    ability.CurrentExperienceCost = OffsetExperienceCost; 
                }

                HasLeftOverPoints = false;
                ClearData();
            }
            else
            {
                for (int i = experienceSpent.Count - 1; i >= 0; i--)
                {
                    currentPoints--;
                    ability.Level--;
                    gameManager.PlayerExperience += experienceSpent[i];
                    ability.CurrentExperienceCost = experienceSpent[i];

                    if (ability.Level == 1)
                    {
                        ability.CurrentExperienceCost = 100;
                    }
                }
                ClearData();
            }
           
        }

        void ClearData()
        {
            experienceSpent.Clear();
            currentPoints = 0;
            maxPoints = 0;
            ability = null;
        }
    }
}