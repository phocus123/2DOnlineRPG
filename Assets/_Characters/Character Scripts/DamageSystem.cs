using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

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
            var player = abilityUseParams.ability.Behaviour.Character.GetComponent<PlayerControl>();
            var finalValue = 0f;

            if (player != null)
            {
                float baseDamage = abilityUseParams.baseDamage;
                CharacterStat reliantStat = abilityUseParams.reliantStat;
                float statMultiplier = abilityUseParams.statMultiplier;
                var reliantStatValue = player.CharacterStats.Find(x => x.name == reliantStat.name).Value;
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