using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;
using RPG.Characters;

namespace RPG.Core
{
    public class AbilityAdvancement : MonoBehaviour
    {
        [SerializeField] LevelExperienceCosts levelExperienceCosts;
        [SerializeField] AbilityPoints abilityPoints;
        [SerializeField] StatPoints statPoints;
        [Space]
        [SerializeField] GameManager gameManager;
        [SerializeField] UIManager uIManager;

        const float ABILITY_INCREMENT_AMOUNT = 0.1f; // TODO Come up with a better system. Possibly adding the increment amount to LevelExperienceCosts

        public AbilityPoints AbilityPoints { get { return abilityPoints; } }
        public StatPoints StatPoints { get { return statPoints; } }

        public delegate void OnStatChanged(AbilityStat abilityStat, float amount);
        public event OnStatChanged InvokeOnStatChanged;
        public event Action OnAbilityPointChanged;

        public bool HasRequiredExperience(int xpAmount) { return xpAmount <= gameManager.PlayerExperience; }

        void Start()
        {
            uIManager.abilityButtonPanel.OnAbilityButtonCreated += RegisterAbilityButtonEvents; // TODO Do i need to unregister before application quit to free memory?
            uIManager.abilityButtonPanel.OnAbilityCanvasClosed += RefundAbilityPoints;
            uIManager.abilityDetailsPanel.OnSelectedAbilityChanged += RefundAbilityPoints;
            uIManager.abilityStatsPanel.InvokeOnStatOperatorCreated += RegisterStatButtonEvent;
            uIManager.abilityStatsPanel.OnStatOperatorDestroyed += UnRegisterStatButtonEvent;
            uIManager.abilityStatsPanel.OnAcceptButtonClicked += ConfirmStatChange;
            uIManager.abilityStatsPanel.OnCancelButtonClicked += ResetStatChanges;
        }

        public void PurchaseAbilityPoints(Ability ability)
        {
            var requiredExperience = GetExperienceCost(ability);

            if (HasRequiredExperience(requiredExperience))
            {
                abilityPoints.PurchasePoints(ability, requiredExperience);
                OnAbilityPointChanged();
            }
        }

        public void RefundAbilityPoints(Ability ability)
        {
            abilityPoints.RefundPoints();
            OnAbilityPointChanged();
        }

        public int GetExperienceCost(Ability ability)
        {
            return levelExperienceCosts[ability.Level];
        }

        void RegisterAbilityButtonEvents(GameObject button)
        {
            AbilityButton guildAbilityButton = button.GetComponent<AbilityButton>();
            uIManager.abilityDetailsPanel.OnAbilityPointPurchased += PurchaseAbilityPoints;
            guildAbilityButton.OnButtonDestroyed += UnRegisterAbilityButtonEvents;
        }

        void UnRegisterAbilityButtonEvents(AbilityButton guildAbilityButton)
        {
            uIManager.abilityDetailsPanel.OnAbilityPointPurchased -= PurchaseAbilityPoints;
            guildAbilityButton.OnButtonDestroyed -= UnRegisterAbilityButtonEvents;
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
            OnAbilityPointChanged();
        }

        void ResetStatChanges() 
        {
            abilityPoints.CurrentPoints = abilityPoints.MaxPoints;
            statPoints.ResetChanges();
        }
    }
}