using System;
using RPG.Characters;
using UnityEngine;

namespace RPG.CameraUI
{
    [Serializable]
    public class TargetFrameUI
    {
        [SerializeField] GameObject targetFrame;

        HealthController currentTargetHealthController;
        AbilityController currentTargetAbilityController;

        public void RegisterForHealthEvents()
        {
            currentTargetHealthController.OnHealthChange += UpdateTargetFrameHealth;
            currentTargetAbilityController.OnEnergyChanged += UpdateTargetFrameEnergy;
            currentTargetHealthController.OnCharacterDeath += HideTargetFrame;
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            currentTargetHealthController = enemy.GetComponentInParent<HealthController>();
            currentTargetAbilityController = enemy.GetComponentInParent<AbilityController>();
            currentTargetHealthController.ShowEnemyHealthBar();
            targetFrame.SetActive(true);
            currentTargetHealthController.Initialize(currentTargetHealthController.CurrentHealthPoints);
            currentTargetAbilityController.Initialize(currentTargetAbilityController.CurrentEnergyPoints);
            RegisterForHealthEvents();
        }

        public void HideTargetFrame()
        {
            if (currentTargetHealthController != null)
            {
                currentTargetHealthController.HideEnemyHealthBar();
                targetFrame.SetActive(false);
                DeregisterForHealthEvents();
                currentTargetHealthController = null;
            }
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
            currentTargetHealthController.OnCharacterDeath -= HideTargetFrame;
        }
    }
}