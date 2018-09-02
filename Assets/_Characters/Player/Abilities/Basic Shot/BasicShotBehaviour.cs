using UnityEngine;

namespace RPG.Characters
{
    public class BasicShotBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            StartAttack(target);
        }

        void StartAttack(GameObject target)
        {
            var useParams = GetUseParams(target);
            var characterAttackController = GetComponent<AttackController>();

            characterAttackController.Attack(useParams);
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as BasicShotConfig).Damage.Value;
            var projectilePrefab = (ability as BasicShotConfig).ProjectilePrefab;
            var animationName = (ability as BasicShotConfig).AnimationName;
            var reliantStat = (ability as BasicShotConfig).ReliantStat;
            var statMultiplier = (ability as BasicShotConfig).StatMultiplier;
            var animationTrigger = (ability as BasicShotConfig).AnimationTrigger;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab, ability, reliantStat, statMultiplier, animationTrigger);

            return useParams;
        }
    }
}