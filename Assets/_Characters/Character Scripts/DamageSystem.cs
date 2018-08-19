using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Characters
{
    public class DamageSystem : MonoBehaviour
    {
        AbilityUseParams abilityUseParams;

        public void DealDamage(AbilityUseParams useParams)
        {
            abilityUseParams = useParams;
            var enemyHealthSystem = useParams.target.GetComponent<HealthSystem>();
            var damageValue = CalculateFinalDamage();
            var uiManager = FindObjectOfType<UIManager>();

            enemyHealthSystem.TakeDamage(damageValue);
            uiManager.TriggerCombatText(enemyHealthSystem.gameObject.transform.position, damageValue);
        }

        float CalculateFinalDamage()
        {
            var player = abilityUseParams.ability.Behaviour.Character.GetComponent<CharacterStats>();
            var finalValue = 0f;

            if (player != null)
            {
                float baseDamage = abilityUseParams.baseDamage;
                PrimaryStat reliantStat = abilityUseParams.reliantStat;
                float statMultiplier = abilityUseParams.statMultiplier;
                var reliantStatValue = Array.Find(player.PrimaryStats, x => x.name == reliantStat.name).Value;
                finalValue = (reliantStatValue * statMultiplier) + baseDamage;
            }
            else
            {
                finalValue = abilityUseParams.baseDamage;
            }

            return finalValue;
        }
    }
}