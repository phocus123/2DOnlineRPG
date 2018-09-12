using System;
using RPG.Characters;
using UnityEngine;

namespace RPG.CameraUI
{
    public class TargetPanel : MonoBehaviour
    {
        HealthController currentTargetHealthController;
        AbilityController currentTargetAbilityController;

        public void ShowTargetPanel(GameObject enemy)
        {
            currentTargetHealthController = enemy.GetComponentInParent<HealthController>();
            currentTargetAbilityController = enemy.GetComponentInParent<AbilityController>();
            currentTargetHealthController.ShowEnemyHealthBar();
            gameObject.SetActive(true);
            currentTargetHealthController.Initialize(currentTargetHealthController.CurrentHealthPoints);
            currentTargetAbilityController.Initialize(currentTargetAbilityController.CurrentEnergyPoints);
            RegisterForHealthEvents();
        }

        public void HideTargetPanel()
        {
            if (currentTargetHealthController != null)
            {
                currentTargetHealthController.HideEnemyHealthBar();
                gameObject.SetActive(false);
                DeregisterForHealthEvents();
                currentTargetHealthController = null;
            }
       }

        void RegisterForHealthEvents()
        {
            currentTargetHealthController.OnHealthChange += UpdateTargetFrameHealth;
            currentTargetAbilityController.OnEnergyChanged += UpdateTargetFrameEnergy;
            currentTargetHealthController.OnCharacterDeath += HideTargetPanel;
        }

        void UpdateTargetFrameHealth(HealthController healthSystem)
        {
            healthSystem.UpdateTargetFrameHealthbar();
        }

        void UpdateTargetFrameEnergy(AbilityController abilityController)
        {
            abilityController.UpdateTargetFrameEnergybar();
        }

        void DeregisterForHealthEvents()
        {
            currentTargetHealthController.OnHealthChange -= UpdateTargetFrameHealth;
            currentTargetAbilityController.OnEnergyChanged -= UpdateTargetFrameEnergy;
            currentTargetHealthController.OnCharacterDeath -= HideTargetPanel;
        }
    }
}