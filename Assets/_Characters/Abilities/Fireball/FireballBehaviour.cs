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
            var characterAttackSystem = GetComponent<AttackSystem>();

            characterAttackSystem.Attack(useParams);
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as FireballConfig).Damage.Value;
            var projectilePrefab = (ability as FireballConfig).ProjectilePrefab;
            var animationName = (ability as FireballConfig).Weapon.AnimationName;
            var reliantStat = (ability as FireballConfig).ReliantStat;
            var statMultiplier = (ability as FireballConfig).StatMultiplier;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab, ability, reliantStat, statMultiplier);

            return useParams;
        }
    }
}