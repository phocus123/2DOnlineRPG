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
            var characterAttackController = GetComponent<AttackController>();

            characterAttackController.Attack(useParams);
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            float damage = (ability as FireballConfig).Damage.Value;
            GameObject projectilePrefab = (ability as FireballConfig).ProjectilePrefab;
            string animationName = (ability as FireballConfig).AnimationName;
            CharacterStat reliantStat = (ability as FireballConfig).ReliantStat;
            float statMultiplier = (ability as FireballConfig).StatMultiplier;
            string animationTrigger = (ability as FireballConfig).AnimationTrigger;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab, ability, reliantStat, statMultiplier, animationTrigger);

            return useParams;
        }
    }
}