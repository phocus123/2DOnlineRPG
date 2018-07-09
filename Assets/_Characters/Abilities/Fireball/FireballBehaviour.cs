using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class FireballBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            StartAttack(target);
        }

        void StartAttack(GameObject target)
        {
            var useParams = GetUseParams(target);
            attackRoutine = StartCoroutine(ProjectileAttack(useParams));
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as FireballConfig).Damage.Value;
            var projectilePrefab = (ability as FireballConfig).ProjectilePrefab;
            var animationName = (ability as FireballConfig).Weapon.AnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab);

            return useParams;
        }
    }
}