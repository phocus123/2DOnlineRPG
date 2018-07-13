using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class AbilitySystem : MonoBehaviour
    {
        public delegate void OnEnergyChanged(AbilitySystem abilitySystem);
        public event OnEnergyChanged InvokeOnEnergyChanged;

        [Header("Abilities")]
        [SerializeField] Ability[] abilities;

        [Header("Energy")]
        [SerializeField] float maxEnergyPoints;
        [SerializeField] Image energyBar;
        [SerializeField] Text playerEnergybarText;

        [Header("Enemy Target Frame")]
        [SerializeField] Image targetFrameEnergybar;
        [SerializeField] Text targetEnergybarText;

        List<AbilityBehaviour> equippedAbilityBehaviours = new List<AbilityBehaviour>();
        float currentEnergyPoints;

        public Ability[] Abilities { get { return abilities; } }
        public List<AbilityBehaviour> EquippedAbilityBehaviours { get { return equippedAbilityBehaviours; } }
        public float MaxEnergyPoints { get { return maxEnergyPoints; } }

        public float CurrentEnergyPoints
        {
            get { return currentEnergyPoints; }
            set { currentEnergyPoints = Mathf.Clamp(value, 0, maxEnergyPoints); }
        }

        float EnergyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        void Start()
        {
            AttachInitialAbilities();
            currentEnergyPoints = maxEnergyPoints;
            UpdateEnergyBar();
        }

        private void Update()
        {
            UpdateEnergyBar();
        }

        public void Initialize(float currentValue, float maxValue)
        {
            maxEnergyPoints = maxValue;
            currentEnergyPoints = currentValue;
            targetEnergybarText.text = Mathf.Round(currentEnergyPoints) + "/" + maxEnergyPoints;
            UpdateTargetFrameEnergybar();
        }

        public void AttemptAbility(AbilityBehaviour abilityBehaviour, GameObject target = null)
        {
            var energyComponent = GetComponent<AbilitySystem>();
            var energyCost = abilityBehaviour.Ability.Energy;

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilityBehaviour.Use(target);
            }
        }

        public void UpdateTargetFrameEnergybar()
        {
            if (targetFrameEnergybar)
            {
                targetFrameEnergybar.fillAmount = EnergyAsPercent;
                targetEnergybarText.text = Mathf.Round(currentEnergyPoints) + "/" + maxEnergyPoints;
            }
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
                equippedAbilityBehaviours.Add(abilities[abilityIndex].Behaviour);            
            }
        }

        void ConsumeEnergy(float energyCost)
        {
            if (InvokeOnEnergyChanged != null)
            {
                InvokeOnEnergyChanged(this);
            }

            float newEnergyamount = currentEnergyPoints - energyCost;
            currentEnergyPoints = Mathf.Clamp(newEnergyamount, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        void UpdateEnergyBar()
        {
            if (energyBar)
            {
                energyBar.fillAmount = EnergyAsPercent;
            }
            if (playerEnergybarText != null)
            {
                playerEnergybarText.text = Mathf.Round(currentEnergyPoints) + "/" + maxEnergyPoints;
            }
            if (InvokeOnEnergyChanged != null)
            {
                InvokeOnEnergyChanged(this);
            }
        }
    }
}