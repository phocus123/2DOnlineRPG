using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class DaggerSlashBehaviour : AbilityBehaviour
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
            var damage = (ability as DaggerSlashConfig).Damage.Value;
            var animationName = (ability as DaggerSlashConfig).AnimationName;
            var reliantStat = (ability as DaggerSlashConfig).ReliantStat;
            var statMultiplier = (ability as DaggerSlashConfig).StatMultiplier;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, null, ability, reliantStat, statMultiplier, null);

            return useParams;
        }
    }
}