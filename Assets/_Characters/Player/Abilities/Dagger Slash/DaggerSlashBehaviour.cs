using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class DaggerSlashBehaviour : AbilityBehaviour
    {
        private void Update()
        {
            StopAttackIfMoving();
        }

        public override void Use(GameObject target)
        {
            if (target != null)
                currentTarget = target.GetComponent<Character>();
            var useParams = GetUseParams(target);

            if (GetComponent<PlayerControl>())
            {
                if (PerformCombatChecks(out messageType, PopulateCombatChecks()))
                {
                    attackRoutine = StartCoroutine(PerformDaggerSlash(useParams));
                    currentTarget = null;
                }
                else
                    GameManager.Instance.alertMessageController.TriggerAlert(messageType);
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(PerformDaggerSlash(useParams));
            }
        }

        List<CombatCheck> PopulateCombatChecks()
        {
            if (NoTarget)
            {
                List<CombatCheck> combatChecks = new List<CombatCheck>
                {
                    new CombatCheck(NoTarget, "NoTarget")
                };
                return combatChecks;
            }
            else
            {
                List<CombatCheck> combatChecks = new List<CombatCheck>
                {
                    new CombatCheck(TargetNotInRange, "TargetNotInRange"),
                    new CombatCheck(TargetNotInLineOfSight, "TargetNotInLineOfSight"),
                    new CombatCheck(CorrectWeaponNotEquipped, "CorrectWeaponNotEquipped"),
                    new CombatCheck(TargetNotAlive, "TargetNotAlive"),
                    new CombatCheck(CharacterNotAlive, "CharacterIsAlive"),
                    new CombatCheck(IsAttacking, "NotCurrentlyAttacking"),
                    new CombatCheck(IsMoving, "NotCurrentlyMoving"),
                    new CombatCheck(AbilityCooldownActive, "AbilityCooldownInactive")
                };
                return combatChecks;
            }
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as DaggerSlashConfig).Damage.Value;
            var animationName = (ability as DaggerSlashConfig).AnimationName;
            var reliantStat = (ability as DaggerSlashConfig).ReliantStat;
            var statMultiplier = (ability as DaggerSlashConfig).StatMultiplier;
            var hitAnimationName = (ability as DaggerSlashConfig).HitAnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, null, ability, reliantStat, statMultiplier, hitAnimationName);

            return useParams;
        }

        protected IEnumerator PerformDaggerSlash(AbilityUseParams useParams)
        {
            var enemyHitboxAnimator = useParams.target.transform.GetChild(0).GetComponent<Animator>();

            Character.characterAnimationController.StartAbilityAnimation(ability.AnimationName);
            yield return animationDelayRoutine = StartCoroutine(AnimationDelay(ANIMATION_DELAY, enemyHitboxAnimator, (ability as DaggerSlashConfig).HitAnimationName));
            Character.damageController.DealDamage(useParams);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            StopAttack(ability.AnimationName);
        }

    }
}