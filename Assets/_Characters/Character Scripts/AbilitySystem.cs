using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class AbilitySystem : MonoBehaviour
    {
        public delegate void OnEnergyChanged();
        public event OnEnergyChanged InvokeOnEnergyChanged;

        [Header("Abilities")]
        [SerializeField] Ability[] abilities;

        [Header("Energy")]
        [SerializeField] float maxEnergyPoints;
        [SerializeField] Image energyBar;
        [SerializeField] Text playerEnergybarText;

        float currentEnergyPoints;

        public Ability[] Abilities { get { return abilities; } }

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

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public void AttemptAbility(Ability ability, GameObject target = null)
        {
            var energyComponent = GetComponent<AbilitySystem>();
            var energyCost = ability.Energy;

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                ability.Use(target);
            }
        }

        private void ConsumeEnergy(float energyCost)
        {
            if (InvokeOnEnergyChanged != null)
            {
                InvokeOnEnergyChanged();
            }

            float newEnergyamount = currentEnergyPoints - energyCost;
            currentEnergyPoints = Mathf.Clamp(newEnergyamount, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            if (energyBar)
            {
                energyBar.fillAmount = EnergyAsPercent;
            }
            if (playerEnergybarText != null)
            {
                playerEnergybarText.text = Mathf.Round(currentEnergyPoints) + "/" + maxEnergyPoints;
            }
        }
    }
}