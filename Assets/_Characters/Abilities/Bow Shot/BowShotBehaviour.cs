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
            var characterAttackSystem = GetComponent<AttackSystem>();

            characterAttackSystem.Attack(useParams);
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as BowShotConfig).Damage.Value;
            var projectilePrefab = (ability as BowShotConfig).ProjectilePrefab;
            var animationName = (ability as BowShotConfig).AnimationName;
            var reliantStat = (ability as BowShotConfig).ReliantStat;
            var statMultiplier = (ability as BowShotConfig).StatMultiplier;
            var animationTrigger = (ability as BowShotConfig).AnimationTrigger;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab, ability, reliantStat, statMultiplier, animationTrigger);

            return useParams;
        }
    }
}