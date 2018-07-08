﻿using RPG.Characters;
using UnityEngine;

namespace RPG.Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject targetFrame;

        HealthSystem currentTargetHealthSystem;

        public void ShowTargetFrame(GameObject enemy)
        {
            currentTargetHealthSystem = enemy.GetComponentInParent<HealthSystem>();
            currentTargetHealthSystem.ShowEnemyHealthBar();
            targetFrame.SetActive(true);
            RegisterForHealthEvents();
            currentTargetHealthSystem.Initialize(currentTargetHealthSystem.CurrentHealthPoints, currentTargetHealthSystem.MaxHealthPoints);
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

        public void UpdateTargetFrame(float value, HealthSystem healthSystem)
        {
            healthSystem.CurrentHealthPoints = value;
            healthSystem.UpdateTargetFrameHealthbar();
        }

        void RegisterForHealthEvents()
        {
            currentTargetHealthSystem.InvokeOnStatChange += UpdateTargetFrame;
            currentTargetHealthSystem.InvokeOnCharacterDestroyed += HideTargetFrame;
        }

        void DeregisterForHealthEvents()
        {
            currentTargetHealthSystem.InvokeOnStatChange -= UpdateTargetFrame;
            currentTargetHealthSystem.InvokeOnCharacterDestroyed -= HideTargetFrame;
        }
    }
}