﻿using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Characters
{
    public class DamageSystem : MonoBehaviour
    {
        AbilityUseParams abilityUseParams;
        CharacterStats characterStats;

        public void DealDamage(AbilityUseParams useParams)
        {
            abilityUseParams = useParams;
            characterStats = abilityUseParams.ability.Behaviour.Character.GetComponent<CharacterStats>();
            var enemyHealthSystem = useParams.target.GetComponent<HealthSystem>();
            float primaryStatDamage = CalculatePrimaryStatMultiplier();
            float finalDamage = primaryStatDamage - GetArmourValue(abilityUseParams.target);
            var uiManager = FindObjectOfType<UIManager>();

            enemyHealthSystem.TakeDamage(finalDamage);
            uiManager.TriggerCombatText(enemyHealthSystem.gameObject.transform.position, finalDamage);
        }

        float CalculatePrimaryStatMultiplier()
        {
            var player = abilityUseParams.ability.Behaviour.Character.GetComponent<PlayerControl>();
            var finalValue = 0f;

            if (player)
            {
                float baseDamage = abilityUseParams.baseDamage;
                PrimaryStat reliantStat = abilityUseParams.reliantStat;
                float statMultiplier = abilityUseParams.statMultiplier;
                var reliantStatValue = Array.Find(characterStats.PrimaryStats, x => x.name == reliantStat.name).Value;
                finalValue = (reliantStatValue * statMultiplier) + baseDamage;
            }
            else
            {
                finalValue = abilityUseParams.baseDamage;
            }

            return finalValue;
        }

        float GetArmourValue(GameObject target)
        {
            var targetStats = target.GetComponent<CharacterStats>();
            return targetStats.Armour.Value;
        }
    }
}