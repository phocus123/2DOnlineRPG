using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class SlashAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            StartAttack(target);
        }

        void StartAttack(GameObject target)
        {
            var useParams = GetUseParams(target);
            var characterAttackSystem = GetComponent<AttackSystem>();

            characterAttackSystem.Attack(useParams);
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as SlashAttackConfig).Damage.Value;
            var animationName = (ability as SlashAttackConfig).AnimationName;
            var reliantStat = (ability as SlashAttackConfig).ReliantStat;
            var statMultiplier = (ability as SlashAttackConfig).StatMultiplier;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, null, ability, reliantStat, statMultiplier, null);

            return useParams;
        }
    }
}