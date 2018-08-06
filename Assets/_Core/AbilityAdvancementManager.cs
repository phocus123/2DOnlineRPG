using RPG.CameraUI;
using RPG.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    [Serializable]
    public class AbilityAdvancementManager : MonoBehaviour
    {
        public delegate void OnStatChanged(AbilityStat abilityStat, float amount);
        public event OnStatChanged InvokeOnStatChanged;

        public delegate void OnAbilityPointChanged();
        public event OnAbilityPointChanged InvokeOnAbilityPointChanged;

        GameManager gameManager;
        UIManager uIManager;

        AbilityDetailsButton[] abilityButtons;
        AbilityPointsTransaction currentAbilityPointTransaction = null;
        AbilityStatTransaction currentAbilityStatTransaction = null;

        [SerializeField] List<int> experienceSpent; // For Testing

        const int STARTING_EXPERIENCE_COST = 100;
        const float ABILITY_INCREMENT_AMOUNT = 0.1f; // TODO Come up with a better system.

        public AbilityPointsTransaction CurrentAbilityPointTransaction { get { return currentAbilityPointTransaction; } }
        public AbilityStatTransaction CurrentAbilityStatTransaction { get { return currentAbilityStatTransaction; } }

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            uIManager = gameManager.uiManager;
        }

        void Start()
        {
            uIManager.abilityButtonPanel.InvokeOnAbilityButtonsCreated += RegisterAbilityButtonEvents; // TODO Can i leave this with no corresponding unsubscription?
            uIManager.abilityButtonPanel.InvokeOnAbilityCanvasClosed += RefundAbilityPoints;
            uIManager.abilityDetailsPanel.InvokeOnSelectedAbilityChanged += RefundAbilityPoints;
            uIManager.abilityStatsPanel.InvokeOnStatOperatorCreated += RegisterStatButtonEvent;
            uIManager.abilityStatsPanel.InvokeOnStatOperatorDestroyed += UnRegisterStatButtonEvent;
            uIManager.abilityStatsPanel.InvokeOnAcceptButtonClicked += ConfirmStatChange;
            uIManager.abilityStatsPanel.InvokeOnCancelButtonClicked += ResetStatChanges;
        }

        private void Update()
        {
            if (currentAbilityPointTransaction != null)
            {
                experienceSpent = currentAbilityPointTransaction.ExperienceSpent;
            }
        }

        public void PurchaseAbilityPoints(Ability ability)
        {
            var requiredExperience = GetExperienceCost(ability);

            if (HasRequiredExperience(requiredExperience))
            {
                if (currentAbilityPointTransaction == null)
                {
                    var tempPoints = 1;
                    List<int> tempExperienceSpent = new List<int> { requiredExperience };
                    currentAbilityPointTransaction = new AbilityPointsTransaction(tempPoints, tempExperienceSpent, ability);
                    currentAbilityPointTransaction.PurchaseAbilityPoint(gameManager, requiredExperience);
                }
                else
                {
                    currentAbilityPointTransaction.CurrentPoints++;
                    currentAbilityPointTransaction.MaxPoints++;
                    currentAbilityPointTransaction.ExperienceSpent.Add(requiredExperience);
                    currentAbilityPointTransaction.PurchaseAbilityPoint(gameManager, requiredExperience);
                }
                InvokeOnAbilityPointChanged();
            }
        }

        public void RefundAbilityPoints(Ability ability)
        {
            if (ability != null && currentAbilityPointTransaction != null && currentAbilityStatTransaction == null)
            {
                currentAbilityPointTransaction.RefundTransaction(gameManager);
                InvokeOnAbilityPointChanged();
                currentAbilityPointTransaction = null;
            }
        }

        public bool HasRequiredExperience(int xpAmount)
        {
            if (xpAmount <= gameManager.PlayerExperience){ return true; }
            else { return false; }        
        }

        public int GetExperienceCost(Ability ability)
        {
            int requiredExperience;

            if (ability.Level == 0)
            {
                requiredExperience = STARTING_EXPERIENCE_COST;
            }
            else
            {
                requiredExperience = ability.CurrentExperienceCost * ability.ExperienceMultiplier;
            }

            return requiredExperience;
        }

        void RegisterAbilityButtonEvents(GameObject button)
        {
            AbilityDetailsButton guildAbilityButton = button.GetComponent<AbilityDetailsButton>();
            uIManager.abilityDetailsPanel.InvokeOnAbilityPointPurchased += PurchaseAbilityPoints;
            guildAbilityButton.InvokeOnButtonDestroyed += UnRegisterAbilityButtonEvents;
        }

        void UnRegisterAbilityButtonEvents(AbilityDetailsButton guildAbilityButton)
        {
            uIManager.abilityDetailsPanel.InvokeOnAbilityPointPurchased -= PurchaseAbilityPoints;
            guildAbilityButton.InvokeOnButtonDestroyed -= UnRegisterAbilityButtonEvents;
        }

        void RegisterStatButtonEvent(GameObject button, AbilityStat stat)
        {
            Button statButton = button.GetComponent<Button>();
            statButton.onClick.AddListener(() => RecordStatChange(stat));
        }

        void UnRegisterStatButtonEvent(GameObject button)
        {
            Button statButton = button.GetComponent<Button>();
            statButton.onClick.RemoveAllListeners();
        }

        void RecordStatChange(AbilityStat abilityStat)
        {
            float tempChangeAmount = 0;

            switch (abilityStat.statOperator)
            {
                case AbilityStat.StatOperator.Add:
                    tempChangeAmount = ABILITY_INCREMENT_AMOUNT;
                    break;
                case AbilityStat.StatOperator.Subtract:
                    tempChangeAmount = -ABILITY_INCREMENT_AMOUNT;
                    break;
            }

            if (currentAbilityStatTransaction == null && currentAbilityPointTransaction != null && currentAbilityPointTransaction.CurrentPoints > 0)
            {
                List<AbilityStat> tempAbilityChange = new List<AbilityStat> { abilityStat };
                List<float> tempChangeAmountList = new List<float> { tempChangeAmount };
                currentAbilityStatTransaction = new AbilityStatTransaction(currentAbilityPointTransaction, tempAbilityChange, tempChangeAmountList);
                currentAbilityPointTransaction.CurrentPoints--;
                InvokeOnStatChanged(abilityStat, tempChangeAmount);
            }
            else if (currentAbilityStatTransaction != null && currentAbilityPointTransaction != null && currentAbilityPointTransaction.CurrentPoints > 0)
            {
                currentAbilityStatTransaction.AbilityChanges.Add(abilityStat);
                currentAbilityStatTransaction.AbilityChangeAmounts.Add(tempChangeAmount);
                currentAbilityPointTransaction.CurrentPoints--;
                InvokeOnStatChanged(abilityStat, tempChangeAmount);
            }
        }

        void ConfirmStatChange()
        {
            currentAbilityStatTransaction.ConfirmTransaction();
            InvokeOnAbilityPointChanged();
            currentAbilityStatTransaction = null;
        }

        void ResetStatChanges() 
        {
            currentAbilityPointTransaction.CurrentPoints = currentAbilityPointTransaction.MaxPoints;

            if (currentAbilityStatTransaction != null)
            {
                currentAbilityStatTransaction.ResetChanges();
            }
        }
    }
}