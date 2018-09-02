using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class AbilityController : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] Ability[] abilities;

        [Header("Energy")]
        [SerializeField] Image energyBar;
        [SerializeField] Text playerEnergybarText;

        [Header("Enemy Target Frame")]
        [SerializeField] Image targetFrameEnergybar;
        [SerializeField] Text targetEnergybarText;

        CharacterManager characterManager;
        List<AbilityBehaviour> equippedAbilityBehaviours = new List<AbilityBehaviour>();
        float currentEnergyPoints;

        public Ability[] Abilities { get { return abilities; } }
        public List<AbilityBehaviour> EquippedAbilityBehaviours { get { return equippedAbilityBehaviours; } }
        public float MaxEnergyPoints { get { return characterManager.MaxEnergyPoints; } }
        float EnergyAsPercent { get { return currentEnergyPoints / characterManager.MaxEnergyPoints; } }

        public float CurrentEnergyPoints
        {
            get { return currentEnergyPoints; }
            set { currentEnergyPoints = Mathf.Clamp(value, 0, characterManager.MaxEnergyPoints); }
        }

        public event Action<AbilityController> OnEnergyChanged = delegate { };

        void Start()
        {
            characterManager = GetComponent<CharacterManager>();

            AttachInitialAbilities();
            currentEnergyPoints = characterManager.MaxEnergyPoints;
            UpdateEnergyBar();
        }

        private void Update()
        {
            UpdateEnergyBar();
        }

        public void Initialize(float currentValue)
        {
            currentEnergyPoints = currentValue;
            targetEnergybarText.text = Mathf.Round(currentEnergyPoints) + "/" + characterManager.MaxEnergyPoints;
            UpdateTargetFrameEnergybar();
        }

        public void AttemptAbility(AbilityBehaviour abilityBehaviour, GameObject target = null)
        {
            var energyComponent = GetComponent<AbilityController>();
            var energyCost = abilityBehaviour.Ability.Energy.Value;

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
                targetEnergybarText.text = Mathf.Round(currentEnergyPoints) + "/" + characterManager.MaxEnergyPoints;
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
            OnEnergyChanged(this);

            float newEnergyamount = currentEnergyPoints - energyCost;
            currentEnergyPoints = Mathf.Clamp(newEnergyamount, 0, characterManager.MaxEnergyPoints);
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
                playerEnergybarText.text = Mathf.Round(currentEnergyPoints) + "/" + characterManager.MaxEnergyPoints;
            }
            OnEnergyChanged(this);
        }
    }
}