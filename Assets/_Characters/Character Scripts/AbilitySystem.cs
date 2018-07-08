using UnityEngine;

namespace RPG.Characters
{
    public class AbilitySystem : MonoBehaviour
    {
        [SerializeField] Ability[] abilities;

        void Start()
        {
            AttachInitialAbilities();
        }

        void Update()
        {

        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public void AttempAbility(int abilityIndex, GameObject target = null)
        {
            abilities[abilityIndex].Use(target);
            //var energyComponent = GetComponent<SpecialAbilities>();
            //var energyCost = abilities[abilityIndex].GetEnergyCost();

            //if (energyCost <= currentEnergyPoints)
            //{
            //    ConsumeEnergy(energyCost);
            //    abilities[abilityIndex].Use(target);
            //}
            //else
            //{
            //    audioSource.PlayOneShot(outOfEnergy);
            //}
        }
    }
}