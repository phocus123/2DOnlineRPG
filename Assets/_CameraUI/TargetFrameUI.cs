using System;
using RPG.Characters;
using UnityEngine;

namespace RPG.CameraUI
{
    [Serializable]
    public class TargetFrameUI
    {
        [SerializeField] GameObject targetFrame;

        HealthSystem currentTargetHealthSystem;
        AbilitySystem currentTargetAbilitySystem;

        public void RegisterForHealthEvents()
        {
            currentTargetHealthSystem.InvokeOnHealthChange += UpdateTargetFrameHealth;
            currentTargetAbilitySystem.InvokeOnEnergyChanged += UpdateTargetFrameEnergy;
            currentTargetHealthSystem.InvokeOnCharacterDeath += HideTargetFrame;
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            currentTargetHealthSystem = enemy.GetComponentInParent<HealthSystem>();
            currentTargetAbilitySystem = enemy.GetComponentInParent<AbilitySystem>();
            currentTargetHealthSystem.ShowEnemyHealthBar();
            targetFrame.SetActive(true);
            currentTargetHealthSystem.Initialize(currentTargetHealthSystem.CurrentHealthPoints, currentTargetHealthSystem.MaxHealthPoints);
            currentTargetAbilitySystem.Initialize(currentTargetAbilitySystem.CurrentEnergyPoints, currentTargetAbilitySystem.MaxEnergyPoints);
        }

        public void HideTargetFrame()
        {
            if (currentTargetHealthSystem != null)
            {
                currentTargetHealthSystem.HideEnemyHealthBar();
                targetFrame.SetActive(false);
                DeregisterForHealthEvents();
                currentTargetHealthSystem = null;
            }
        }

        void UpdateTargetFrameHealth(HealthSystem healthSystem)
        {
            healthSystem.UpdateTargetFrameHealthbar();
        }

        void UpdateTargetFrameEnergy(AbilitySystem abilitySystem)
        {
            abilitySystem.UpdateTargetFrameEnergybar();
        }

        void DeregisterForHealthEvents()
        {
            currentTargetHealthSystem.InvokeOnHealthChange -= UpdateTargetFrameHealth;
            currentTargetAbilitySystem.InvokeOnEnergyChanged -= UpdateTargetFrameEnergy;
            currentTargetHealthSystem.InvokeOnCharacterDeath -= HideTargetFrame;
        }
    }
}