using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Characters
{
    public class DamageController : MonoBehaviour
    {
        AbilityUseParams abilityUseParams;
        CharacterManager characterManager;

        public void DealDamage(AbilityUseParams useParams)
        {
            abilityUseParams = useParams;
            characterManager = abilityUseParams.ability.Behaviour.Character.GetComponent<CharacterManager>();
            var enemyHealthController = useParams.target.GetComponent<HealthController>();
            float primaryStatDamage = CalculatePrimaryStatMultiplier();
            float finalDamage = primaryStatDamage - GetArmourValue(abilityUseParams.target);
            var uiManager = FindObjectOfType<UIManager>();

            enemyHealthController.TakeDamage(finalDamage);
            uiManager.TriggerCombatText(enemyHealthController.gameObject.transform.position, finalDamage);
        }

        float CalculatePrimaryStatMultiplier()
        {
            var player = abilityUseParams.ability.Behaviour.Character.GetComponent<PlayerControl>();
            var finalValue = 0f;

            if (player)
            {
                float baseDamage = abilityUseParams.baseDamage;
                CharacterStat reliantStat = abilityUseParams.reliantStat;
                float statMultiplier = abilityUseParams.statMultiplier;
                var reliantStatValue = Array.Find(characterManager.CharacterrStats, x => x.name == reliantStat.name).Value;
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
            var targetStats = target.GetComponent<CharacterManager>().CharacterrStats;
            var armourStat = Array.Find(targetStats, x => x.name == "Armour");
            return armourStat.Value;
        }
    }
}