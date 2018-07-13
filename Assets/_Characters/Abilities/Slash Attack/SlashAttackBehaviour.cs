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
            attackRoutine = StartCoroutine(MeleeAttack(useParams));
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as SlashAttackConfig).Damage.Value;
            var animationName = (ability as SlashAttackConfig).Weapon.AnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, null);

            return useParams;
        }
    }
}