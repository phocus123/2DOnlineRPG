﻿using RPG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class AbilityPoints : MonoBehaviour 
    {
        [SerializeField] GameManager gameManager;

        int currentPoints = 0;
        int maxPoints = 0;
        List<int> experienceSpent = new List<int>();
        Ability ability;

        public List<int> ExperienceSpent { get { return experienceSpent; } }
        public int MaxPoints { get { return maxPoints; } }

        public int CurrentPoints
        {
            get { return currentPoints; }
            set { currentPoints = value; }
        }

        public void PurchasePoints(Ability ability, int requiredExperience)
        {
            this.ability = ability;
            gameManager.PlayerExperience -= requiredExperience;
            experienceSpent.Add(requiredExperience);
            currentPoints++;
            maxPoints++;
            ability.Level++;
        }

        public void RefundPoints()
        {
            for (int i = experienceSpent.Count - 1; i >= 0; i--)
            {
                currentPoints--;
                ability.Level--;
                gameManager.PlayerExperience += experienceSpent[i];
            }

            experienceSpent.Clear();
            currentPoints = 0;
            maxPoints = 0;
        }
    }
}