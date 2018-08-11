using RPG.CameraUI;
using RPG.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class AbilityAdvancement : MonoBehaviour
    {
        public delegate void OnStatChanged(AbilityStat abilityStat, float amount);
        public event OnStatChanged InvokeOnStatChanged;

        public delegate void OnAbilityPointChanged();
        public event OnAbilityPointChanged InvokeOnAbilityPointChanged;

        [SerializeField] LevelExperienceCosts levelExperienceCosts;
        [SerializeField] AbilityPoints abilityPoints;
        [SerializeField] StatPoints statPoints;

        GameManager gameManager;
        UIManager uIManager;

        const float ABILITY_INCREMENT_AMOUNT = 0.1f; // TODO Come up with a better system. Possibly adding the increment amount to LevelExperienceCosts

        public AbilityPoints AbilityPoints { get { return abilityPoints; } }
        public StatPoints StatPoints { get { return statPoints; } }

        public bool HasRequiredExperience(int xpAmount) { return xpAmount <= gameManager.PlayerExperience; }

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            uIManager = gameManager.uiManager;
        }

        void Start()
        {
            uIManager.abilityButtonPanel.InvokeOnAbilityButtonsCreated += RegisterAbilityButtonEvents; // TODO Do i need to unregister before application quit to free memory?
            uIManager.abilityButtonPanel.InvokeOnAbilityCanvasClosed += RefundAbilityPoints;
            uIManager.abilityDetailsPanel.InvokeOnSelectedAbilityChanged += RefundAbilityPoints;
            uIManager.abilityStatsPanel.InvokeOnStatOperatorCreated += RegisterStatButtonEvent;
            uIManager.abilityStatsPanel.InvokeOnStatOperatorDestroyed += UnRegisterStatButtonEvent;
            uIManager.abilityStatsPanel.InvokeOnAcceptButtonClicked += ConfirmStatChange;
            uIManager.abilityStatsPanel.InvokeOnCancelButtonClicked += ResetStatChanges;
        }

        public void PurchaseAbilityPoints(Ability ability)
        {
            var requiredExperience = GetExperienceCost(ability);

            if (HasRequiredExperience(requiredExperience))
            {
                abilityPoints.PurchasePoints(ability, gameManager, requiredExperience);
                InvokeOnAbilityPointChanged();
            }
        }

        public void RefundAbilityPoints(Ability ability)
        {
            abilityPoints.RefundPoints(gameManager);
            InvokeOnAbilityPointChanged();
        }

        public int GetExperienceCost(Ability ability)
        {
            return levelExperienceCosts[ability.Level];
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
            statButton.onClick.AddListener(() => AlterStat(stat));
        }

        void UnRegisterStatButtonEvent(GameObject button)
        {
            Button statButton = button.GetComponent<Button>();
            statButton.onClick.RemoveAllListeners();
        }

        void AlterStat(AbilityStat abilityStat)
        {
            if (abilityPoints.CurrentPoints > 0)
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
                statPoints.RecordStatChange(abilityPoints, abilityStat, tempChangeAmount);
                InvokeOnStatChanged(abilityStat, tempChangeAmount);
            }
        }

        void ConfirmStatChange()
        {
            statPoints.ConfirmStatChange();
            InvokeOnAbilityPointChanged();
        }

        void ResetStatChanges() 
        {
            abilityPoints.CurrentPoints = abilityPoints.MaxPoints;
            statPoints.ResetChanges();
        }
    }
}