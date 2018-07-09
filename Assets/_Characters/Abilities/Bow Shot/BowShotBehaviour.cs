using UnityEngine;

namespace RPG.Characters
{
    public class BowShotBehaviour : AbilityBehaviour
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
            var damage = (ability as BowShotConfig).Damage.Value;
            var projectilePrefab = (ability as BowShotConfig).ProjectilePrefab;
            var animationName = (ability as BowShotConfig).Weapon.AnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab);

            return useParams;
        }
    }
}